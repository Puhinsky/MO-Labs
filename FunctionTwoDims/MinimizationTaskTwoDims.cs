namespace FunctionTwoDims
{
    public class MinimizationTaskTwoDims
    {
        public TargetFunctionTwoDims Function { get; private set; }
        public TargetFunctionTwoDims[,] PartialDerivs { get; private set; }
        public double Epsilon { get; set; }

        public MinimizationTaskTwoDims(TargetFunctionTwoDims function, double epsilon, TargetFunctionTwoDims[,] partialDerivs)
        {
            Function = function;
            Epsilon = epsilon;
            PartialDerivs = partialDerivs;
        }
    }
}
