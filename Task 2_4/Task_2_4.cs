using Algorithms;
using AlgoritmsTwoDims;
using CSV;
using Drawing;
using FunctionTwoDims;
using MathNet.Numerics.LinearAlgebra;
using Newton = AlgoritmsTwoDims.Newton;

MinimizationTaskTwoDims task = new(
    (Vector<double> x) => new PointTwoDims(x, Math.Pow(Math.Pow(x[0], 2) + x[1] - 11, 2) + Math.Pow(x[0] + Math.Pow(x[1], 2) - 7, 2)),
    0.01d,
    new TargetFunctionTwoDims[][,]
    {
        new TargetFunctionTwoDims[,]
        {
            {
                (Vector<double> x) => new PointTwoDims(x, 4 * Math.Pow(x[0], 3) + 4 * x[0] * x[1] - 42 * x[0] + 2 * Math.Pow(x[1], 2) - 14),
                (Vector<double> x) => new PointTwoDims(x, 2 * Math.Pow(x[0], 2) + 4 * x[0] * x[1] + 4 * Math.Pow(x[1], 3) - 26 * x[1] - 22)
            }
        },
        new TargetFunctionTwoDims[,]
        {
            {
                (Vector<double> x) => new PointTwoDims(x, 12 * Math.Pow(x[0], 2) - 4 * x[1] - 42),
                (Vector<double> x) => new PointTwoDims(x, 4 * (x[0] - x[1]))
            },
            {
                (Vector<double> x) => new PointTwoDims(x, 4 * (x[0] - x[1])),
                (Vector<double> x) => new PointTwoDims(x, 4 * x[0] + 12 * Math.Pow(x[1], 2) - 26)
            }
        }
    });


var epsilons = new List<double> { 0.001d };

var startPoints =  new List<Vector<double>>()
{
    Vector<double>.Build.Dense(new[] { 0d, 0d }),
    Vector<double>.Build.Dense(new[] { -5d, 0d })
};

string _savePath = "C:\\Users\\puhinsky\\source\\repos\\MO Labs\\Resources\\Lab2\\";
CsvFileWriter writer = new(_savePath, "Задание 4");

FunctionPlotter plotter = new("x1", "x2", 0.001d);
Function.Range plotRange = new() { Min = -6, Max = 5 };

List<List<ReportTwoDims>> reports = new();

SteepestDescent<RadixSearch> steepestDescent = new();
ConjugateGradients<RadixSearch> conjugateGradients = new();
Newton newton = new();

var algorithms = new List<MinimizatorTwoDims>()
{
    steepestDescent,
    conjugateGradients,
    newton,
};

writer.AddData(algorithms.Select(a => a.Report.Algorithm));

foreach (var epsilon in epsilons)
{
    task.Epsilon = epsilon;

    foreach (var startPoint in startPoints)
    {
        var parameterReport = new List<ReportTwoDims>();

        steepestDescent.TryGetMin(startPoint, task);
        conjugateGradients.TryGetMin(startPoint, task);
        newton.TryGetMin(startPoint, task);

        parameterReport.Add(steepestDescent.Report);
        parameterReport.Add(conjugateGradients.Report);
        parameterReport.Add(newton.Report);

        reports.Add(parameterReport);

        algorithms.ForEach(a =>
        {
            plotter.PlotContour(task.Function, plotRange, plotRange, levelStep: 10);
            plotter.SetLines(a.Report.Path);
            plotter.Export(_savePath + "\\Graphic Task 4", a.Report.Algorithm + $" start point = {startPoint[0]} {startPoint[1]}, eplsilon = {epsilon}");
        });
    }
}

reports.ForEach(r => writer.AddData(r.Select(x => x.FunctionCalculations)));
reports.ForEach(r => writer.AddData(r.Select(x => x.Min.X[0])));
reports.ForEach(r => writer.AddData(r.Select(x => x.Min.X[1])));

writer.Save();
