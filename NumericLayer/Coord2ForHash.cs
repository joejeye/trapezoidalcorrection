using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ImageDistorsion.NumericLayer
{
    public readonly struct Coord2ForHash<T>(T x, T y)
    {
        public readonly T x = x; 
        public readonly T y = y;

        public T[] ToArray()
        {
            return [x, y];
        }

        public Vector<double> ToVecDouble()
        {
            return Vector<double>.Build.DenseOfArray([(dynamic)x, (dynamic)y]);
        }

        /// <summary>
        /// Get the equality comparer function for hashing
        /// </summary>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static Coord2Comparer<T> GetEqCmpr(double tolerance = 1e-6)
        {
            return new(tolerance);
        }
    }

    public class Coord2Comparer<T>(double tolerance = 1e-6) : IEqualityComparer<Coord2ForHash<T>>
    {
        private double Tolerance { get; } = tolerance;

        private bool WithinTol(T x, T y)
        {
            return Math.Abs((double)((dynamic)x -  (dynamic)y)) <= Tolerance;
        }

        public bool Equals(Coord2ForHash<T> a, Coord2ForHash<T> b)
        {
            return WithinTol(a.x, b.x) && WithinTol(a.y, b.y);
        }

        public int GetHashCode(Coord2ForHash<T> a) => a.GetHashCode();
    }
}
