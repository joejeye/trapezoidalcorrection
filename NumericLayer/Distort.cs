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

        /// <summary>
        /// The array of the vertex coordinates. Each vertex coordinate
        /// is a 2D vector of double-precision type. The order A->B->C->D
        /// follows the clock-wise direction
        /// </summary>
        public VecDbl[] ABCD_Prime;

        public DistortFunc DistortMapping { get; }

        public Distort(double w, double h, VecDbl[] abcd)
        {
            Width = w;
            Height = h;
            ABCD_Prime = abcd;
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
