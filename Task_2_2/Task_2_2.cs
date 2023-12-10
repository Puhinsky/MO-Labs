using Algorithms;
using AlgoritmsTwoDims;
using CSV;
using Drawing;
using FunctionTwoDims;
using MathNet.Numerics.LinearAlgebra;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Newton = AlgoritmsTwoDims.Newton;

MinimizationTaskTwoDims task1 = new(
    (Vector<double> x) => new PointTwoDims(x, 129 * Math.Pow(x[0], 2) - 256 * x[0] * x[1] + 129 * Math.Pow(x[1], 2) - 51 * x[0] - 149 * x[1] - 27),
    0.001d,
    new TargetFunctionTwoDims[][,]
    {
        new TargetFunctionTwoDims[,]
        {
            {
                (Vector<double> x) => new PointTwoDims(x, 258 * x[0] - 256 * x[1] - 51d),
                (Vector<double> x) => new PointTwoDims(x, -256 * x[0] + 258 * x[1] - 149d)
            }
        },
        new TargetFunctionTwoDims[,]
        {
            {
                (Vector<double> x) => new PointTwoDims(x, 258d),
                (Vector<double> x) => new PointTwoDims(x, -256d)
            },
            {
                (Vector<double> x) => new PointTwoDims(x, -256d),
                (Vector<double> x) => new PointTwoDims(x, 258)
            }
        }
    });

double a = 250;

MinimizationTaskTwoDims task2 = new(
    (Vector<double> x) => new PointTwoDims(x, Math.Pow(x[0], 2) + a * Math.Pow(x[1], 2)),
    0.001d,
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

string _savePath = "C:\\Users\\puhinsky\\source\\repos\\MO Labs\\Resources\\Lab2\\";
CsvFileWriter writer = new(_savePath, "Задание 2");

FunctionPlotter plotter = new("x1", "x2", 0.001d);
Function.Range plotRange = new() { Min = 0, Max = 60 };

var startPoint1 = Vector<double>.Build.Dense(new[] { 1d, 1d });

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
List<List<ReportTwoDims>> reports = new();
List<MinimizationTaskTwoDims> tasks = new() { task1, task2 };

int funtionNumber = 0;

foreach (var task in tasks)
{
    var taskReport = new List<ReportTwoDims>();

    steepestDescent.TryGetMin(startPoint1, task);
    conjugateGradients.TryGetMin(startPoint1, task);
    newton.TryGetMin(startPoint1, task);
    simplex.TryGetMin(startPoint1, task, l: 2d, sigma: 0.6d);
    coordinateDescent.TryGetMin(startPoint1, task);
    hookJeeves.TryGetMin(startPoint1, task, delta: Vector<double>.Build.DenseOfArray(new double[] { 1d, 1d }), gamma: 1.5d, 0.0001d);
    random.TryGetMin(startPoint1, task, alpha: 30d, gamma: 1.2d, triesCount: 100);

    taskReport.Add(steepestDescent.Report);
    taskReport.Add(conjugateGradients.Report);
    taskReport.Add(newton.Report);
    taskReport.Add(simplex.Report);
    taskReport.Add(coordinateDescent.Report);
    taskReport.Add(hookJeeves.Report);
    taskReport.Add(random.Report);

    reports.Add(taskReport);

    algorithms.ForEach(a =>
    {
        plotter.PlotContour(task.Function, plotRange, plotRange);
        plotter.SetLine(a.Report.Path);
        plotter.Export(_savePath + "\\Graphic Task 2", a.Report.Algorithm + $" f {funtionNumber}");
    });

    funtionNumber++;
}

reports.ForEach(r => writer.AddData(r.Select(x => x.FunctionCalculations)));
reports.ForEach(r =>
{
    writer.AddData(r.Select(x => x.Min.X[0]));
    writer.AddData(r.Select(x => x.Min.X[1]));
});

writer.Save();