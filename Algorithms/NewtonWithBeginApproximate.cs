using Function;

namespace Algorithms
{
    public class NewtonWithBeginApproximate : Newton
    {
        private readonly GoldenRatio _approximateMinimizator;
        private readonly double _approximateEpsilon = 0.5d;

        private double _previousX;
        private double _x;

        private double Delta => Math.Abs(_x - _previousX);

        public NewtonWithBeginApproximate(MinimizationTask task, int maxIterationCount) : base(task, maxIterationCount)
        {
            Report.Algorithm = "Метод Ньютона";
            _approximateMinimizator = new GoldenRatio(new MinimizationTask(task.Function, task.Range, _approximateEpsilon));
        }

        protected override void Init()
        {
            _approximateMinimizator.TryGetMin();
            _previousX = _approximateMinimizator.Report.Min.X;
            Report.FunctionCalculations = _approximateMinimizator.Report.FunctionCalculations;
            CalculateNext();
        }

        protected override void OnIteration()
        {
            _previousX = _x;
            CalculateNext();
        }

        protected override bool TerminationCondition()
        {
            return Delta < Epsilon;
        }

        protected override void OnPostTermination()
        {
            MinPoint = CalculateFunction(_x, 0);
        }

        private void CalculateNext()
        {
            _x = _previousX - CalculateFunction(_previousX, 1).Y / CalculateFunction(_previousX, 2).Y;
        }
    }
}
