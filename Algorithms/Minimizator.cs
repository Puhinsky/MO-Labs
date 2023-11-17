using Algorithms;
using Function;
using Range = Function.Range;

namespace Algorithms
{
    public abstract class Minimizator
    {
        private readonly MinimizationTask _task;

        protected Point MinPoint;

        public Report Report;

        protected abstract bool TerminationCondition();
        protected virtual bool InterruptCondition() { return false; }
        protected virtual void Init() { }
        protected virtual void OnIteration() { }
        protected virtual void OnPostTermination() { }

        public Minimizator(MinimizationTask task)
        {
            _task = task;
        }

        protected Point CalculateFunction(double x, int diff)
        {
            Report.FunctionCalculations++;

            return _task.Function[diff].Point(x);
        }

        protected Range Range => _task.Range;
        protected double Epsilon => _task.Epsilon;

        public bool TryGetMin()
        {
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
