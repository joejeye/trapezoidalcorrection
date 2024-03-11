using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ImageDistorsion.NumericLayer
{
    using VecDbl = Vector<double>;

    internal class Distort
    {
        public delegate VecDbl DistortFunc(VecDbl vd);

        // The width of the original rectangular image
        public double Width;

        // The height of the original rectangular image
        public double Height;

        private ConvexPolygon cvxPol;

        public ConvexPolygon CvxPolygon
        {
            get
            {
                return cvxPol;
            }

            private set
            {
                cvxPol = value;
            }
        }

        /// <summary>
        /// The array of the vertex coordinates. Each vertex coordinate
        /// is a 2D vector of double-precision type. The order A->B->C->D
        /// follows the clock-wise direction
        /// </summary>
        public VecDbl[] ABCD_Prime
        {
            get
            {
                return CvxPolygon.Vertices;
            }
        }

        private DistortFunc disfun;

        public DistortFunc DistortMapping
        {
            get
            {
                return disfun;
            }

            private set
            {
                disfun = value;
            }
        }

        public Distort(double w, double h, VecDbl[] abcd)
        {
            CommonConstructor(w, h, new ConvexPolygon(abcd));
            if (cvxPol == null || disfun == null)
            {
                throw new NullReferenceException("");
            }
        }

        public Distort(double w, double h, ConvexPolygon cp)
        {
            CommonConstructor(w, h, cp);
            if (cvxPol == null || disfun == null)
            {
                throw new NullReferenceException("");
            }
        }

        private void CommonConstructor(double w, double h, ConvexPolygon cp)
        {
            Width = w;
            Height = h;
            ArgumentNullException.ThrowIfNull(cp);
            if (cp.NSides != 4)
            {
                throw new ArgumentException("The polygon must be a quarilateral");
            }
            CvxPolygon = cp;
            DistortMapping = GetMapping();
        }

        private DistortFunc GetMapping()
        {
            VecDbl A = ABCD_Prime[0];
            VecDbl B = ABCD_Prime[1];
            VecDbl C = ABCD_Prime[2];
            VecDbl D = ABCD_Prime[3];
            VecDbl F(VecDbl vd)
            {
                double s = vd[0] / Height;
                double t = vd[1] / Width;
                VecDbl ans = (1 - t) * ((1 - s) * A + s * B) +
                    t * ((1 - s) * D + s * C);
                return ans;
            }
            return F;
        }
    }
}
