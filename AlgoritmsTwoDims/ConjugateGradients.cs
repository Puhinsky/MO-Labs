using Algorithms;
using Function;
using FunctionTwoDims;
using Range = Function.Range;

namespace AlgoritmsTwoDims
{
    public class ConjugateGradients<SM> : MinimizatorTwoDims where SM : Minimizator, new()
    {
        private double[]? _x;
        private double[]? _gradient;
        private double[]? _prev_gradient;
        private double[]? _p;

        private double _alpha;
        private double _beta;

        private int _k;
        private readonly int _dimensions = 2;

        private readonly MinimizationTask _singleTask;
        private readonly SM _singleMinimizator = new();

        private bool NeedRestart => _k + 1 >= _dimensions;

        public ConjugateGradients()
        {
            Report.Algorithm = "Метод сопряженных градиентов";
            _singleTask = new MinimizationTask(
                new TargetFunction[]
                {
                    (x) => 
                    {
                        return new Point(x, CalculateFunction(_x!.Add(_p!.MultiplyConstant(x))).Y);
                    }
                },
                new Range()
                {
                    Min = 0,
                    Max = double.MaxValue
                });
        }

        protected override void Init()
        {
            _singleTask.Epsilon = Epsilon;

            _x = new double[DimensionsCount];
            _p = new double[DimensionsCount];
            _gradient = new double[DimensionsCount];
            _prev_gradient = new double[DimensionsCount];

            StartX!.CopyTo(_x, 0);
            _k = 0;
            _gradient = CalculateGradient(_x);
            _p = _gradient.MultiplyConstant(-1d);
        }

        protected override void OnIteration()
        {
            CalculateAlpha();
            _x = _x!.Add(_p!.MultiplyConstant(_alpha));

            _gradient!.CopyTo(_prev_gradient!, 0);
            _gradient = CalculateGradient(_x);

            if (NeedRestart)
            {
                _k = 0;
                _beta = 0;
            }
            else
            {
                CalculateBeta();
            }

            _p = _gradient.MultiplyConstant(-1d).Add(_p!.MultiplyConstant(_beta));
        }

        protected override bool TerminationCondition()
        {
            return _gradient!.CalculateLenght() < Epsilon;
        }

        protected override void OnPostTermination()
        {
            MinPoint = CalculateFunction(_x!);
        }

        private void CalculateAlpha()
        {
            _singleMinimizator.TryGetMin(_singleTask);
            _alpha = _singleMinimizator.Report.Min.X;
        }

        private void CalculateBeta()
        {
            _beta = Math.Pow(_gradient!.CalculateLenght(), 2) / Math.Pow(_prev_gradient!.CalculateLenght(), 2);
        }
    }
}
