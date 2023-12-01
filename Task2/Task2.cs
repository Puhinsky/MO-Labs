using Algorithms;
using CSV;
using Drawing;
using Function;
using Range = Function.Range;

internal class Program
{
    private static MinimizationTask task = new(
        new TargetFunction[]
        {
            (x) => new Point(x, x * Math.Atan(x) - 0.5d * Math.Log(1 + Math.Pow(x, 2))),
            (x) => new Point(x, Math.Atan(x)),
            (x) => new Point(x, 1 / (1 + Math.Pow(x, 2))),
        },
        new Range()
        {
            Min = -10d,
            Max = 10d
        },
        0.01d
        );

    private static void Main()
    {
        Point minPoint = new()
        {
            X = 0,
            Y = 0
        };

        string _savePath = "C:\\Users\\puhinsky\\source\\repos\\MO Labs\\Resources\\Task2";

        CsvFileWriter writer = new(_savePath, "сходимость");

        /*FunctionPlotter plotter = new("x", "f(x)", 0.001d);
        plotter.Plot(task.Function[0], task.Range);
        plotter.Export(_savePath, "первообразная");
        plotter.Plot(task.Function[1], task.Range);
        plotter.Export(_savePath, "производная 1");
        plotter.Plot(task.Function[2], task.Range);
        plotter.Export(_savePath, "производная 2");*/

        Newton newton = new(12);
        NewtonRaphson newtonRaphson = new(12);
        Marquardt marquardt = new(12);

        List<double> newtonValues = new();
        List<Report> newtonReports = new();

        List<double> newtonRaphsonValues = new();
        List<Report> newtonRaphsonReports = new();

        List<double> marquardtValues = new();
        List<Report> marquardtReports = new();

        for (double x = task.Range.Min; x < task.Range.Max; x += task.Epsilon)
        {
            AddResult(newton, x, newtonValues, newtonReports);
            AddResult(newtonRaphson, x, newtonRaphsonValues, newtonRaphsonReports);
            AddResult(marquardt, x, marquardtValues, marquardtReports);
        }

        WriteResult(writer, newtonValues, newtonReports);
        WriteResult(writer, newtonRaphsonValues, newtonRaphsonReports);
        WriteResult(writer, marquardtValues, marquardtReports);

        writer.Save();
    }

    private static void AddResult(Newton algorithm, double x, List<double> values, List<Report> reports)
    {
        if (algorithm.TryGetMin(task, x))
        {
            values.Add(x);
            reports.Add(algorithm.Report);
        }
    }

    private static void WriteResult(CsvFileWriter writer, List<double> values, List<Report> reports)
    {
        writer.AddData(values);
        writer.AddData(reports.Select(r => r.Min.Y));
        writer.AddData(reports.Select(r => r.FunctionCalculations));
    }
}