using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;

namespace ImageDistorsion.NumericLayer
{
    using VecDbl = Vector<double>;

    /// <summary>
    /// The concept of pullback comes from the differential geometry. Given a mapping between two
    /// manifolds f: M -> N and a function (field valued or vector-valued) g: N -> R^n, the pullback
    /// is a function g^*: M -> R^n such that g^*(p) = g(f(p)) for all p in M. In the numeric layer
    /// of this project, the f is the distorsion mapping (i.e., the perspective projection), g assigns
    /// each point in the quadrilateral a color, and g^* assigns each point in the corrected domain a 
    /// corresponding color.
    /// Usually, f is invertible, but we do not perfrom the perspective correction using the inverse f.
    /// Because there would be missing values in the corrected domain as the pixels are dicrete. Instead,
    /// the pullback ensures that each point in the corrected domain is assigned a color.
    /// </summary>
    public class PullbackCorrection
    {
        /// <summary>
        /// The distorted quadrilateral possibly resulted from some perspective projection
        /// </summary>
        public ConvexPolygon Quadrilateral { get; private set; }

        /// <summary>
        /// The estimated original rectangle that is supposed to be the real shape of the object
        /// before the perspective projection
        /// </summary>
        public RectPolygon CorrectedDomain { get; private set; }

        /// <summary>
        /// The mapping from the subset of the Euclidean plane to the color space
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public delegate Color VisualizationMapping(double x, double y);

        /// <summary>
        /// The mapping that tells the color of the point in the quadrilateral
        /// </summary>
        public VisualizationMapping VisMap { get; private set; }


        /// <summary>
        /// The mapping that assigns each point in the corrected domain the color that
        /// the image of the distorsion mapping of the point in the quadrilateral.
        /// </summary>
        public VisualizationMapping CorrectedVisualization =>
            (double x, double y) =>
            {
                Coord2ForHash<double> point = new(x, y);
                Coord2ForHash<double> mappedPoint = DistorsionMapping(point);
                return VisMap(mappedPoint.x, mappedPoint.y);
            };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="quadrilateral"></param>
        /// <param name="correctedDomain"></param>
        /// <param name="visMap">The mapping that assigns each point in the numeric layer distorted quadrilateral a color</param>
        public PullbackCorrection(ConvexPolygon quadrilateral, RectPolygon correctedDomain, VisualizationMapping visMap)
        {
            Quadrilateral = quadrilateral;
            CorrectedDomain = correctedDomain;
            VisMap = visMap;
        }

        /// <summary>
        /// The mapping from the corrected domain (the rectangle) to the original domain (the quadrilaterl)
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private Coord2ForHash<double> DistorsionMapping(Coord2ForHash<double> point)
        {
            VecDbl A = Quadrilateral.Vertices[0];
            VecDbl B = Quadrilateral.Vertices[1];
            VecDbl C = Quadrilateral.Vertices[2];
            VecDbl D = Quadrilateral.Vertices[3];
            double t = (point.x - CorrectedDomain.Xmin) / CorrectedDomain.MaxXDist;
            double s = (point.y - CorrectedDomain.Ymin) / CorrectedDomain.MaxYDist;
            VecDbl M = (1 - t) * A + t * B;
            VecDbl N = (1 - t) * D + t * C;
            VecDbl P = (1 - s) * M + s * N;
            return new(P[0], P[1]);
        }
    }
}
