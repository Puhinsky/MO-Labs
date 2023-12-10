using Algorithms;
using Function;
using FunctionTwoDims;
using MathNet.Numerics.LinearAlgebra;
using Range = Function.Range;

namespace AlgoritmsTwoDims
{
    public class HookJeeves : MinimizatorTwoDims
    {
        private Vector<double>? _x;
        private Vector<double>[]? _basis;
        private Vector<double>? _delta;
        private double _gamma = 3d;
        private Vector<double>? _testX;

        private readonly RadixSearch _singleMinimizator = new();
        private readonly MinimizationTask _singleTask;
        private double _singleEpsilon = 0.0001d;

        public HookJeeves()
        {
            Report.Algorithm = "Метод Хука-Дживса";
            _singleTask = new(
                new TargetFunction[]
                {
                    (x) => new Point(x, CalculateFunction(_x + x * (_testX - _x)).Y)
                },
                new Range()
                {
                    Min = 0,
                    Max = double.MaxValue
                });
        }

        public bool TryGetMin(Vector<double> x, MinimizationTaskTwoDims task, Vector<double> delta, double gamma, double singleEpsilon)
        {
            _delta = delta;
            _gamma = gamma;
            _singleEpsilon = singleEpsilon;

            return TryGetMin(x, task);
        }

        protected override void Init()
        {
            _singleTask.Epsilon = _singleEpsilon;
            BuildBasis();
            _x = Vector<double>.Build.DenseOfVector(StartX!);
            Report.Path.Add(_x!);
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
            _testX = TestSearch();

            if (_testX != _x)
            {
                _singleMinimizator.TryGetMin(_singleTask);
                var alpha = _singleMinimizator.Report.Min.X;
                _x += alpha * (_testX - _x);
            }

            Report.Path.Add(_x!);
        }
    }
}
