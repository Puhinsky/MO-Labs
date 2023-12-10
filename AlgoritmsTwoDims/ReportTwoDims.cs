using FunctionTwoDims;
using MathNet.Numerics.LinearAlgebra;

namespace AlgoritmsTwoDims
{
    public struct ReportTwoDims
    {
        public string Algorithm;
        public int FunctionCalculations;
        public PointTwoDims Min;
        public List<Vector<double>> Path;
    }
}
