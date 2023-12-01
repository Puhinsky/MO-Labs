namespace FunctionTwoDims
{
    public delegate PointTwoDims TargetFunctionTwoDims(double[] x);

    public static class VectorExtensions
    {
        public static double CalculateLenght(this double[] vector)
        {
            return Math.Sqrt(vector.Sum(x => Math.Pow(x, 2)));
        }

        public static double[] Add(this double[] value, in double[] other)
        {
            var result = new double[value.Length];

            for (int i = 0; i < value.Length; i++)
            {
                result[i] = value[i] + other[i];
            }

            return result;
        }

        public static double[] Substract(this double[] value, in double[] other)
        {
            var result = new double[value.Length];

            for (int i = 0; i < value.Length; i++)
            {
                result[i] = value[i] - other[i];
            }

            return result;
        }

        public static double[] MultiplyConstant(this double[] value, in double constant)
        {
            var result = new double[value.Length];

            for (int i = 0; i < value.Length; i++)
            {
                result[i] = value[i] * constant;
            }

            return result;
        }
    }
}