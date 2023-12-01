using Algorithms;
using CSV;
using Drawing;
using Function;
using Range = Function.Range;

MinimizationTask task = new(
        new TargetFunction[]
        {
            (x) => new Point(x, Math.Pow(x, 4) + Math.Pow(x, 2) + x + 1),
            (x) => new Point(x, 4 * Math.Pow(x, 3) + 2 * x + 1),
            (x) => new Point(x, 12 * Math.Pow(x, 2) + 2),
            (x) => new Point(x, 24 * x)
        },
        new Range()
        {
            Min = -1d,
            Max = 0d
        },
        0.01d
        );

Point minPoint = new()
{
    X = -0.385458499d,
    Y = 0.785195253d
};

List<Minimizator> algorithms = new() {
    new BruteForce(),
    new RadixSearch(),
    new Dichotomy(),
    new GoldenRatio(),
    new Parabola(),
    new MidPoint(),
    new Chords(),
    new Newton(12) };

List<double> epsilons = new()
        {
            0.01d,
            0.001d,
            0.0001d
        };

string _savePath = "C:\\Users\\puhinsky\\source\\repos\\MO Labs\\Resources\\Task1";
CsvFileWriter writer = new(_savePath, "Результат");

/*FunctionPlotter plotter = new("x", "f(x)", 0.001d);
plotter.Plot(task.Function[0], task.Range);
plotter.SetPoint(minPoint);
plotter.Export(_savePath, "function");*/

var reports = new List<List<Report>>();

epsilons.ForEach(e =>
{
    task.Epsilon = e;
    algorithms.ForEach(m => m.TryGetMin(task));
    reports.Add(algorithms.Select(m => m.Report).ToList());
});

writer.AddData(algorithms.Select(m => m.Report.Algorithm).ToList());

reports.ForEach(e =>
{
    writer.AddData(e.Select(i => i.FunctionCalculations));
});

reports.ForEach(e =>
{
    writer.AddData(e.Select(i => Math.Abs(i.Min.X - minPoint.X)));
});

writer.Save();
