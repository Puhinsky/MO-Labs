using Function;

namespace Algorithms
{
    public class RadixSearch : Minimizator
    {
        private static readonly double _searchConstant = 0.25d;

        private double _delta;
        private bool _isMinValuePassed;

        public RadixSearch(MinimizationTask task) : base(task)
        {
            Report.Algorithm = "Поразрядный поиск";
        }

        protected override void Init()
        {
            _isMinValuePassed = false;
            _delta = _searchConstant;
            MinPoint = CalculateFunction(Range.Min, 0);
        }

        protected override void OnIteration()
        {
            if (_isMinValuePassed)
                ReverseSearch();

            var currentPoint = Next();

            if (MinPoint.Y < currentPoint.Y)
            {
                _isMinValuePassed = true;
            }
            else
            {
                MinPoint = currentPoint;
            }
        }

        protected override bool TerminationCondition()
        {
            return _isMinValuePassed && (Math.Abs(_delta) < Epsilon);
        }

        private Point Next()
        {
            return CalculateFunction(MinPoint.X + _delta, 0);
        }

        private void ReverseSearch()
        {
            _delta *= -1;
            _delta *= _searchConstant;
            _isMinValuePassed = false;
        }
    }
}
