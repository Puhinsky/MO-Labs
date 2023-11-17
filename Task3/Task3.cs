using Algorithms;
using Drawing;
using Function;
using Range = Function.Range;

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

string _savePath = "C:\\Users\\puhinsky\\source\\repos\\MO Labs\\Resources\\Task3";

/*FunctionPlotter plotter = new("x", "f(x)", 0.001d);
plotter.Plot(task1.Function[0], task1.Range);
plotter.Export(_savePath, "функция 1");
plotter.Plot(task2.Function[0], task2.Range);
plotter.Export(_savePath, "функция 2");*/

BruteForce bruteForce1 = new(task1);
BruteForce bruteForce2 = new(task2);

Polilyne polilyne1 = new(task1);
Polilyne polilyne2 = new(task2);

bruteForce1.TryGetMin();
bruteForce2.TryGetMin();

polilyne1.TryGetMin();
polilyne2.TryGetMin(4);

Console.WriteLine(bruteForce1.Report.Min.X);
Console.WriteLine(bruteForce2.Report.Min.X);

Console.WriteLine(polilyne1.Report.Min.X);
Console.WriteLine(polilyne2.Report.Min.X);