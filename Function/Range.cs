namespace Function
{
    public struct Range
    {
        public double Min;
        public double Max;
    }

    public static class RangeExtensions
    {
        public static double Delta(this Range range)
        {
            return range.Max - range.Min;
        }

        public static double Middle(this Range range)
        {
            return (range.Max + range.Min) / 2;
        }

        public static void CropLeft(ref this Range range, double left)
        {
            range.Min = left;
        }

        public static void CropRight(ref this Range range, double right)
        {
            range.Max = right;
        }
    }
}
