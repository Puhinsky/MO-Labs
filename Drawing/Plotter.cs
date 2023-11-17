using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Drawing
{
    public abstract class Plotter
    {
        private readonly PlotModel _model;

        public Plotter()
        {
            _model = new PlotModel();
        }

        protected void AddAxis(Axis axis)
        {
            _model.Axes.Add(axis);
        }

        protected void AddSeries(Series series)
        {
            _model.Series.Add(series);
        }

        protected void ClearSeries()
        {
            _model.Series.Clear();
        }

        protected void AddAnnotation(Annotation annotation)
        {
            _model.Annotations.Add(annotation);
        }

        public void Export(string path, string svgName, float width = 1280, float height = 720)
        {
            using var writer = new FileStream($"{path}\\{svgName}.svg", FileMode.Create);
            SvgExporter.Export(_model, writer, width, height, true);
        }
    }
}
