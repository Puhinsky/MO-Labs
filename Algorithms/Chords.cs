using Function;
using Range = Function.Range;

namespace Algorithms
{
    public class Chords : Minimizator
    {
        private Point _diffValue;
        private double _diffA;
        private double _diffB;
        private Range _range;

        public Chords(MinimizationTask task) : base(task)
        {
            Report.Algorithm = "Метод хорд";
        }

        protected override void Init()
        {
            _range = Range;
            CalculateDiffA();
            CalculateDiffB();
            CalculateDiffX();
        }

        protected override void OnIteration()
        {
            if (_diffValue.Y > 0)
            {
                _range.CropLeft(_diffValue.X);
                CalculateDiffA();
            }
            else
            {
                _range.CropRight(_diffValue.X);
                CalculateDiffB();
            }

            CalculateDiffX();
        }

        protected override void OnPostTermination()
        {
            MinPoint = CalculateFunction(_diffValue.X, 0);
        }

        protected override bool TerminationCondition()
        {
            return Math.Abs(_diffValue.Y) <= Epsilon;
        }

        private void CalculateDiffX()
        {
            var x = _range.Min - _diffA / (_diffB - _diffA) * (_range.Max - _range.Min);
            _diffValue = CalculateFunction(x, 1);
        }

        private void CalculateDiffA()
        {
            _diffA = CalculateFunction(_range.Min, 1).Y;
        }

        private void CalculateDiffB()
        {
            _diffB = CalculateFunction(_range.Max, 1).Y;
        }
    }
}
