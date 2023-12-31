﻿using Algorithms;
using Function;
using FunctionTwoDims;
using MathNet.Numerics.LinearAlgebra;
using Range = Function.Range;

namespace AlgoritmsTwoDims
{
    public class ConjugateGradients<SM> : MinimizatorTwoDims where SM : Minimizator, new()
    {
        private Vector<double>? _x;
        private Vector<double>? _gradient;
        private Vector<double>? _prev_gradient;
        private Vector<double>? _p;

        private double _alpha;
        private double _beta;

        private int _k;
        private readonly int _dimensions = 2;

        private readonly MinimizationTask _singleTask;
        private readonly SM _singleMinimizator = new();
        private double _singleEpsilon = 0.0001d;

        private bool NeedRestart => _k + 1 >= _dimensions;

        public ConjugateGradients()
        {
            Report.Algorithm = "Метод сопряженных градиентов";
            _singleTask = new MinimizationTask(
                new TargetFunction[]
                {
                    (x) =>
                    {
                        return new Point(x, CalculateFunction(_x + x * _p).Y);
                    }
                },
                new Range()
                {
                    Min = 0,
                    Max = double.MaxValue
                });
        }

        public bool TryGetMin(Vector<double> x, MinimizationTaskTwoDims task, double singleEpsilon)
        {
            _singleEpsilon = singleEpsilon;

            return TryGetMin(x, task);
        }

        protected override void Init()
        {
            _singleTask.Epsilon = _singleEpsilon;

            _x = Vector<double>.Build.DenseOfVector(StartX);
            _gradient = Vector<double>.Build.Dense(_x.Count);
            Report.Path.Add(new List<Vector<double>>());
            Report.Path.First().Add(_x);

            _k = 0;
            _gradient = CalculateGradient(_x);
            _p = -1 * _gradient;
        }

        protected override void OnIteration()
        {
            CalculateAlpha();
            _x += _alpha * _p;

            _prev_gradient = Vector<double>.Build.DenseOfVector(_gradient);

            _gradient = CalculateGradient(_x);

            if (NeedRestart)
            {
                _k = 0;
                _beta = 0;
            }
            else
            {
                CalculateBeta();
                _k++;
            }

            _p = -1d * _gradient + _beta * _p;
            Report.Path.First().Add(_x);
        }

        protected override bool TerminationCondition()
        {
            return _gradient!.L2Norm() < Epsilon;
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
            _beta = Math.Pow(_gradient!.L2Norm(), 2) / Math.Pow(_prev_gradient!.L2Norm(), 2);
        }
    }
}
