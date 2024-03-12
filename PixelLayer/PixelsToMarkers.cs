using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ImageDistorsion.NumericLayer.NumericVisualization;

namespace ImageDistorsion.PixelLayer
{
    internal class PixelsToMarkers : IEnumerable
    {
        private ColorArray2D _ColorArr;
        public ColorArray2D ColorArr { get => _ColorArr; private set => _ColorArr = value; }
        public int ImgWidth => ColorArr.ImgBmp.Width;
        public int ImgHeight => ColorArr.ImgBmp.Height;

        public PixelsToMarkers(ColorArray2D ca2d)
        {
            _ColorArr = ca2d;
        }

        public PixelsToMarkers(string ImgFilePath)
            : this(new ColorArray2D(ImgFilePath)) { }

        public PTMEnumerator GetEnumerator()
        {
            return new(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    internal class PTMEnumerator : IEnumerator
    {
        private PixelsToMarkers PTM { get; }
        private int rowIdx { set; get; }
        private int colIdx { set; get; }
        private int ImgWidth => PTM.ImgWidth;
        private int ImgHeight => PTM.ImgHeight;

        public PTMEnumerator(PixelsToMarkers ptm)
        {
            PTM = ptm;
            Reset();
        }

        public void Reset()
        {
            rowIdx = 0;
            colIdx = -1;
        }

        public bool MoveNext()
        {
            if (++colIdx < ImgWidth)
            {
                return true;
            } else if (++rowIdx < ImgHeight)
            {
                colIdx = 0;
                return true;
            } else
            {
                return false;
            }
        }

        public NuMarker<Color> Current => new(colIdx, ImgHeight - 1 - rowIdx, PTM.ColorArr[rowIdx, colIdx]);

        object IEnumerator.Current => Current;
    }
}
