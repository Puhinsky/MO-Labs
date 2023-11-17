using Function;

namespace Algorithms
{
    public class Parabola : Minimizator
    {
        private readonly double _offset = 0.1d;
        private readonly Point[] _parabola = new Point[3];

        private Point _previousMin;

        private double Delta => Math.Abs(MinPoint.X - _previousMin.X);

        public Parabola(MinimizationTask task) : base(task)
        {
            Report.Algorithm = "Метод параболы";
        }

        protected override void Init()
        {
            FindPoints();
            _previousMin = CalculateParabolaMin();
            InsertPoint(_previousMin);
            MinPoint = CalculateParabolaMin();
        }

        protected override void OnIteration()
        {
            _previousMin = MinPoint;
            InsertPoint(_previousMin);
            MinPoint = CalculateParabolaMin();
        }

        protected override bool TerminationCondition()
        {
            return Delta < Epsilon;
        }

        private void FindPoints()
        {
            _parabola[0].X = Range.Min;
            _parabola[2].X = Range.Max;
            _parabola[1].X = _parabola[0].X + _offset;

            for (int i = 0; i < _parabola.Length; i++)
            {
                _parabola[i] = CalculateFunction(_parabola[i].X, 0);
            }

            if (IsParabolic(_parabola))
                return;

            _parabola[1].X = _parabola[2].X - _offset;
            _parabola[1] = CalculateFunction(_parabola[1].X, 0);
        }

        private static bool IsParabolic(Point[] p)
        {
            return p[0].X < p[1].X && p[1].X < p[2].X
                && p[0].Y >= p[1].Y && p[1].Y <= p[2].Y;
        }

        private Point CalculateParabolaMin()
        {
            var a1 = _parabola[1].Diff(_parabola[0]);
            var a2 = 1 / (_parabola[2].X - _parabola[1].X) *
                (_parabola[2].Diff(_parabola[0]) - _parabola[1].Diff(_parabola[0]));
            var x = (_parabola[0].X + _parabola[1].X - a1 / a2) / 2;

            return CalculateFunction(x, 0);
        }

        private void InsertPoint(Point point)
        {
            if (point.X < _parabola[1].X)
            {
                if (point.Y > _parabola[1].Y)
                {
                    InsertLeft(point);

                    return;
                }

                InsertCenterLeft(point);

                return;
            }

            if (point.Y > _parabola[1].Y)
            {
                InsertRight(point);

                return;
            }

            InsertCenterRight(point);

            return;
        }

        private void InsertRight(Point point)
        {
            _parabola[2] = point;
        }

        private void InsertLeft(Point point)
        {
            _parabola[0] = point;
        }

        private void InsertCenterRight(Point point)
        {
            _parabola[0] = _parabola[1];
            _parabola[1] = point;
        }

        private void InsertCenterLeft(Point point)
        {
            _parabola[2] = _parabola[1];
            _parabola[1] = point;
        }
    }
}
