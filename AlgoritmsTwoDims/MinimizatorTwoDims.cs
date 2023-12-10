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

        private PointTwoDims CalculatePartialDeriv(Vector<double> x, int diff, int column, int row)
        {
            Report.FunctionCalculations++;

            return _task!.PartialDerivs[diff][row, column](x);
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

            for (int column = 0; column < x.Count; column++)
            {
                for (int row = column; row < x.Count; row++)
                {
                    hessian[column, row] = CalculatePartialDeriv(x, 1, column, row).Y;
                }
            }

            for(int column = 1; column < x.Count; column++)
            {
                for (int row = 0; row < column; row++)
                {
                    hessian[column, row] = hessian[row, column];
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
            Report.Path = new();

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
