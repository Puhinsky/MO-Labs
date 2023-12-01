using FunctionTwoDims;

namespace AlgoritmsTwoDims
{
    public abstract class MinimizatorTwoDims
    {
        private MinimizationTaskTwoDims? _task;

        protected double[]? StartX { get; private set; }
        protected int DimensionsCount => StartX!.Length;

        protected PointTwoDims MinPoint;

        public ReportTwoDims Report;

        protected abstract bool TerminationCondition();
        protected virtual bool InterruptCondition() { return false; }
        protected virtual void Init() { }
        protected virtual void OnIteration() { }
        protected virtual void OnPostTermination() { }

        private PointTwoDims CalculatePartialDeriv(double[] x, int diff, int variableColumn, int variableRow)
        {
            return _task!.PartialDerivs[diff][variableRow, variableColumn](x);
        }

        protected PointTwoDims CalculateFunction(double[] x)
        {
            Report.FunctionCalculations++;

            return _task!.Function(x);
        }

        protected double[] CalculateGradient(double[] x)
        {
            var gradient = new double[x.Length];

            for (int i = 0; i < x.Length; i++)
            {
                gradient[i] = CalculatePartialDeriv(x, 0, i, 0).Y;
            }

            return gradient;
        }

        protected double Epsilon => _task!.Epsilon;

        public bool TryGetMin(double[] x, MinimizationTaskTwoDims task)
        {
            _task = task;
            StartX = x;
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
