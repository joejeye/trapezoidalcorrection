using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageDistorsion.PixelLayer
{
    public readonly struct RowColForHash(int rowIdx, int colIdx)
    {
        public readonly int RowIdx = rowIdx;
        public readonly int ColIdx = colIdx;

        public int[] ToArray()
        {
            return [RowIdx, ColIdx];
        }

        public static RowColComparer GetEqCmpr() => new();
    }

    public class RowColComparer : IEqualityComparer<RowColForHash>
    {
        public bool Equals(RowColForHash x, RowColForHash y)
        {
            return x.RowIdx == y.RowIdx && x.ColIdx == y.ColIdx;
        }

        public int GetHashCode(RowColForHash obj) => obj.GetHashCode();
    }

}
