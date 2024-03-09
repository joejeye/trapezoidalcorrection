using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageDistorsion
{
    internal class ChessBoard
    {
        // Number of squares along the horizontal direction
        public int BoardWidth { get; set; }

        // Number of squares along the vertical direction
        public int BoardHeight { get; set;}

        // Number of dots for a square along the horizontal direction
        public int SquareWidth { get; set; }

        // Number of dots for a square along the vertical direction
        public int SquareHeight { get; set;}

        // The total number of points in the chessboard along the horizontal direction
        public int SizeW { get; }

        // The total number of points in the chessboard along the vertical direction
        public int SizeH { get; }

        public ChessBoard(
            int BW = 10,
            int BH = 10,
            int SW = 3,
            int SH = 3
            )
        {
            BoardWidth = BW;
            BoardHeight = BH;
            SquareWidth = SW;
            SquareHeight = SH;

            SizeW = BoardWidth * SquareWidth;
            SizeH = BoardHeight * SquareHeight;
        }

        // Overload the operator []
        public bool this[int dotRowIdx, int dotColIdx]
        {
            get {
                ValidDotIdx(dotRowIdx, true);
                ValidDotIdx(dotColIdx, false);
                return BinValAt(dotRowIdx, true) ^ BinValAt(dotColIdx, false);
            }            
        }

        bool BinValAt(int idx, bool horizontal)
        {
            int n;
            if (horizontal)
            {
                n = SquareWidth;
            } else
            {
                n = SquareHeight;
            }

            int r = idx % (2 * n);
            return r < n;
        }

        void ValidDotIdx(int dotIdx, bool horizontal)
        {
            int n;
            if (horizontal)
            {
                n = SizeW;
            } else
            {
                n = SizeH;
            }

            if (!(0 <= dotIdx) && (dotIdx < n))
            {
                throw new IndexOutOfRangeException("Dot index out of range");
            }
        }
    }
}
