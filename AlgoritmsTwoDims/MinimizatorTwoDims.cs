using FunctionTwoDims;
using MathNet.Numerics.LinearAlgebra;

namespace AlgoritmsTwoDims
{
    public abstract class MinimizatorTwoDims
    {
        private MinimizationTaskTwoDims? _task;

        protected Vector<double>? StartX { get; private set; }

        protected PointTwoDims MinPoint;

        public ReportTwoDims Report;

        protected abstract bool TerminationCondition();
        protected virtual bool InterruptCondition() { return false; }
        protected virtual void Init() { }
        protected virtual void OnIteration() { }
        protected virtual void OnPostTermination() { }

        private PointTwoDims CalculatePartialDeriv(Vector<double> x, int diff, int variableColumn, int variableRow)
        {
            return _task!.PartialDerivs[diff][variableRow, variableColumn](x);
        }

        protected PointTwoDims CalculateFunction(Vector<double> x)
        {
            Report.FunctionCalculations++;

            return _task!.Function(x);
        }

        protected Vector<double> CalculateGradient(Vector<double> x)
        {
            var gradient = Vector<double>.Build.Dense(x.Count);

            for (int i = 0; i < gradient.Count; i++)
            {
                gradient[i] = CalculatePartialDeriv(x, 0, i, 0).Y;
            }

            return gradient;
        }

        protected Matrix<double> CalculateHessian(Vector<double> x)
        {
            var hessian = Matrix<double>.Build.Dense(x.Count, x.Count);

            for (int i = 0; i < x.Count; i++)
            {
                for (int j = 0; j < x.Count; j++)
                {
                    hessian[i, j] = CalculatePartialDeriv(x, 1, i, j).Y;
                }
            }

            return hessian;
        }

        protected double Epsilon => _task!.Epsilon;

        public bool TryGetMin(Vector<double> x, MinimizationTaskTwoDims task)
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
