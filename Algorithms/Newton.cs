using Function;

namespace Algorithms
{
    public class Newton : Minimizator
    {
        private readonly int _maxIterationCount;
        private int _iterationCount;

        protected double StartX = 0;
        protected double PreviousX;
        protected double X;

        private double Delta => Math.Abs(X - PreviousX);

        public Newton(int maxIterationCount)
        {
            _maxIterationCount = maxIterationCount;
            Report.Algorithm = "Метод Ньютона";
        }

        public bool TryGetMin(MinimizationTask task, double startX)
        {
            StartX = startX;

            return TryGetMin(task);
        }

        protected override void Init()
        {
            _iterationCount = 0;
            PreviousX = StartX;
            X = CalculateNext(PreviousX);
        }

        protected override void OnIteration()
        {
            PreviousX = X;
            X = CalculateNext(PreviousX);
            _iterationCount++;
        }

        protected override void OnPostTermination()
        {
            MinPoint = CalculateFunction(X, 0);
        }

        protected override bool TerminationCondition()
        {
            return Delta < Epsilon;
        }

        protected override bool InterruptCondition()
        {
            return _iterationCount >= _maxIterationCount;
        }

        protected virtual double CalculateNext(double previousX)
        {
            return previousX - CalculateFunction(previousX, 1).Y / CalculateFunction(previousX, 2).Y;
        }
    }
}
