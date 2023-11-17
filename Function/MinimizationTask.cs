namespace Function
{
    public class MinimizationTask
    {
        public TargetFunction[] Function { get; private set; }
        public Range Range { get; private set; }
        public double Epsilon { get; set; }

        public MinimizationTask(TargetFunction[] function, Range range, double epsilon)
        {
            Function = function;
            Range = range;
            Epsilon = epsilon;
        }
    }
}
