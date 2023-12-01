using AlgoritmsTwoDims;
using FunctionTwoDims;

double a = 10d;

MinimizationTaskTwoDims task = new(
    new TargetFunctionTwoDims((double x1, double x2) =>
    {
        return new PointTwoDims()
        {
            X1 = x1,
            X2 = x2,
            Y = Math.Pow(x1, 2) + a * Math.Pow(x2, 2) 
        };
    }),
    0.001d,
    new TargetFunctionTwoDims[,]
    {
        {
            new TargetFunctionTwoDims((double x1, double x2) =>
            {
                return new PointTwoDims()
                {
                    X1 = x1,
                    X2 = x2,
                    Y = 2 * x1
                };
            }),
            new TargetFunctionTwoDims((double x1, double x2) =>
            {
                return new PointTwoDims()
                {
                    X1 = x1,
                    X2 = x2,
                    Y = 2 * a * x2
                };
            })
        }
    });

SteepestDescent steepestDescent = new(task);
steepestDescent.TryGetMin(1, 1);
Console.WriteLine($"{steepestDescent.Report.Min.X1} {steepestDescent.Report.Min.X2} {steepestDescent.Report.Min.Y} {steepestDescent.Report.FunctionCalculations}");