using FunctionTwoDims;
using MathNet.Numerics.LinearAlgebra;

namespace AlgoritmsTwoDims
{
    public class RandomSearch : MinimizatorTwoDims
    {
        private Vector<double>? _x;
        private double _alpha = 1d;
        private double _gamma = 3d;
        private int _triesCount = 100;

        public RandomSearch()
        {
            Report.Algorithm = "Случайный поиск";
        }

        public bool TryGetMin(Vector<double> x, MinimizationTaskTwoDims task, double alpha, double gamma, int triesCount)
        {
            _alpha = alpha;
            _gamma = gamma;
            _triesCount = triesCount;

            return TryGetMin(x, task);
        }

        protected override void Init()
        {
            _x = Vector<double>.Build.DenseOfVector(StartX!);
            Report.Path.Add(new List<Vector<double>>());
            Report.Path.First().Add(_x);

            MinPoint = CalculateFunction(_x);
            Process();
        }

        protected override void OnIteration()
        {
            Process();
        }

        protected override bool TerminationCondition()
        {
           if(_alpha < Epsilon)
            {
                return true;
            }
            else
            {
                _alpha /= _gamma;

                return false;
            }
        }

        private void Process()
        {
            for (int i = 0; i < _triesCount; i++)
            {
                var testX = _x + _alpha * GetRandomDirection();
                var testPoint = CalculateFunction(testX);

                if (MinPoint.Y > testPoint.Y)
                {
                    _x = testX;
                    MinPoint = testPoint;
                    Report.Path.First().Add(_x);

                    return;
                }
            }
        }

        private Vector<double> GetRandomDirection()
        {
            return Vector<double>.Build.Random(_x!.Count).Normalize(1d);
        }
    }
}
