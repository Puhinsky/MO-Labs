using FunctionTwoDims;
using MathNet.Numerics.LinearAlgebra;

namespace AlgoritmsTwoDims
{
    public class Newton : MinimizatorTwoDims
    {
        private Vector<double>? _gradient;
        private Vector<double>? _x;

        public Newton()
        {
            Report.Algorithm = "Метод Ньютона";
        }

        protected override void Init()
        {
            _x = Vector<double>.Build.DenseOfVector(StartX);
            _gradient = CalculateGradient(_x!);
            Report.Path.Add(new List<Vector<double>>());
            Report.Path.First().Add(_x);
        }

        protected override void OnIteration()
        {
            _x -= CalculateHessian(_x!).Inverse() * _gradient;
            _gradient = CalculateGradient(_x!);
            Report.Path.First().Add(_x);
        }

        protected override bool TerminationCondition()
        {
            return _gradient!.L2Norm() < Epsilon;
        }

        protected override void OnPostTermination()
        {
            MinPoint = CalculateFunction(_x!);
        }
    }
}
