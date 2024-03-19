using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ImageDistorsion.NumericLayer
{
    /// <summary>
    /// Wrapper for 2D coordinates to be used as keys in a dictionary
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public readonly struct Coord2ForHash<T>(T x, T y)
    {
        /// <summary>
        /// The x coordinate
        /// </summary>
        public readonly T x = x; 
        
        /// <summary>
        /// The y coordinate
        /// </summary>
        public readonly T y = y;

        /// <summary>
        /// Create a 2-element array from the coordinates
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            return [x, y];
        }

        /// <summary>
        /// Create a 2D vector from the coordinates
        /// </summary>
        /// <returns></returns>
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

    /// <summary>
    /// The equality comparer for hashing
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tolerance"></param>
    public class Coord2Comparer<T>(double tolerance = 1e-6) : IEqualityComparer<Coord2ForHash<T>>
    {
        private double Tolerance { get; } = tolerance;

        /// <summary>
        /// Check if the difference between two numbers is within the tolerance
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool WithinTol(T x, T y)
        {
            ArgumentNullException.ThrowIfNull(x);
            ArgumentNullException.ThrowIfNull(y);   
            return Math.Abs((double)((dynamic)x -  (dynamic)y)) <= Tolerance;
        }

        /// <summary>
        /// Implement the equality comparer
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals(Coord2ForHash<T> a, Coord2ForHash<T> b)
        {
            return WithinTol(a.x, b.x) && WithinTol(a.y, b.y);
        }

        /// <summary>
        /// Implement the hash code function
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public int GetHashCode(Coord2ForHash<T> a) => a.GetHashCode();
    }
}
