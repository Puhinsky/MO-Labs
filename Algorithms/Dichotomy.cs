using Function;
using Range = Function.Range;

namespace Algorithms
{
    public class Dichotomy : Minimizator
    {
        private Range _range;
        private double _sigma;

        public Dichotomy(MinimizationTask task) : base(task)
        {
            Report.Algorithm = "Дихтомия";
        }

        protected override void Init()
        {
            _sigma = Epsilon * 0.2d;
            _range = Range;
        }

        protected override void OnIteration()
        {
            var left = CalculateLeft();
            var right = CalculateRight();

            if (left.Y > right.Y)
                _range.CropLeft(right.X);
            else
                _range.CropRight(left.X);
        }

        protected override void OnPostTermination()
        {
            MinPoint = CalculateFunction(_range.Middle(), 0);
        }

        protected override bool TerminationCondition()
        {
            return _range.Delta() / 2 < Epsilon;
        }

        private Point CalculateLeft() => CalculateFunction(_range.Middle() - _sigma / 2, 0);
        private Point CalculateRight() => CalculateFunction(_range.Middle() + _sigma / 2, 0);
    }
}
