using FunctionTwoDims;

namespace AlgoritmsTwoDims
{
    public abstract class MinimizatorTwoDims
    {
        protected MinimizationTaskTwoDims Task;

        protected double StartX1 { get; private set; }
        protected double StartX2 { get; private set; }

        protected PointTwoDims MinPoint;

        public ReportTwoDims Report;

        protected abstract bool TerminationCondition();
        protected virtual bool InterruptCondition() { return false; }
        protected virtual void Init() { }
        protected virtual void OnIteration() { }
        protected virtual void OnPostTermination() { }

        public MinimizatorTwoDims(MinimizationTaskTwoDims task)
        {
            Task = task;
        }

        private PointTwoDims CalculatePartialDeriv(double x1, double x2, int diff, int x)
        {
            return Task.PartialDerivs[diff, x].Point(x1, x2);
        }

        protected PointTwoDims CalculateFunction(double x1, double x2)
        {
            Report.FunctionCalculations++;

            return Task.Function.Point(x1, x2);
        }

        protected double[] CalculateGradient(double x1, double x2)
        {
            var gradient = new double[2];

            gradient[0] = CalculatePartialDeriv(x1, x2, 0, 0).Y;
            gradient[1] = CalculatePartialDeriv(x1, x2, 0, 1).Y;

            return gradient;
        }

        protected double Epsilon => Task.Epsilon;

        public bool TryGetMin(double x1, double x2)
        {
            StartX1 = x1;
            StartX2 = x2;
            Report.FunctionCalculations = 0;
            Init();

            bool isInterrupt = InterruptCondition();

            while (!TerminationCondition() && !isInterrupt)
            {
                OnIteration();
                isInterrupt = InterruptCondition();
            }

            OnPostTermination();
            Report.Min = MinPoint;

            return !isInterrupt;
        }
    }
}
