using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageDistorsion;
using System.Drawing;

namespace ImageDistorsion.PixelLayer
{
    /// <summary>
    /// Create the pixel layer pullback correction, which is the ultimate goal of the
    /// pixel layer
    /// </summary>
    public class PullbackCorrection
    {   
        /// <summary>
        /// The mapping from row and column indexes to Euclidean plane coordinates
        /// </summary>
        public RowCol_Coord_Mapping RowCol2Coord { get; private set; }

        /// <summary>
        /// The numeric layer pullback correction
        /// </summary>
        public NumericLayer.PullbackCorrection NuPullback { get; private set; }

        /// <summary>
        /// The pixel layer pullback correction
        /// </summary>
        public Func<int, int, Color> CorrectedRowColToColorMapping =>
        (int rowIdx, int colIdx) =>
        {
            var c4h = RowCol2Coord.RowColToCoordMapping(rowIdx, colIdx);
            return NuPullback.CorrectedVisualization(c4h.x, c4h.y);
        };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rowCol2Coord"></param>
        /// <param name="nuPullback"></param>
        public PullbackCorrection(RowCol_Coord_Mapping rowCol2Coord, NumericLayer.PullbackCorrection nuPullback)
        {
            RowCol2Coord = rowCol2Coord;
            NuPullback = nuPullback;
        }
    }
}
