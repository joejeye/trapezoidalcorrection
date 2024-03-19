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

    /// <summary>
    /// A class to represent a convex polygon in 2D space. The polygon is defined by an array of vertices.
    /// </summary>
    public class ConvexPolygon
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

        /// <summary>
        /// The maximum x-coordinate of the vertices
        /// </summary>
        public double Xmax { get => (from vd in Vertices select vd[0]).Max(); }

        /// <summary>
        /// The minimum x-coordinate of the vertices
        /// </summary>
        public double Xmin { get => (from vd in Vertices select vd[0]).Min(); }
        
        /// <summary>
        /// The maximum y-coordinate of the vertices
        /// </summary>
        public double Ymax { get => (from vd in Vertices select vd[1]).Max(); }
        
        /// <summary>
        /// The minimum y-coordinate of the vertices
        /// </summary>
        public double Ymin { get => (from vd in Vertices select vd[1]).Min(); }
        
        /// <summary>
        /// The diameter of the polygon in the x-direction
        /// </summary>
        public double MaxXDist { get => Xmax - Xmin; }
        
        /// <summary>
        /// The diameter of the polygon in the y-direction
        /// </summary>
        public double MaxYDist { get => Ymax - Ymin; }

        /// <summary>
        /// Construct a convex polygon from an array of vertices. Each vertex is a 2D vector.
        /// </summary>
        /// <param name="vtcs"></param>
        public ConvexPolygon(VecDbl[] vtcs)
        {
            Vertices = vtcs;
            CheckConvexity();
        }

        /// <summary>
        /// Construct a convex polygon from an array of vertices. Each vertex is a 2-element array of double.
        /// </summary>
        /// <param name="vertArr"></param>
        public ConvexPolygon(double[][] vertArr)
        {
            Vertices = (from vert in vertArr
                        select VecDbl.Build.DenseOfArray(vert)).ToArray();
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

                prevVec = currVec;
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

        /// <summary>
        /// Create the convex hull of an array of points on the 2D plane
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static ConvexPolygon ConvexHullOf(double[][] points)
        {
            // Check validity
            foreach(var p in points)
            {
                if (p.Length != 2)
                    throw new ArgumentException("Every point must has the length of 2");
            }

            // Check for 1-, 2-, 3-simplexes
            if (points.Length <= 3)
            {
                return new ConvexPolygon(points);
            }

            // Get the left-most point
            double[] lmP = LeftMostPoint(points);

            // Record the points that has been marked as convex hull vertices
            HashSet<Coord2ForHash<double>> vertedPnts = new(new Coord2Comparer<double>())
            {
                new Coord2ForHash<double>(lmP[0], lmP[1])
            };

            // Use the gift wrapping algorithm to find the convex hull
            double[] currP = lmP;
            while (true)
            {
                double[] nextVert = NextCvxHlVert(points, currP);
                if (vertedPnts.Contains(new Coord2ForHash<double>(nextVert[0], nextVert[1])))
                {
                    break;
                } else
                {
                    vertedPnts.Add(new Coord2ForHash<double>(nextVert[0], nextVert[1]));
                    currP = nextVert;
                }
            }

            VecDbl[] cvxVerts = ( from p in vertedPnts select p.ToVecDouble() ).ToArray();
            return new ConvexPolygon(cvxVerts);
        }

        private static double[] LeftMostPoint(double[][] points)
        {
            return (from p in points
                    orderby p[0] ascending
                    select p).First();
        }

        private static double[] NextCvxHlVert(double[][] points, double[] currP)
        {
            VecDbl currPnt = VecDbl.Build.DenseOfArray(currP);
            VecDbl? pivotPnt = null;
            // Find the first point that does not coincide with currP
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i][0] != currP[0] || points[i][1] != currPnt[1])
                {
                    pivotPnt = VecDbl.Build.DenseOfArray(points[i]);
                    break;
                }
            }

            // The edge as a candidate of the boundary of the convex hull
            VecDbl edge = pivotPnt - currPnt;

            // Search for the next vertex point
            for (int i = 0; i < points.Length; i++)
            {
                var itP = points[i];
                if (itP[0] == currP[0] && itP[1] == currP[1])
                {
                    continue;
                }
                VecDbl pivotVec = VecDbl.Build.DenseOfArray(itP) - currPnt;
                if (XProdDirVol(pivotVec, edge) < 0) // Point i is on the left of the edge
                {
                    pivotPnt = VecDbl.Build.DenseOfArray(itP);
                    edge = pivotPnt - currPnt;
                }
            }
            ArgumentNullException.ThrowIfNull(pivotPnt);
            return [pivotPnt[0], pivotPnt[1]];
        }
    }
}
