using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageDistorsion.NumericLayer;

namespace ImageDistorsion.PixelLayer
{
    /// <summary>
    /// Create mappings between the [row, column] pixel indexes and
    /// (x, y) numeric point coordinates. 
    /// The [0, 0] pixel is at the
    /// upper-left corner of the image, while the (0, 0) coordinate 
    /// is at the lower-left corner of the image.
    /// The mappings are designed such that the relative positions
    /// of the pixels are preserved.
    /// </summary>
    public class RowCol_Coord_Mapping
    {
        // The number of pixels along the horizontal
        // and the vertical directions respectively
        public int NHorizontalPixs { get; private set; }
        public int NVerticalPixs { get; private set; }

        // The length of the image in the Euclidean 
        // coordinate system along x- and y- axes
        // respectively
        public double XSpan { get; private set; }
        public double YSpan { get; private set; }

        // The numeric point spacing along x- and y- axes respectively
        public double SpacingX { get => XSpan / (NHorizontalPixs - 1); }
        public double SpacingY { get => YSpan / (NVerticalPixs - 1); }

        /// <summary>
        /// The coordinate of the lower-left corner of the image in the
        /// Euclidean plane
        /// </summary>
        public double[] LLCoord { get; private set; }

        /// <summary>
        /// Construct the mapping between the [row, column] pixel indexes
        /// </summary>
        /// <param name="NHP"></param>
        /// <param name="NVP"></param>
        /// <param name="xspan"></param>
        /// <param name="yspan"></param>
        /// <param name="llcoord"></param>
        /// <exception cref="ArgumentException"></exception>
        public RowCol_Coord_Mapping(int NHP, int NVP, double xspan = -1, double yspan = -1, double[]? llcoord = null)
        {
            NHorizontalPixs = NHP;
            NVerticalPixs = NVP;

            if (xspan == -1)
            {
                XSpan = NHorizontalPixs - 1;
            } else
            {
                if (xspan <= 0)
                {
                    throw new ArgumentException("xspan must be positive");
                }
                XSpan = xspan;
            }

            if (yspan == -1)
            {
                YSpan = NVerticalPixs - 1;
            } else
            {
                if (yspan <= 0)
                {
                    throw new ArgumentException("yspan must be positive");
                }
                YSpan = yspan;
            }            
            
            if (llcoord != null)
            {
                if (llcoord.Length != 2)
                {
                    throw new ArgumentException("The coordinate must has a length of 2");
                }
                LLCoord = llcoord;
            } else
            {
                LLCoord = [0, 0];
            }
        }

        /// <summary>
        /// Map the row and column indexes to the (x, y) coordinates
        /// </summary>
        public Func<int, int, Coord2ForHash<double>> RowColToCoordMapping
        {
            get => (int rowIdx, int colIdx) =>
            {
                if (rowIdx >= NVerticalPixs || colIdx >= NHorizontalPixs)
                {
                    throw new IndexOutOfRangeException();
                }
                double x = colIdx * SpacingX + LLCoord[0];
                double y = (NVerticalPixs - 1 - rowIdx) * SpacingY + LLCoord[1];
                return new(x, y);
            };
        }

        /// <summary>
        /// Map the (x, y) coordinates to the row and column indexes
        /// </summary>
        public Func<double, double, RowColForHash> CoordToRowColMapping
        {
            get => (double x, double y) =>
            {
                if (x > XSpan + LLCoord[0] || y > YSpan + LLCoord[1])
                {
                    throw new IndexOutOfRangeException();
                }

                // Find the nearest lattice point
                int rowIdx = NVerticalPixs - 1 - (int)Math.Round((y - LLCoord[1]) / SpacingY);
                int colIdx = (int)Math.Round((x - LLCoord[0]) / SpacingX);
                return new(rowIdx, colIdx);
            };
        }
    }
}
