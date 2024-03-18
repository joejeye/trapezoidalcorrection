using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageDistorsion.PixelLayer
{
    /// <summary>
    /// Get the bouding rectangle for a set of lattice points
    /// </summary>
    public class BoundingFrame
    {
        /// <summary>
        /// The flag indicating if the reading has been finished
        /// </summary>
        public bool ReadFinished { get; private set; } = false;
        
        private int RowMin { get; set; } = int.MaxValue;
        private int RowMax { get; set; } = int.MinValue;
        private int ColMin { get; set; } = int.MaxValue;
        private int ColMax { get; set; } = int.MinValue;

        /// <summary>
        /// The top-left row and column indexes of the bounding frame
        /// </summary>
        public RowColForHash TopLeftRowCol
        {
            get
            {
                if (!ReadFinished)
                {
                    throw new InvalidOperationException("The reading has not been finished");
                }
                return new(RowMin, ColMin);
            }
        }

        /// <summary>
        /// The bottom-right row and column indexes of the bounding frame
        /// </summary>
        public RowColForHash BottomRightRowCol
        {
            get
            {
                if (!ReadFinished)
                {
                    throw new InvalidOperationException("The reading has not been finished");
                }
                return new(RowMax, ColMax);
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public BoundingFrame() { }

        /// <summary>
        /// Construct the bounding frame and read one lattice point
        /// </summary>
        /// <param name="rowIdx"></param>
        /// <param name="colIdx"></param>
        public BoundingFrame(int rowIdx, int colIdx)
        {
            ReadLatticePoint(rowIdx, colIdx);
        }

        /// <summary>
        /// Construct the bounding frame and read multiple lattice points
        /// </summary>
        /// <param name="points"></param>
        public BoundingFrame(int[][] points)
        {
            ReadLatticePoint(points);
        }

        /// <summary>
        /// Construct the bounding frame and read one lattice point
        /// </summary>
        /// <param name="rowCol"></param>
        public BoundingFrame(RowColForHash rowCol)
        {
            ReadLatticePoint(rowCol);
        }

        /// <summary>
        /// Construct the bounding frame and read multiple lattice points
        /// </summary>
        /// <param name="rowCols"></param>
        public BoundingFrame(RowColForHash[] rowCols)
        {
            ReadLatticePoint(rowCols);
        }

        /// <summary>
        /// Read one lattice point
        /// </summary>
        /// <param name="rowIdx"></param>
        /// <param name="colIdx"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void ReadLatticePoint(int rowIdx, int colIdx)
        {
            if (ReadFinished)
            {
                throw new InvalidOperationException("The reading has been finished");
            }
            RowMin = Math.Min(RowMin, rowIdx);
            RowMax = Math.Max(RowMax, rowIdx);
            ColMin = Math.Min(ColMin, colIdx);
            ColMax = Math.Max(ColMax, colIdx);
        }

        /// <summary>
        /// Read multiple lattice points
        /// </summary>
        /// <param name="points"></param>
        public void ReadLatticePoint(int[][] points)
        {
            foreach (var point in points)
            {
                ReadLatticePoint(point[0], point[1]);
            }
        }

        /// <summary>
        /// Read one lattice point
        /// </summary>
        /// <param name="rowCol"></param>
        public void ReadLatticePoint(RowColForHash rowCol)
        {
            ReadLatticePoint(rowCol.RowIdx, rowCol.ColIdx);
        }

        /// <summary>
        /// Read multiple lattice points
        /// </summary>
        /// <param name="rowCols"></param>
        public void ReadLatticePoint(RowColForHash[] rowCols)
        {
            foreach (var rowCol in rowCols)
            {
                ReadLatticePoint(rowCol);
            }
        }

        /// <summary>
        /// Mark the reading as finished
        /// </summary>
        public void FinishReading()
        {
            ReadFinished = true;
        }
    }
}
