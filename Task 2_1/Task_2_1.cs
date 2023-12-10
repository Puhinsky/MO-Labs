using Algorithms;
using AlgoritmsTwoDims;
using CSV;
using Drawing;
using FunctionTwoDims;
using MathNet.Numerics.LinearAlgebra;
using Newton = AlgoritmsTwoDims.Newton;

double a = 10;

MinimizationTaskTwoDims task = new(
    (Vector<double> x) => new PointTwoDims(x, Math.Pow(x[0], 2) + a * Math.Pow(x[1], 2)),
    0.01d,
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

var parameters = new List<double> { 1d, 250d, 1000d };
var epsilons = new List<double> { 0.001d, 0.00001d };

var startPoint0 = Vector<double>.Build.Dense(new[] { 0d, 0d });
var startPoint1 = Vector<double>.Build.Dense(new[] { 1d, 1d });

string _savePath = "C:\\Users\\puhinsky\\source\\repos\\MO Labs\\Resources\\Lab2\\";
CsvFileWriter writer = new(_savePath, "Задание 1");

FunctionPlotter plotter = new("x1", "x2", 0.001d);
Function.Range plotRange = new() { Min = -5, Max = 5 };

List<List<ReportTwoDims>> reports = new();

SteepestDescent<RadixSearch> steepestDescent = new();
ConjugateGradients<RadixSearch> conjugateGradients = new();
Newton newton = new();
RegularSimplex simplex = new();
CoordinateDescent<RadixSearch> coordinateDescent = new();
HookJeeves hookJeeves = new();
RandomSearch random = new();

var algorithms = new List<MinimizatorTwoDims>()
{
    steepestDescent,
    conjugateGradients,
    newton,
    simplex,
    coordinateDescent,
    hookJeeves,
    random
};

writer.AddData(algorithms.Select(a => a.Report.Algorithm));

foreach (var epsilon in epsilons)
{
    task.Epsilon = epsilon;

    foreach (var parameter in parameters)
    {
        var parameterReport = new List<ReportTwoDims>();
        a = parameter;

        steepestDescent.TryGetMin(startPoint1, task);
        conjugateGradients.TryGetMin(startPoint1, task);
        newton.TryGetMin(startPoint1, task);
        simplex.TryGetMin(startPoint1, task, l: 2d, sigma: 0.6d);
        coordinateDescent.TryGetMin(startPoint1, task);
        hookJeeves.TryGetMin(startPoint1, task, delta: Vector<double>.Build.DenseOfArray(new double[] { 1d, 1d }), gamma: 1.5d, 0.0001d);
        random.TryGetMin(startPoint1, task, alpha: 30d, gamma: 1.2d, triesCount: 100);

        parameterReport.Add(steepestDescent.Report);
        parameterReport.Add(conjugateGradients.Report);
        parameterReport.Add(newton.Report);
        parameterReport.Add(simplex.Report);
        parameterReport.Add(coordinateDescent.Report);
        parameterReport.Add(hookJeeves.Report);
        parameterReport.Add(random.Report);
        reports.Add(parameterReport);

        algorithms.ForEach(a =>
        {
            plotter.PlotContour(task.Function, plotRange, plotRange);
            plotter.SetLine(a.Report.Path);
            plotter.Export(_savePath + "\\Graphic Task 1", a.Report.Algorithm + $" a = {parameter}, eplislon = {epsilon}");
        });
    }
}

reports.ForEach(r => writer.AddData(r.Select(x => x.FunctionCalculations)));
reports.ForEach(r => writer.AddData(r.Select(x => x.Min.X[0])));
reports.ForEach(r => writer.AddData(r.Select(x => x.Min.X[1])));

writer.Save();
