using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ImageDistorsion.NumericLayer.NumericVisualization
{
    /// <summary>
    /// A marker for visualization
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="color"></param>
    public struct NuMarker<T>(double x, double y, T color)
    {
        /// <summary>
        /// The x coordinate of the marker
        /// </summary>
        public double x = x;

        /// <summary>
        /// The y coordinate of the marker
        /// </summary>
        public double y = y;

        /// <summary>
        /// The color of the marker
        /// </summary>
        public T color = color;

        /// <summary>
        /// Constructor of NuMarker
        /// </summary>
        /// <param name="vd"></param>
        /// <param name="color"></param>
        public NuMarker(Vector<double> vd, T color)
            : this(vd[0], vd[1], color) { }

        /// <summary>
        /// Convert the instance of NuMarker to a Vector&lt;double&gt;
        /// </summary>
        /// <returns></returns>
        public readonly Vector<double> GetVecDouble()
        {
            return Vector<double>.Build.DenseOfArray([x, y]);
        }
    }
}
