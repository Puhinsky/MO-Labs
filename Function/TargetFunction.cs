namespace Function
{
    public class TargetFunction
    {
        public Func<double, Point> Point { get; private set; }
        public Func<double, double> Value { get; private set; }

        public TargetFunction(Func<double, Point> point)
        {
            Point = point;
            Value = (double value) => Point(value).Y;
        }
    }
}
