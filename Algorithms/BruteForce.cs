using Function;

namespace Algorithms
{
    public class BruteForce : Minimizator
    {
        private int _iterationsCount;
        private int _iterationIndex;
        private double _delta;

        private double CurrentX => Range.Min + _iterationIndex * _delta;

        public BruteForce()
        {
            Report.Algorithm = "Метод перебора";
        }

        protected override void Init()
        {
            _iterationIndex = 0;
            _iterationsCount = (int)Math.Ceiling(Range.Delta() / Epsilon) + 1;
            _delta = Range.Delta() / _iterationsCount;
            MinPoint = Next();
            _iterationIndex++;
        }


        protected override void OnIteration()
        {
            var currentPoint = Next();
            _iterationIndex++;

            if (MinPoint.Y > currentPoint.Y)
            {
                MinPoint = currentPoint;
            }
        }

        protected override bool TerminationCondition()
        {
            return _iterationIndex >= _iterationsCount;
        }

        private Point Next()
        {
            return CalculateFunction(CurrentX, 0);
        }
    }
}
