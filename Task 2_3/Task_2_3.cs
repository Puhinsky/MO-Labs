using Algorithms;
using AlgoritmsTwoDims;
using CSV;
using Drawing;
using FunctionTwoDims;
using MathNet.Numerics.LinearAlgebra;
using Newton = AlgoritmsTwoDims.Newton;

MinimizationTaskTwoDims task = new(
    (Vector<double> x) => new PointTwoDims(x, 100 * Math.Pow(Math.Pow(x[0], 2) - x[1], 2) + Math.Pow(x[0] - 1, 2)),
    0.01d,
    new TargetFunctionTwoDims[][,]
    {
        new TargetFunctionTwoDims[,]
        {
            {
                (Vector<double> x) => new PointTwoDims(x, 400 * Math.Pow(x[0], 3) - 400 * x[0] * x[1] + 2 * x[0] - 2),
                (Vector<double> x) => new PointTwoDims(x, -200 * (Math.Pow(x[0], 2) - x[1]))
            }
        },
        new TargetFunctionTwoDims[,]
        {
            {
                (Vector<double> x) => new PointTwoDims(x, 1200 * Math.Pow(x[0], 2) - 400 * x[1] + 2),
                (Vector<double> x) => new PointTwoDims(x, -400 * x[0])
            },
            {
                (Vector<double> x) => new PointTwoDims(x,  -400 * x[0]),
                (Vector<double> x) => new PointTwoDims(x, 200)
            }
        }
    });


var epsilons = new List<double> { 0.001d, 0.00001d };
var singleEpsilons = new List<double> { 0.0001d, 0.000001d, 0.00000001d };

var startPoint = Vector<double>.Build.Dense(new[] { -1d, 1d });

string _savePath = "C:\\Users\\puhinsky\\source\\repos\\MO Labs\\Resources\\Lab2\\";
CsvFileWriter writer = new(_savePath, "Задание 3");

FunctionPlotter plotter = new("x1", "x2", 0.001d);
Function.Range plotRange = new() { Min = -2, Max = 2 };

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

    foreach (var singleEpsilon in singleEpsilons)
    {
        var parameterReport = new List<ReportTwoDims>();

        steepestDescent.TryGetMin(startPoint, task, singleEpsilon);
        conjugateGradients.TryGetMin(startPoint, task, singleEpsilon);
        newton.TryGetMin(startPoint, task);
        simplex.TryGetMin(startPoint, task, l: 2d, sigma: 0.6d);
        coordinateDescent.TryGetMin(startPoint, task, singleEpsilon);
        hookJeeves.TryGetMin(startPoint, task, delta: Vector<double>.Build.DenseOfArray(new double[] { 1d, 1d }), gamma: 1.5d, singleEpsilon);
        random.TryGetMin(startPoint, task, alpha: 30d, gamma: 1.2d, triesCount: 100);

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
            plotter.SetLines(a.Report.Path);
            plotter.Export(_savePath + "\\Graphic Task 3", a.Report.Algorithm + $" single epsilon = {singleEpsilon}, eplislon = {epsilon}");
        });
    }
}

reports.ForEach(r => writer.AddData(r.Select(x => x.FunctionCalculations)));
reports.ForEach(r => writer.AddData(r.Select(x => x.Min.X[0])));
reports.ForEach(r => writer.AddData(r.Select(x => x.Min.X[1])));

writer.Save();
