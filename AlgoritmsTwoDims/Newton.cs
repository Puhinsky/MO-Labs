namespace AlgoritmsTwoDims
{
    public class Newton : MinimizatorTwoDims
    {
        public Newton()
        {
            Report.Algorithm = "Метод Ньютон";
        }

        protected override bool TerminationCondition()
        {
            throw new NotImplementedException();
        }
    }
}
