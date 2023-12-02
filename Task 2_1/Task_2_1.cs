using Algorithms;
using AlgoritmsTwoDims;
using FunctionTwoDims;
using MathNet.Numerics.LinearAlgebra;
using Newton = AlgoritmsTwoDims.Newton;

double a = 1000;

MinimizationTaskTwoDims task = new(
    (Vector<double> x) => new PointTwoDims(x, Math.Pow(x[0], 2) + a * Math.Pow(x[1], 2)),
    0.00001d,
    new TargetFunctionTwoDims[][,]
    {
        new TargetFunctionTwoDims[,]
        {
            {
                (Vector<double> x) => new PointTwoDims(x, 2 * x[0]),
                (Vector<double> x) => new PointTwoDims(x, 2 * a * x[1])
            }
        },
        new TargetFunctionTwoDims[,]
        {
            {
                (Vector<double> x) => new PointTwoDims(x, 2),
                (Vector<double> x) => new PointTwoDims(x, 0)
            },
            {
                (Vector<double> x) => new PointTwoDims(x, 0),
                (Vector<double> x) => new PointTwoDims(x, 2 * a)
            }
        }
    });

MinimizationTaskTwoDims testTask = new(
    (Vector<double> x) => new PointTwoDims(x, 4 * Math.Pow(x[0], 2) + 3 * Math.Pow(x[1], 2) - 4 * x[0] * x[1] + x[0]),
    0.001d,
    new TargetFunctionTwoDims[][,]
    {
        new TargetFunctionTwoDims[,]
        {
            {
                (Vector<double> x) => new PointTwoDims(x, 8 * x[0] - 4 * x[1] + 1),
                (Vector<double> x) => new PointTwoDims(x, 6 * x[1] - 4 * x[0])
            }
        },
        new TargetFunctionTwoDims[,]
        {
            {
                (Vector <double> x) => new PointTwoDims(x, 8d),
                (Vector <double> x) => new PointTwoDims(x, -4d)
            },
            {
                (Vector <double> x) => new PointTwoDims(x, -4d),
                (Vector <double> x) => new PointTwoDims(x, 6)
            }
        }
    });

var startPoint0 = Vector<double>.Build.Dense(new[] { 0d, 0d });
var startPoint1 = Vector<double>.Build.Dense(new[] { 1d, 1d });

SteepestDescent<RadixSearch> steepestDescent = new();
steepestDescent.TryGetMin(startPoint1, task);
Console.WriteLine($"{steepestDescent.Report.Min.X[0]} {steepestDescent.Report.Min.X[1]} {steepestDescent.Report.Min.Y} {steepestDescent.Report.FunctionCalculations}");

ConjugateGradients<RadixSearch> conjugateGradients = new();
conjugateGradients.TryGetMin(startPoint1, task);
Console.WriteLine($"{conjugateGradients.Report.Min.X[0]} {conjugateGradients.Report.Min.X[1]} {conjugateGradients.Report.Min.Y} {conjugateGradients.Report.FunctionCalculations}");

Newton newton = new();
newton.TryGetMin(startPoint1, task);
Console.WriteLine($"{newton.Report.Min.X[0]} {newton.Report.Min.X[1]} {newton.Report.Min.Y} {newton.Report.FunctionCalculations}");