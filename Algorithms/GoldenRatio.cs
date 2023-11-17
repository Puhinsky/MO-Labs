using Function;
using Range = Function.Range;

namespace Algorithms
{
    public class GoldenRatio : Minimizator
    {
        private static readonly double _tau = (Math.Sqrt(5) - 1) / 2;

        private Range _range;
        private Point _left;
        private Point _right;

        public GoldenRatio(MinimizationTask task) : base(task)
        {
            Report.Algorithm = "Метод золотого сечения";
        }

        protected override void Init()
        {
            _range = Range;
            _left = CalculateLeft();
            _right = CalculateRight();
        }

        protected override void OnIteration()
        {
            if (_left.Y > _right.Y)
            {
                _range.CropLeft(_left.X);
                _left = _right;
                _right = CalculateRight();
            }
            else
            {
                _range.CropRight(_right.X);
                _right = _left;
                _left = CalculateLeft();
            }
        }

        protected override void OnPostTermination()
        {
            MinPoint = CalculateFunction(_range.Middle(), 0);
        }

        protected override bool TerminationCondition()
        {
            return _range.Delta() / 2 < Epsilon;
        }

        private Point CalculateLeft() => CalculateFunction(_range.Min + (1 - _tau) * _range.Delta(), 0);
        private Point CalculateRight() => CalculateFunction(_range.Min + _tau * _range.Delta(), 0);
    }
}
