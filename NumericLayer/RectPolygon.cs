using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ImageDistorsion.NumericLayer
{
    using VecDbl = Vector<double>;

    /// <summary>
    /// Rectangle
    /// </summary>
    public class RectPolygon : ConvexPolygon
    {
        /// <summary>
        /// The coordinates of the lower left vertex
        /// </summary>
        public double[] LL { get => [Xmin, Ymin]; }

        /// <summary>
        /// The coordinates of the lower right vertex
        /// </summary>
        public double[] LR { get => [Xmax, Ymin]; }

        /// <summary>
        /// The coordinates of the upper right vertex
        /// </summary>
        public double[] UR { get => [Xmax, Ymax]; }

        /// <summary>
        /// The coordinates of the upper left vertex
        /// </summary>
        public double[] UL { get => [Xmin, Ymax]; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lowerLeftVertex"></param>
        /// <param name="upperRightVertex"></param>
        public RectPolygon(VecDbl lowerLeftVertex, VecDbl upperRightVertex)
            : base(PrepareForBase(lowerLeftVertex, upperRightVertex))
        {
            ValidateArgs(lowerLeftVertex, upperRightVertex);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bottomLeftPoint"></param>
        /// <param name="topRightPoint"></param>
        /// <exception cref="ArgumentException"></exception>
        public RectPolygon(double[] bottomLeftPoint, double[] topRightPoint)
            : this(VecDbl.Build.DenseOfArray(bottomLeftPoint), VecDbl.Build.DenseOfArray(topRightPoint))
        {
            if (bottomLeftPoint.Length != 2 || topRightPoint.Length != 2)
            {
                throw new ArgumentException("The length of the input array should be 2");
            }
        }

        private static VecDbl[] PrepareForBase(VecDbl lowerLeftVertex, VecDbl upperRightVertex)
        {
            double xmin = lowerLeftVertex[0];
            double xmax = upperRightVertex[0];
            double ymin = lowerLeftVertex[1];
            double ymax = upperRightVertex[1];
            Func<double, double, VecDbl> MakeVec = (double x, double y) => VecDbl.Build.DenseOfArray([x, y]);
            var A = MakeVec(xmin, ymin);
            var B = MakeVec(xmax, ymin);
            var C = MakeVec(xmax, ymax);
            var D = MakeVec(xmin, ymax);
            return [A, B, C, D];
        }

        private static void ValidateArgs(VecDbl lowerLeftVertex, VecDbl upperRightVertex)
        {
            bool f1 = lowerLeftVertex[0] <= upperRightVertex[0];
            bool f2 = lowerLeftVertex[1] <= upperRightVertex[1];
            if (!(f1 && f2))
            {
                throw new ArgumentException("Wrong relative positions for the lower left vertex and the upper right vertex");
            }
        }
    }

}
