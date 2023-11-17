using Algorithms;
using CSV;
using Drawing;
using Function;
using Range = Function.Range;

MinimizationTask task = new(
        new TargetFunction[]
        {
            new TargetFunction((double x) =>
            {
                return new Point()
                {
                    X = x,
                    Y = Math.Pow(x, 4) + Math.Pow(x, 2) + x + 1
                };
            }),
            new TargetFunction((double x) =>
            {
                return new Point()
                {
                    X = x,
                    Y = 4 * Math.Pow(x, 3) + 2 * x + 1
                };
            }),
            new TargetFunction((double x) =>
            {
                return new Point()
                {
                    X = x,
                    Y = 12 * Math.Pow(x, 2) + 2
                };
            }),
            new TargetFunction((double x) =>
            {
                return new Point()
                {
                    X = x,
                    Y = 24 * x
                };
            })
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
    new BruteForce(task),
    new RadixSearch(task),
    new Dichotomy(task),
    new GoldenRatio(task),
    new Parabola(task),
    new MidPoint(task),
    new Chords(task),
    new Newton(task, 12) };

List<double> epsilons = new()
        {
            0.01d,
            0.001d,
            0.0001d
        };

string _savePath = "C:\\Users\\puhinsky\\source\\repos\\MO Labs\\Resources\\Task1";
CsvFileWriter writer = new(_savePath, "Результат");

FunctionPlotter plotter = new("x", "f(x)", 0.001d);
plotter.Plot(task.Function[0], task.Range);
plotter.SetPoint(minPoint);
plotter.Export(_savePath, "function");

var reports = new List<List<Report>>();

epsilons.ForEach(e =>
{
    task.Epsilon = e;
    algorithms.ForEach(m => m.TryGetMin());
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
