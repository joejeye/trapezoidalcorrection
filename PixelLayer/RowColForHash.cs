using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageDistorsion.PixelLayer
{
    /// <summary>
    /// Wrapper for row and column index for hash
    /// </summary>
    /// <param name="rowIdx"></param>
    /// <param name="colIdx"></param>
    public readonly struct RowColForHash(int rowIdx, int colIdx)
    {
        /// <summary>
        /// Row index
        /// </summary>
        public readonly int RowIdx = rowIdx;

        /// <summary>
        /// Column index
        /// </summary>
        public readonly int ColIdx = colIdx;

        /// <summary>
        /// Convert to array
        /// </summary>
        /// <returns></returns>
        public int[] ToArray()
        {
            return [RowIdx, ColIdx];
        }

        /// <summary>
        /// Get the comparer
        /// </summary>
        /// <returns></returns>
        public static RowColComparer GetEqCmpr() => new();
    }

    /// <summary>
    /// Comparer for RowColForHash
    /// </summary>
    public class RowColComparer : IEqualityComparer<RowColForHash>
    {
        /// <summary>
        /// Implement the Equals method
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(RowColForHash x, RowColForHash y)
        {
            return x.RowIdx == y.RowIdx && x.ColIdx == y.ColIdx;
        }

        /// <summary>
        /// Implement the GetHashCode method
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(RowColForHash obj) => obj.GetHashCode();
    }

}
