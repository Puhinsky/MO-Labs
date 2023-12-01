using Algorithms;
using Function;
using FunctionTwoDims;
using Range = Function.Range;

namespace AlgoritmsTwoDims
{
    public class SteepestDescent<SM> : MinimizatorTwoDims where SM : Minimizator, new()
    {
        private double[] _gradient;
        private double[] _x;
        private double _alpha;

        private readonly MinimizationTask _singleTask;
        private readonly SM _singleMinimizator = new();

        public SteepestDescent()
        {
            _gradient = new double[2];
            _x = new double[2];

            Report.Algorithm = "Наискорейший спуск";
            _singleTask = new MinimizationTask(
                    new TargetFunction[]
                    {
                        (x) => new Point(x, CalculateFunction(_x.Substract(_gradient.MultiplyConstant(x))).Y)
                    },
                    new Range()
                    {
                        Min = 0,
                        Max = double.MaxValue
                    });
        }

        protected override void Init()
        {
            _singleTask.Epsilon = Epsilon;
            StartX!.CopyTo(_x, 0);
        }

        protected override void OnIteration()
        {
            CalculateAlpha();
            _x = _x.Substract(_gradient.MultiplyConstant(_alpha));
        }

        protected override void OnPostTermination()
        {
            MinPoint = CalculateFunction(_x);
        }

        protected override bool TerminationCondition()
        {
            _gradient = CalculateGradient(_x);

            return _gradient.CalculateLenght() < Epsilon;
        }

        private void CalculateAlpha()
        {
            _singleMinimizator.TryGetMin(_singleTask);
            _alpha = _singleMinimizator.Report.Min.X;
        }
    }
}