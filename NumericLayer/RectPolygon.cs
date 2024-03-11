using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ImageDistorsion.NumericLayer
{
    using VecDbl = Vector<double>;

    internal class RectPolygon : ConvexPolygon
    {
        public RectPolygon(VecDbl lowerLeftVertex, VecDbl upperRightVertex)
            : base(PrepareForBase(lowerLeftVertex, upperRightVertex))
        {
            ValidateArgs(lowerLeftVertex, upperRightVertex);
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
