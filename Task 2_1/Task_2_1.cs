using Algorithms;
using AlgoritmsTwoDims;
using FunctionTwoDims;

double a = 1000d;

MinimizationTaskTwoDims task = new(
    (double[] x) => new PointTwoDims(x, Math.Pow(x[0], 2) + a * Math.Pow(x[1], 2)),
    0.001d,
    new TargetFunctionTwoDims[][,]
    {
        new TargetFunctionTwoDims[,]
        {
            {
                (double[] x) => new PointTwoDims(x, 2 * x[0]),
                (double[] x) => new PointTwoDims(x, 2 * a * x[1])
            }
        },
        new TargetFunctionTwoDims[,]
        {
            {
                (double[] x) => new PointTwoDims(x, 2),
                (double[] x) => new PointTwoDims(x, 0)
            },
            {
                (double[] x) => new PointTwoDims(x, 0),
                (double[] x) => new PointTwoDims(x, 2 * a)
            }
        }
    });

MinimizationTaskTwoDims testTask = new(
    (double[] x) => new PointTwoDims(x, 4 * Math.Pow(x[0], 2) + 3 * Math.Pow(x[1], 2) - 4 * x[0] * x[1] + x[0]),
    0.001d,
    new TargetFunctionTwoDims[][,]
    {
        new TargetFunctionTwoDims[,]
        {
            {
                (double[] x) => new PointTwoDims(x, 8 * x[0] - 4 * x[1] + 1),
                (double[] x) => new PointTwoDims(x, 6 * x[1] - 4 * x[0])
            }
        },
    });

SteepestDescent<RadixSearch> steepestDescent = new();
steepestDescent.TryGetMin(new double[] { 1, 1 }, task);
Console.WriteLine($"{steepestDescent.Report.Min.X[0]} {steepestDescent.Report.Min.X[1]} {steepestDescent.Report.Min.Y} {steepestDescent.Report.FunctionCalculations}");

ConjugateGradients<RadixSearch> conjugateGradients = new();
conjugateGradients.TryGetMin(new double[] { 1, 1 }, task);
Console.WriteLine($"{conjugateGradients.Report.Min.X[0]} {conjugateGradients.Report.Min.X[1]} {conjugateGradients.Report.Min.Y} {conjugateGradients.Report.FunctionCalculations}");