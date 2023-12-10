using Function;
using FunctionTwoDims;
using MathNet.Numerics.LinearAlgebra;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Range = Function.Range;

namespace Drawing
{
    public class FunctionPlotter : Plotter
    {
        private readonly double _accuracity;

        public FunctionPlotter(string labelX, string labelY, double accuracity)
        {
            _accuracity = accuracity;

            AddAxis(new LinearAxis()
            {
                Title = labelX,
                AxisTitleDistance = 10,
                Position = AxisPosition.Bottom,
                MinimumDataMargin = 10,
            });

            AddAxis(new LinearAxis()
            {
                Title = labelY,
                AxisTitleDistance = 15,
                Position = AxisPosition.Left,
            });
        }

        public void Plot(TargetFunction function, Range range)
        {
            ClearSeries();
            AddSeries(new FunctionSeries((x) => function(x).Y, range.Min, range.Max, _accuracity));
        }

        public void PlotContour(TargetFunctionTwoDims function, Range rangeX, Range rangeY, int step = 100)
        {
            ClearSeries();

            double peaks(double x, double y) => function(Vector<double>.Build.DenseOfArray(new double[] { x, y })).Y;
            var xx = ArrayBuilder.CreateVector(rangeX.Min, rangeX.Max, step);
            var yy = ArrayBuilder.CreateVector(rangeY.Min, rangeY.Max, step);
            var peaksData = ArrayBuilder.Evaluate(peaks, xx, yy);

            AddSeries(new ContourSeries
            {
                Color = OxyColors.Black,
                LabelBackground = OxyColors.White,
                ColumnCoordinates = yy,
                RowCoordinates = xx,
                Data = peaksData
            });
        }

        public void SetPoint(Point point)
        {
            var mark = new LineSeries();
            mark.Points.Add(new DataPoint(point.X, point.Y));
            mark.MarkerType = MarkerType.Circle;
            mark.LabelFormatString = $"({point.X:N3}; {point.Y:N3})";
            AddSeries(mark);
        }

        public void SetLine(IEnumerable<Vector<double>> values)
        {
            var line = new LineSeries();

            foreach (var x in values)
            {
                line.Points.Add(new DataPoint(x[0], x[1]));
                line.MarkerType = MarkerType.Circle;
            }

            AddSeries(line);
        }

        public void Clear()
        {
            ClearSeries();
        }
    }
}
