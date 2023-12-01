using Function;

namespace Algorithms
{
    public class NewtonRaphson : Newton
    {
        public NewtonRaphson(int maxIterationCount) : base(maxIterationCount)
        {
            Report.Algorithm = "Метод Ньютона-Рафсона";
        }

        protected override double CalculateNext(double previousX)
        {
            var deriv1 = CalculateFunction(previousX, 1).Y;
            var deriv2 = CalculateFunction(previousX, 2).Y;

            var tau = CalculateTau(previousX, deriv1, deriv2);

            return previousX - tau * deriv1 / deriv2;
        }

        private double CalculateTau(double previousX, double deriv1, double deriv2)
        {
            var deriv1_pow2 = Math.Pow(deriv1, 2);

            return deriv1_pow2 / (deriv1_pow2 + Math.Pow(CalculateFunction(CalculateX(previousX, deriv1, deriv2), 1).Y, 2));
        }

        private static double CalculateX(double previousX, double deriv1, double deriv2)
        {
            return previousX - deriv1 / deriv2;
        }
    }
}
