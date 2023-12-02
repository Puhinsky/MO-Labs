using MathNet.Numerics.LinearAlgebra;

namespace FunctionTwoDims
{
    public struct PointTwoDims
    {
        public Vector<double> X;
        public double Y;

        public PointTwoDims(Vector<double> x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
