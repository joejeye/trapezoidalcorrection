using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ImageDistorsion.NumericLayer.NumericVisualization
{
    public struct NuMarker<T>(double x, double y, T color)
    {
        public double x = x;
        public double y = y;
        public T color = color;

        public NuMarker(Vector<double> vd, T color)
            : this(vd[0], vd[1], color) { }

        public readonly Vector<double> GetVecDouble()
        {
            return Vector<double>.Build.DenseOfArray([x, y]);
        }
    }
}
