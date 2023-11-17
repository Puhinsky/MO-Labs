using Function;
using Range = Function.Range;

namespace Algorithms
{
    public class MidPoint : Minimizator
    {
        private double _diffValue;
        private Range _range;

        public MidPoint(MinimizationTask task) : base(task)
        {
            Report.Algorithm = "Метод средней точки";
        }

        protected override void Init()
        {
            _range = Range;
        }

        protected override void OnIteration()
        {
            if (_diffValue > 0)
                _range.CropRight(_range.Middle());
            else
                _range.CropLeft(_range.Middle());
        }

        protected override void OnPostTermination()
        {
            MinPoint = CalculateFunction(_range.Middle(), 0);
        }

        protected override bool TerminationCondition()
        {
            _diffValue = CalculateFunction(_range.Middle(), 1).Y;

            return Math.Abs(_diffValue) <= Epsilon;
        }
    }
}
