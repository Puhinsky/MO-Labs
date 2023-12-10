using Algorithms;
using Function;
using FunctionTwoDims;
using MathNet.Numerics.LinearAlgebra;
using Range = Function.Range;

namespace AlgoritmsTwoDims
{
    public class CoordinateDescent<SM> : MinimizatorTwoDims where SM : Minimizator, new()
    {
        private Vector<double>[]? _basis;
        private Vector<double>? _x;
        private int _currentBasis;
        private PointTwoDims _prevPoint;

        private readonly SM _singleMinimizator = new();
        private readonly MinimizationTask _singleTask;
        private double _singleEpsilon = 0.0001d;

        public CoordinateDescent()
        {
            Report.Algorithm = "Покоординатный спуск";
            _singleTask = new(
                new TargetFunction[]
                {
                    (x) => new Point(x, CalculateFunction(_x + x * _basis![_currentBasis]).Y)
                },
                new Range()
                {
                    Min = 0,
                    Max = double.MaxValue
                }
            );
        }

        public bool TryGetMin(Vector<double> x, MinimizationTaskTwoDims task, double singleEpsilon)
        {
            _singleEpsilon = singleEpsilon;

            return TryGetMin(x, task);
        }

        protected override void Init()
        {
            _singleTask.Epsilon = _singleEpsilon;
            BuildBasis();
            _x = Vector<double>.Build.DenseOfVector(StartX!);
            Report.Path.Add(_x);
            MinPoint = CalculateFunction(_x);
            SolveForBasis();
        }

        protected override void OnIteration()
        {
            SolveForBasis();
            Report.Path.Add(_x!);
        }

        protected override bool TerminationCondition()
        {
            return _prevPoint.Y - MinPoint.Y < Epsilon;
        }

        private void BuildBasis()
        {
            _basis = new Vector<double>[StartX!.Count];

            for (int i = 0; i < _basis.Length; i++)
            {
                _basis[i] = Vector<double>.Build.Dense(_basis.Length);
                _basis[i][i] = 1d;
            }
        }

        private void SolveForBasis()
        {
            for (int i = 0; i < _basis!.Length; i++)
            {
                _currentBasis = i;
                _singleMinimizator.TryGetMin(_singleTask);
                var alpha = _singleMinimizator.Report.Min.X;
                _x += alpha * _basis[_currentBasis];
                _prevPoint = MinPoint;
                MinPoint = CalculateFunction(_x);
            }
        }
    }
}
