using Algorithms;
using Function;
using FunctionTwoDims;
using Range = Function.Range;

namespace AlgoritmsTwoDims
{
    public class SteepestDescent : MinimizatorTwoDims
    {
        private double[] _gradient;
        private readonly double[] _x;
        private double _alpha;

        private MinimizationTask _singleTask;
        private RadixSearch _singleMinimizator;

        public SteepestDescent(MinimizationTaskTwoDims task) : base(task)
        {
            _gradient = new double[2];
            _x = new double[2];

            _singleMinimizator = new RadixSearch();
            _singleTask = new MinimizationTask(
                    new TargetFunction[]
                    {
                        (x) => new Point(x, CalculateFunction(_x[0] - x * _gradient[0], _x[1] - x * _gradient[1]).Y)
                    },
                    new Range()
                    {
                        Min = 0,
                        Max = double.MaxValue
                    },
                    Epsilon
                    );
        }

        protected override void Init()
        {
            _x[0] = StartX1;
            _x[1] = StartX2;
            MinPoint = CalculateFunction(_x[0], _x[1]);
        }

        protected override void OnIteration()
        {
            CalculateAlpha();
            CalculateX(0);
            CalculateX(1);
        }

        protected override void OnPostTermination()
        {
            MinPoint = CalculateFunction(_x[0], _x[1]);
        }

        protected override bool TerminationCondition()
        {
            _gradient = CalculateGradient(_x[0], _x[1]);

            return _gradient.CalculateLenght() < Epsilon;
        }

        private void CalculateAlpha()
        {
            _singleMinimizator.TryGetMin(_singleTask);
            _alpha = _singleMinimizator.Report.Min.X;        }

        private void CalculateX(int dimension)
        {
            _x[dimension] = _x[dimension] - _alpha * _gradient[dimension];
        }
    }
}