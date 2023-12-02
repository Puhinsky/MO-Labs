using MathNet.Numerics.LinearAlgebra;

namespace FunctionTwoDims
{
    public delegate PointTwoDims TargetFunctionTwoDims(Vector<double> x);

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

        public static double[,] Transpose(this double[,] value)
        {
            int numRows = value.GetLength(0);
            int numCols = value.GetLength(1);

            var result = new double[numCols, numRows];

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    result[j, i] = value[i, j];
                }
            }

            return result;
        }

        public static double[] MultiplyVector(this double[,] value, double[] other)
        {
            int numRows = value.GetLength(0);
            int numCols = value.GetLength(1);

            if (numCols != other.Length)
            {
                throw new ArgumentException("The number of columns in the matrix must be equal to the length of the vector.");
            }

            var result = new double[numRows];

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    result[i] += value[i, j] * other[j];
                }
            }

            return result;
        }
    }
}