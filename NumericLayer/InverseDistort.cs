using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ImageDistorsion.NumericLayer
{
    using VecDbl = Vector<double>;

    internal class InverseDistort
    {
        private ConvexPolygon _convexPolygon;

        public ConvexPolygon CvxPolygon
        {
            get { return _convexPolygon; }
            private set { _convexPolygon = value; }
        }

        public VecDbl[] ABCD
        {
            get
            {
                return CvxPolygon.Vertices;
            }
        }

        private double _XSpan;
        public double XSpan
        {
            get => _XSpan;
            private set { _XSpan = value; }
        }

        private double _YSpan;
        public double YSpan
        {
            get => _YSpan;
            private set { _YSpan = value; }
        }

        public InverseDistort(ConvexPolygon convexPolygon, double xspan, double yspan)
        {
            ArgumentNullException.ThrowIfNull(convexPolygon);
            if (convexPolygon.NSides != 4)
            {
                throw new ArgumentException("The polygon must be a quadrilateral");
            }
            CvxPolygon = convexPolygon;
            XSpan = xspan;
            YSpan = yspan;
            GetMapping();
        }

        public delegate VecDbl InvDistrtFunc(VecDbl vd);
        private InvDistrtFunc _Mapping;
        public InvDistrtFunc Mapping
        {
            get => _Mapping;
            private set { _Mapping = value; }
        }

        private double epsilon = 1e-6;

        public double Epsilon
        { 
            get => epsilon;
            private set { epsilon = value; }
        }

        public void SetTolerance(double epsilon)
        {
            if (!(0 < epsilon && epsilon <= 0.01))
            {
                throw new ArgumentException("The tolerance must be in the range (0, 0.01]");
            }
            Epsilon = epsilon;
        }

        /// <summary>
        /// Generate the inverse distortion mapping
        /// </summary>
        private void GetMapping()
        {
            var A = ABCD[0];
            var B = ABCD[1];
            var C = ABCD[2];
            var D = ABCD[3];
            VecDbl F(VecDbl point)
            {
                // Find x
                double x = 0;
                Func<double, VecDbl> M = (double s) => s * B + (1 - s) * A;
                Func<double, VecDbl> N = (double s) => s * C + (1 - s) * D;
                double left = 0;
                double right = 1;                
                while (Math.Abs(left - right) > Epsilon)
                {
                    double mid = (left + right) / 2;
                    ConvexPolygon leftHalf = new([ A, M(mid), N(mid), D ]);
                    if (leftHalf.ContainsPoint(point, out bool isOn))
                    {
                        if (isOn && IsAlign(M(mid), point, N(mid)))
                        {
                            x = mid;
                            break;
                        } else
                        {
                            right = mid;
                        }
                    } else
                    {
                        left = mid;
                    }
                }

                // Find y
                double y = 0;
                Func<double, VecDbl> V = (double t) => t * D + (1 - t) * A;
                Func<double, VecDbl> W = (double t) => t * C + (1 - t) * B;
                left = 0;
                right = 1;
                while (Math.Abs(left - right) > Epsilon)
                {
                    double mid = (left + right) / 2;
                    ConvexPolygon lowerHalf = new([A, B, W(mid), V(mid)]);
                    if (lowerHalf.ContainsPoint(point, out bool isOn))
                    {
                        if (isOn && IsAlign(V(mid), point, W(mid)))
                        {
                            y = mid;
                            break;
                        } else
                        {
                            right = mid;
                        }
                    } else
                    {
                        left = mid;
                    }
                }

                return VecDbl.Build.DenseOfArray([x, y]);
            }
            Mapping = F;
        }

        /// <summary>
        /// Check if a collection of points are on the same line
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private bool IsAlign(params VecDbl[] points)
        {
            if (points.Length <= 2)
            {
                return true;
            }
            
            var refVec = points[1] - points[0];
            for (int i = 2; i < points.Length; i++)
            {
                var currVec = points[i] - points[0];
                if (!IsParallel(refVec, currVec))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check if two vectors are parallel
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        private bool IsParallel(VecDbl v1, VecDbl v2)
        {
            return Math.Abs(v1[0] * v2[1] - v1[1] * v2[0]) <= Epsilon;
        }
    }
}
