namespace FunctionTwoDims
{
    public class TargetFunctionTwoDims
    {
        public Func<double, double, PointTwoDims> Point { get; private set; }
        public Func<double, double, double> Value { get; private set; }

        public TargetFunctionTwoDims(Func<double, double, PointTwoDims> point)
        {
            Point = point;
            Value = (double x1, double x2) => Point(x1, x2).Y;
        }
    }

    public static class VectorExtensions
    {
        public static double CalculateLenght(this double[] vector)
        {
            return Math.Sqrt(vector.Sum(x=>Math.Pow(x, 2)));
        }
    }
}