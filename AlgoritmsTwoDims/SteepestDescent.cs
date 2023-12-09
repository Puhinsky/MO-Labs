using Algorithms;
using Function;
using FunctionTwoDims;
using MathNet.Numerics.LinearAlgebra;
using Range = Function.Range;

namespace AlgoritmsTwoDims
{
    public class SteepestDescent<SM> : MinimizatorTwoDims where SM : Minimizator, new()
    {
        private Vector<double>? _gradient;
        private Vector<double>? _x;
        private double _alpha;

        private readonly MinimizationTask _singleTask;
        private readonly SM _singleMinimizator = new();
        private double _singleEpsilon = 0.0001d;

        public SteepestDescent()
        {
            Report.Algorithm = "Наискорейший спуск";
            _singleTask = new MinimizationTask(
                    new TargetFunction[]
                    {
                        (x) => new Point(x, CalculateFunction(_x - x *_gradient).Y)
                    },
                    new Range()
                    {
                        Min = 0,
                        Max = double.MaxValue
                    });
        }

        public bool TryGetMin(Vector<double> x, MinimizationTaskTwoDims task, double singleEpsilon)
        {
            _singleEpsilon = singleEpsilon;

            return TryGetMin(x, task);
        }

        protected override void Init()
        {
            _x = Vector<double>.Build.DenseOfVector(StartX);
            _singleTask.Epsilon = _singleEpsilon;
        }

        protected override void OnIteration()
        {
            CalculateAlpha();
            _x -= _alpha * _gradient;
        }

        protected override void OnPostTermination()
        {
            MinPoint = CalculateFunction(_x!);
        }

        protected override bool TerminationCondition()
        {
            _gradient = CalculateGradient(_x!);

            return _gradient.L2Norm() < Epsilon;
        }

        private void CalculateAlpha()
        {
            _singleMinimizator.TryGetMin(_singleTask);
            _alpha = _singleMinimizator.Report.Min.X;
        }
    }
}