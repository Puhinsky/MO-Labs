using Function;

namespace Algorithms
{
    public class Marquardt : Newton
    {
        private bool _isInitial;
        private double _mu;
        private Point _previousPoint;

        public Marquardt(MinimizationTask task, int maxIterationCount) : base(task, maxIterationCount) { }

        protected override void Init()
        {
            _isInitial = true;
            base.Init();
            _previousPoint = CalculateFunction(PreviousX, 0);
        }

        protected override void OnIteration()
        {
            _isInitial = false;
            UpdateMu();
            base.OnIteration();
        }

        protected override void OnPostTermination() { }

        protected override double CalculateNext(double previousX)
        {
            var deriv1 = CalculateFunction(previousX, 1).Y;
            var deriv2 = CalculateFunction(previousX, 2).Y;

            if (_isInitial)
                _mu = deriv2 * 10d;

            return previousX - deriv1 / (deriv2 + _mu);
        }

        private void UpdateMu()
        {
            MinPoint = CalculateFunction(X, 0);

            if (MinPoint.Y < _previousPoint.Y)
                _mu /= 2;
            else
                _mu *= 2;

            _previousPoint = MinPoint;
        }
    }
}
