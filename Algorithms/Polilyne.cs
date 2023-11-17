using Function;

namespace Algorithms
{
    public class Polilyne : Minimizator
    {
        private double _lipschitz = 1;

        private readonly List<Point> _pairs = new();
        private Point _minPair;
        private double _delta;

        public Polilyne(MinimizationTask task) : base(task)
        {
            Report.Algorithm = "Метод ломаных";
        }

        public bool TryGetMin(double lipschitz)
        {
            _lipschitz = lipschitz;

            return TryGetMin();
        }

        protected override void Init()
        {
            _pairs.Clear();
            _pairs.Add(CalculateInitPoint());

            _minPair = _pairs.MinBy(x => x.Y);
            MinPoint = CalculateFunction(_minPair.X, 0);
        }

        protected override void OnIteration()
        {
            var p = 0.5d * (MinPoint.Y + _minPair.Y);
            _pairs.Add(new Point()
            {
                X = _minPair.X - _delta,
                Y = p
            });
            _pairs.Add(new Point()
            {
                X = _minPair.X + _delta,
                Y = p
            });
            _pairs.Remove(_minPair);

            _minPair = _pairs.MinBy(x => x.Y);
            MinPoint = CalculateFunction(_minPair.X, 0);
        }

        protected override bool TerminationCondition()
        {
            _delta = 1 / (2 * _lipschitz) * (MinPoint.Y - _minPair.Y);

            return 2 * _lipschitz * _delta <= Epsilon;
        }

        private Point CalculateInitPoint()
        {
            var fA = CalculateFunction(Range.Min, 0).Y;
            var fB = CalculateFunction(Range.Max, 0).Y;

            return new Point()
            {
                X = 1d / (2d * _lipschitz) * (fA - fB + _lipschitz * (Range.Min + Range.Max)),
                Y = 0.5d * (fA + fB + _lipschitz * (Range.Min - Range.Max))
            };
        }
    }
}
