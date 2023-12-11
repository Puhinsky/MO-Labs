using FunctionTwoDims;
using MathNet.Numerics.LinearAlgebra;
using System.Linq;

namespace AlgoritmsTwoDims
{
    public class RegularSimplex : MinimizatorTwoDims
    {
        private PointTwoDims[]? _simplex;
        private double _l = 2d;
        private double _sigma = 0.5d;
        private int _n;

        public RegularSimplex()
        {
            Report.Algorithm = "Правильный симлекс";
        }

        public bool TryGetMin(Vector<double> x, MinimizationTaskTwoDims task, double l, double sigma)
        {
            _l = l;
            _sigma = sigma;

            return TryGetMin(x, task);
        }

        protected override void Init()
        {
            _n = StartX!.Count;
            InitSimplex();
            ReportSimplex();
        }

        protected override bool TerminationCondition()
        {
            return _l < Epsilon;
        }

        protected override void OnIteration()
        {
            var maxPoint = _simplex!.MaxBy(f => f.Y);

            if (TryReflectMax(maxPoint, out PointTwoDims reflectPoint))
            {
                ReplacePoint(maxPoint, reflectPoint);
                ReportSimplex();

                return;
            }

            if (TryReflectOther(maxPoint, out (PointTwoDims, PointTwoDims) otherReflectPoint))
            {
                ReplacePoint(otherReflectPoint.Item1, otherReflectPoint.Item2);
                ReportSimplex();

                return;
            }

            Reduce();
            ReportSimplex();
        }

        protected override void OnPostTermination()
        {
            MinPoint = _simplex!.MinBy(f => f.Y);
        }

        private void InitSimplex()
        {
            _simplex = new PointTwoDims[_n + 1];
            _simplex[0] = CalculateFunction(StartX!);

            for (int i = 1; i < _simplex.Length; i++)
            {
                _simplex[i].X = Vector<double>.Build.Dense(_n);

                for (int j = 0; j < _n; j++)
                {
                    if (i == j + 1)
                        _simplex[i].X[j] = StartX![j] + (Math.Sqrt(_n + 1) - 1) / (_n * Math.Sqrt(2)) * _l;
                    else
                        _simplex[i].X[j] = StartX![j] + (Math.Sqrt(_n + 1) + _n - 1) / (_n * Math.Sqrt(2)) * _l;
                }

                _simplex[i] = CalculateFunction(_simplex[i].X);
            }
        }

        private PointTwoDims Reflect(PointTwoDims pointToReflect)
        {
            var center = Vector<double>.Build.Dense(_n);

            foreach (var point in _simplex!)
            {
                if (point != pointToReflect)
                    center += point.X;
            }

            center *= 2 / _n;
            var reflection = center - pointToReflect.X;

            return CalculateFunction(reflection);
        }

        private bool TryReflectMax(PointTwoDims maxPoint, out PointTwoDims reflectPoint)
        {
            reflectPoint = Reflect(maxPoint);

            return maxPoint.Y > reflectPoint.Y;
        }

        private bool TryReflectOther(PointTwoDims maxPoint, out (PointTwoDims, PointTwoDims) result)
        {
            var reflects = new List<(PointTwoDims, PointTwoDims)>();
            bool minFinded = false;

            foreach (var point in _simplex!.Where(x => x != maxPoint))
            {
                var reflectPoint = Reflect(point);

                if (point.Y > reflectPoint.Y)
                {
                    reflects.Add((point, reflectPoint));
                    minFinded = true;
                }
            }

            if (minFinded)
                result = reflects.MinBy(f => f.Item2.Y);
            else
                result = (new PointTwoDims(), new PointTwoDims());

            return minFinded;
        }

        private void ReplacePoint(PointTwoDims point, PointTwoDims newPoint)
        {
            var list = _simplex!.ToList();
            var index = list.IndexOf(point);
            list.Remove(point);
            list.Insert(index, newPoint);
            _simplex = list.ToArray();
        }

        private void Reduce()
        {
            _l *= _sigma;

            for (int i = 1; i < _simplex!.Length; i++)
            {
                _simplex[i].X = _simplex[0].X + _sigma * (_simplex[i].X - _simplex[0].X);
                _simplex[i] = CalculateFunction(_simplex[i].X);
            }
        }

        private void ReportSimplex()
        {
            Report.Path.Add(_simplex!.Select(s => s.X).Append(_simplex![0].X).ToList());
            //Report.Path.AddRange(_simplex!.Select(s => s.X).Append(_simplex![0].X));
        }
    }
}
