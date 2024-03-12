using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ImageDistorsion.NumericLayer
{
    internal readonly struct Coord2ForHash<T>
    {
        public readonly T x; 
        public readonly T y;

        public Coord2ForHash(T x, T y)
        {
            this.x = x;
            this.y = y;
        }

        public T[] ToArray()
        {
            return [x, y];
        }

        public Vector<double> ToVecDouble()
        {
            return Vector<double>.Build.DenseOfArray([(dynamic)x, (dynamic)y]);
        }
    }

    internal class Coord2Comparer<T> : IEqualityComparer<Coord2ForHash<T>>
    {
        private double tolerance { get; }

        public Coord2Comparer(double tolerance = 1e-6)
        {
            this.tolerance = tolerance;
        }

        private bool WithinTol(T x, T y)
        {
            return Math.Abs((double)((dynamic)x -  (dynamic)y)) <= tolerance;
        }

        public bool Equals(Coord2ForHash<T> a, Coord2ForHash<T> b)
        {
            return WithinTol(a.x, b.x) && WithinTol(a.y, b.y);
        }

        public int GetHashCode(Coord2ForHash<T> a) => a.GetHashCode();
    }
}
