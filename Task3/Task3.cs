using Algorithms;
using CSV;
using Drawing;
using Function;
using System.Runtime.CompilerServices;
using Range = Function.Range;

internal class Program
{
    private static void Main()
    {
        MinimizationTask task1 = new(
new TargetFunction[]
{
            new TargetFunction((x) =>
            {
                return new Point()
                {
                    X = x,
                    Y = Math.Cos(x) / Math.Pow(x, 2)
                };
            }),
            new TargetFunction((x) =>
            {
                return new Point()
                {
                    X = x,
                    Y = -(x * Math.Sin(x) + 2 * Math.Cos(x)) / Math.Pow(x, 3)
                };
            })
},
new Range()
{
    Min = 1d,
    Max = 12d
},
0.01d
);

        MinimizationTask task2 = new(
        new TargetFunction[]
        {
            new TargetFunction((x) =>
            {
                return new Point()
                {
                    X = x,
                    Y = 0.1d * x + 2 * Math.Sin(4 * x)
                };
            }),
            new TargetFunction((x) =>
            {
                return new Point()
                {
                    X = x,
                    Y = 8 * Math.Cos(4 * x) + 0.1d
                };
            })
        },
        new Range()
        {
            Min = 0d,
            Max = 4d
        },
        0.01d
        );

        double min1 = 2.458714176d;
        double min2 = 1.17497d;

        string _savePath = "C:\\Users\\puhinsky\\source\\repos\\MO Labs\\Resources\\Task3";

        CsvFileWriter writer1 = new(_savePath, "глобальная минимизация 1");
        CsvFileWriter writer2 = new(_savePath, "глобальная минимизация 2");

        /*FunctionPlotter plotter = new("x", "f(x)", 0.001d);
        plotter.Plot(task1.Function[0], task1.Range);
        plotter.SetPoint(task1.Function[0].Point(min1));
        plotter.Export(_savePath, "функция 1");
        plotter.Plot(task2.Function[0], task2.Range);
        plotter.SetPoint(task2.Function[0].Point(min2));
        plotter.Export(_savePath, "функция 2");*/

        BruteForce bruteForce1 = new(task1);
        BruteForce bruteForce2 = new(task2);

        Polyline polyline1 = new(task1);
        Polyline polyline2 = new(task2);

        bruteForce1.TryGetMin();
        bruteForce2.TryGetMin();

        List<Report> bruteForceReports1 = new();
        List<Report> bruteForceReports2 = new();

        List<double> lipschitzs1 = new();
        List<double> lipschitzs2 = new();

        List<Report> polylineReports1 = new();
        List<Report> polylineReports2 = new();

        for (double lipschitz = 0.005; lipschitz <= 3; lipschitz += 0.005)
        {
            polyline1.TryGetMin(lipschitz);
            polyline2.TryGetMin(lipschitz);

            AddResult(lipschitz, polyline1, bruteForce1, lipschitzs1, polylineReports1, bruteForceReports1);
            AddResult(lipschitz, polyline2, bruteForce2, lipschitzs2, polylineReports2, bruteForceReports2);
        }

        WriteResult(writer1, lipschitzs1, polylineReports1, bruteForceReports1, min1);
        WriteResult(writer2, lipschitzs2, polylineReports2, bruteForceReports2, min2);

        writer1.Save();
        writer2.Save();
    }

    private static void AddResult(double lipschitz, Polyline polyline, BruteForce bruteForce, List<double> lipschitzs, List<Report> polylineReports, List<Report> bruteForceReports)
    {
        lipschitzs.Add(lipschitz);
        polylineReports.Add(polyline.Report);
        bruteForceReports.Add(bruteForce.Report);
    }

    private static void WriteResult(CsvFileWriter writer, List<double> lipschitzs, List<Report> polylineReports, List<Report> bruteForceReports, double min)
    {
        writer.AddData(bruteForceReports.Select(x => x.Min.X));
        writer.AddData(bruteForceReports.Select(x => Math.Abs(min - x.Min.X)));
        writer.AddData(bruteForceReports.Select(x => x.FunctionCalculations));
        writer.AddData(lipschitzs);
        writer.AddData(polylineReports.Select(x=>x.Min.X));
        writer.AddData(polylineReports.Select(x => Math.Abs(min - x.Min.X)));
        writer.AddData(polylineReports.Select(x => x.FunctionCalculations));
    }
}