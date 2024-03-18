using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ImageDistorsion.PixelLayer;

namespace ImageDistorsion.NumericLayer
{
    /// <summary>
    /// Given a mapping f from the Euclidean plane to the set of lattice points, and a mapping g defined
    /// on the set of lattice points mapping each lattice point to a color, this class is used to
    /// pull the g back to the Euclidean plane. That is to say, the class derives a mapping h from the
    /// Euclidean plane to the set of colors.
    /// </summary>
    public class DistortedVisualizationMapping
    {
        /// <summary>
        /// The delegate tyoe for mapping the row and column indexes to a color
        /// </summary>
        /// <param name="rowIdx"></param>
        /// <param name="colIdx"></param>
        /// <returns></returns>
        public delegate Color RowColToColorMapping(int rowIdx, int colIdx);

        /// <summary>
        /// The delegate for mapping the coordinates to the row and column indexes
        /// </summary>
        public RowColToColorMapping RowCol2Color { get; private set; }

        /// <summary>
        /// The instance of the class RowCol_Coord_Mapping for the mapping from the
        /// coordinates in the Euclidean plane to the row and column indexes
        /// </summary>
        public RowCol_Coord_Mapping Coord2RowColMapping { get; private set; }

        /// <summary>
        /// The delegate for mapping the coordinates to the color
        /// </summary>
        public Func<double, double, Color> Map => (double x, double y) =>
        {
            RowColForHash rowcol = Coord2RowColMapping.CoordToRowColMapping(x, y);
            return RowCol2Color(rowcol.RowIdx, rowcol.ColIdx);
        };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rowCol2Color"></param>
        /// <param name="coord2RowColMapping"></param>
        public DistortedVisualizationMapping(RowColToColorMapping rowCol2Color, RowCol_Coord_Mapping coord2RowColMapping)
        {
            RowCol2Color = rowCol2Color;
            Coord2RowColMapping = coord2RowColMapping;
        }
    }
}
