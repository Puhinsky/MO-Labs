using Function;
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
                MinimumDataMargin = 10
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

        public void SetPoint(Point point)
        {
            var mark = new LineSeries();
            mark.Points.Add(new DataPoint(point.X, point.Y));
            mark.MarkerType = MarkerType.Circle;
            mark.LabelFormatString = $"({point.X:N3}; {point.Y:N3})";
            AddSeries(mark);
        }
    }
}
