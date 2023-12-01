using Algorithms;
using Function;
using Range = Function.Range;

namespace Algorithms
{
    public abstract class Minimizator
    {
        private MinimizationTask? _task;

        protected Point MinPoint;

        public Report Report;

        protected abstract bool TerminationCondition();
        protected virtual bool InterruptCondition() { return false; }
        protected virtual void Init() { }
        protected virtual void OnIteration() { }
        protected virtual void OnPostTermination() { }

        protected Point CalculateFunction(double x, int diff)
        {
            Report.FunctionCalculations++;

            return _task!.Function[diff](x);
        }

        protected Range Range => _task!.Range;
        protected double Epsilon => _task!.Epsilon;

        public bool TryGetMin(MinimizationTask task)
        {
            _task = task;
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
