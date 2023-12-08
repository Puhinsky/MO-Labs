using MathNet.Numerics.LinearAlgebra;
using System.Diagnostics.CodeAnalysis;

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

        public override readonly bool Equals([NotNullWhen(true)] object? obj)
        {
            var other = (PointTwoDims)obj!;

            return other.X == X && other.Y == Y;
        }

        public static bool operator ==(PointTwoDims left, PointTwoDims right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PointTwoDims left, PointTwoDims right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
