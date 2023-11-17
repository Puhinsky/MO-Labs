namespace Function
{
    public struct Point
    {
        public double X;
        public double Y;
    }

    public static class PointExtensions
    {
        public static double Diff(this Point point, Point other)
        {
            return (point.Y - other.Y) / (point.X - other.X);
        }
    }
}
