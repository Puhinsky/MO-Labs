using FunctionTwoDims;
using MathNet.Numerics.LinearAlgebra;

namespace AlgoritmsTwoDims
{
    public class HookJeeves : MinimizatorTwoDims
    {
        private Vector<double>? _x;
        private Vector<double>[]? _basis;
        private Vector<double>? _delta;
        private readonly double _a = 2d;
        private double _gamma = 3d;

        public HookJeeves()
        {
            Report.Algorithm = "Метод Хука-Дживса";
        }

        public bool TryGetMin(Vector<double> x, MinimizationTaskTwoDims task, Vector<double> delta, double gamma)
        {
            _delta = delta;
            _gamma = gamma;

            return TryGetMin(x, task);
        }

        protected override void Init()
        {
            BuildBasis();
            _x = Vector<double>.Build.DenseOfVector(StartX!);
            Process();
        }

        protected override bool TerminationCondition()
        {
            if (_delta!.L2Norm() < Epsilon)
            {
                return true;
            }
            else
            {
                _delta /= _gamma;

                return false;
            }
        }

        protected override void OnIteration()
        {
            Process();
        }

        protected override void OnPostTermination()
        {
            MinPoint = CalculateFunction(_x!);
        }

        private void BuildBasis()
        {
            _basis = new Vector<double>[StartX!.Count];

            for (int i = 0; i < _basis.Length; i++)
            {
                _basis[i] = Vector<double>.Build.Dense(_basis.Length);
                _basis[i][i] = 1d;
            }
        }

        private Vector<double> TestSearch()
        {
            Vector<double> result = _x!;
            MinPoint = CalculateFunction(_x!);

            for (int i = 0; i < _basis!.Length; i++)
            {
                var testX = result - _delta![i] * _basis[i];

                if (MinPoint.Y > CalculateFunction(testX).Y)
                {
                    result = testX;
                }
                else
                {
                    testX = result + _delta![i] * _basis[i];

                    if (MinPoint.Y > CalculateFunction(testX).Y)
                        result = testX;
                }
            }

            return result;
        }

        private void Process()
        {
            var testX = TestSearch();

            if (testX != _x)
            {
                _x = _a * testX - _x;
            }
        }
    }
}
