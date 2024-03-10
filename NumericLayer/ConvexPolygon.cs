using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ImageDistorsion.NumericLayer
{
    using VecDbl = Vector<double>;

    internal class ConvexPolygon
    {
        /// <summary>
        /// The array of the vertices of the polygon
        /// </summary>
        public VecDbl[] Vertices { get; }

        /// <summary>
        /// The number of sides of the polygon
        /// </summary>
        public int NSides
        {
            get
            {
                return Vertices.Length;
            }
        }

        public ConvexPolygon(VecDbl[] vtcs)
        {
            Vertices = vtcs;
            CheckConvexity();
        }

        public ConvexPolygon(double[][] vertArr)
        {
            Vertices = (from vert in vertArr
                        select VecDbl.Build.DenseOfArray(vert)).ToArray<VecDbl>();
            CheckConvexity();
        }

        /// <summary>
        /// Check if a point is contained in the convex polygon, and if a point is on
        /// the polygon boundary.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="onBoundary"></param>
        /// <returns>Returns true if the point in question is contained in the polygon (including
        /// on the boundary). The `onBoundary` parameter is returned true if the point is on 
        /// the polygon bounday. False for otherwise cases.</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool ContainsPoint(VecDbl point, out bool onBoundary)
        {
            if (point.Count != 2)
            {
                throw new ArgumentException("The dimension of the input point must be 2");
            }

            if (Vertices.Length == 1)
            {
                onBoundary = true;
                return (Vertices[0][0] == point[0] && Vertices[0][1] == point[1]);
            }

            VecDbl nextVertex;
            int sideStatus = 0;
            for (int i = 0; i < Vertices.Length; i++)
            {
                if (i ==  Vertices.Length - 1)
                {
                    nextVertex = Vertices[0];
                } else
                {
                    nextVertex = Vertices[i + 1];
                }
                var edgeVec = nextVertex - Vertices[i];
                var pivotVec = point - Vertices[i];
                int currStatus = Math.Sign(XProdDirVol(edgeVec, pivotVec));
                if (currStatus == 0)
                {
                    onBoundary = true;
                    return true;
                }
                if (sideStatus == 0)
                {
                    sideStatus = currStatus;
                } else if (sideStatus != currStatus)
                {
                    onBoundary = false;
                    return false;
                }
            }
            onBoundary = false;
            return true;
        }

        /// <summary>
        /// Check the convexity of the polygon defined by the ordered vertices.
        /// If the polygon is not convex, throw an exception.
        /// The polygon is convex if and only if an ant always turn right or left
        /// as it traverses the boundary.
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void CheckConvexity()
        {
            if (Vertices.Length <= 3)
                return;

            int turningStatus = 0;
            var prevVec = Vertices[0] - Vertices[^1];
            for (int i = 0; i < Vertices.Length; i++)
            {
                VecDbl currVec;
                if (i == Vertices.Length - 1)
                {
                    currVec = Vertices[0] - Vertices[i];
                } else
                {
                    currVec = Vertices[i + 1] - Vertices[i];
                }
                int currStat = Math.Sign(XProdDirVol(prevVec, currVec));
                if (turningStatus == 0)
                {
                    turningStatus = currStat;
                } else if (turningStatus != currStat)
                {
                    throw new Exception("The polygon is not convex");
                }
            }

        }

        /// <summary>
        /// Compute the directed volume (area in 2D space) of the X-product
        /// The X-product is defined as
        /// v1 X v2 = ||v1||*||v2||sin(theta)
        /// where the theta is the angle required to rotate counter-clockwise from v1 to v2
        /// </summary>
        /// <param name="v1">The start vector</param>
        /// <param name="v2">The end vector</param>
        /// <returns>The directed volume</returns>
        private static double XProdDirVol(VecDbl v1, VecDbl v2)
        {
            var M = Matrix<double>.Build.DenseOfColumnVectors([v1, v2]);
            return M.Determinant();
        }
    }
}
