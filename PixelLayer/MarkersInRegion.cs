using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageDistorsion.NumericLayer;
using ImageDistorsion.NumericLayer.NumericVisualization;
using MathNet.Numerics.LinearAlgebra;
using System.Drawing;

namespace ImageDistorsion.PixelLayer
{
    using VecDbl = Vector<double>;

    public class MarkersInRegion : IEnumerable
    {
        /// <summary>
        /// The object that can access the color info of an image at
        /// the i-th row and the j-th column using the overloaded 
        /// operator [i, j]. Note, this is not an array.
        /// </summary>
        private ColorArray2D _PixArr;
        public ColorArray2D PixArr { get => _PixArr; private set => _PixArr = value; }

        /// <summary>
        /// The selected region in the image that is to be deformed, represented
        /// as a convex polygon
        /// </summary>
        private ConvexPolygon _SelectedRegion;
        public ConvexPolygon SelectedRegion { get => _SelectedRegion; private set => _SelectedRegion = value; }

        public int[][] VertexRowColIdxs { get; private set; }

        public RowCol_Coord_Mapping RCCrdMap { get; private set; }

        public MarkersInRegion(ColorArray2D ca2d, int[][] vertexRowColIdxs)
        {
            _PixArr = ca2d;
            foreach (var vrc in vertexRowColIdxs)
            {
                if (vrc[0] >= PixArr.NVerticalPix || vrc[1] >= PixArr.NHorizontalPix)
                {
                    throw new IndexOutOfRangeException("The vertex indexes must be within the image");
                }
            }
            this.VertexRowColIdxs = vertexRowColIdxs;
            GetBoundingRectangle(out int[] upperLeftRowCol, out int[] lowerRightRowCol);
            Crop(upperLeftRowCol, lowerRightRowCol);
            RCCrdMap = new RowCol_Coord_Mapping(PixArr.NHorizontalPix, PixArr.NVerticalPix);
            var coord4hashes = from vrc in this.VertexRowColIdxs
                               select RCCrdMap.RowColToCoordMapping(vrc[0], vrc[1]);
            var nuVertices = from coord in coord4hashes
                             select new double[] { coord.x, coord.y };
            _SelectedRegion = new ConvexPolygon(nuVertices.ToArray());
        }

        public MarkersInRegion(string fileFullPath, int[][] VertexRowColIdxs)
            : this(new ColorArray2D(fileFullPath), VertexRowColIdxs) { }

        public MIREnumerator GetEnumerator() => new(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void Crop(int[] upperLeftRowCol, int[] lowerRightRowCol)
        {
            PixArr.Crop(upperLeftRowCol, lowerRightRowCol);
            for (int vertexIdx = 0; vertexIdx < VertexRowColIdxs.Length; vertexIdx++)
            {
                VertexRowColIdxs[vertexIdx][0] -= upperLeftRowCol[0];
                if (VertexRowColIdxs[vertexIdx][0] >= PixArr.NVerticalPix)
                {
                    throw new IndexOutOfRangeException("Row index out of  selected region");
                }
                VertexRowColIdxs[vertexIdx][1] -= upperLeftRowCol[1];
                if (VertexRowColIdxs[vertexIdx][1] >= PixArr.NHorizontalPix)
                {
                    throw new IndexOutOfRangeException("Column index out of selected region");
                }
            }
        }        

        private void GetBoundingRectangle(out int[] upperLeftRowCol, out int[] lowerRightRowCol)
        {
            int rowmin = (from vrc in VertexRowColIdxs select vrc[0]).Min();
            int rowmax = (from vrc in VertexRowColIdxs select vrc[0]).Max();
            int colmin = (from vrc in VertexRowColIdxs select vrc[1]).Min();
            int colmax = (from vrc in VertexRowColIdxs select vrc[1]).Max();
            if (rowmax >= PixArr.NVerticalPix || colmax >= PixArr.NHorizontalPix)
            {
                throw new IndexOutOfRangeException("The vertex indexes must be within the image");
            }
            upperLeftRowCol = [rowmin, colmin];
            lowerRightRowCol = [rowmax, colmax];
        }
    }

    public class MIREnumerator : IEnumerator
    {
        private MarkersInRegion MIR { get; }
        private int RowIdx { set; get; }
        private int ColIdx { set; get; }

        public MIREnumerator(MarkersInRegion mir)
        {
            MIR = mir;
            Reset();
        }

        public void Reset()
        {
            RowIdx = 0;
            ColIdx = -1;
        }

        public bool MoveNext()
        {
            if (++ColIdx < MIR.PixArr.NHorizontalPix)
            {
                Coord2ForHash<double> coord = MIR.RCCrdMap.RowColToCoordMapping(RowIdx, ColIdx);
                if (MIR.SelectedRegion.ContainsPoint(VecDbl.Build.DenseOfArray([coord.x, coord.y]), out _))
                {
                    return true;
                } else
                {
                    return MoveNext();
                }
            } else if (++RowIdx < MIR.PixArr.NVerticalPix)
            {
                ColIdx = 0;
                Coord2ForHash<double> coord = MIR.RCCrdMap.RowColToCoordMapping(RowIdx, ColIdx);
                if (MIR.SelectedRegion.ContainsPoint(VecDbl.Build.DenseOfArray([coord.x, coord.y]), out _))
                {
                    return true;
                } else
                {
                    return MoveNext();
                }
            } else
            {
                return false;
            }
        }

        public NuMarker<Color> Current
        {
            get
            {
                Coord2ForHash<double> coord = MIR.RCCrdMap.RowColToCoordMapping(RowIdx, ColIdx);
                return new(coord.x, coord.y, MIR.PixArr[RowIdx, ColIdx]);
            }
        }

        object IEnumerator.Current => Current;
    }
}
