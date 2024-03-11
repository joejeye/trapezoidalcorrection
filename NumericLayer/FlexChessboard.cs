using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ImageDistorsion.NumericLayer
{
    using VecDbl = Vector<double>;

    internal class FlexChessboard
    {
        private RectPolygon _RectangleFrame;
        public RectPolygon RectangleFrame
        {
            get => _RectangleFrame;
            private set => _RectangleFrame = value;
        }
        public double Xmin { get => RectangleFrame.Vertices[0][0]; }
        public double Xmax { get => RectangleFrame.Vertices[1][0]; }
        public double Ymin { get => RectangleFrame.Vertices[0][1]; }
        public double Ymax { get => RectangleFrame.Vertices[2][1]; }

        private double _SqXSpan;
        public double SqXSpan
        {
            get => _SqXSpan;
            private set => _SqXSpan = value;
        }

        private double _SqYSpan;
        public double SqYSpan
        {
            get => _SqYSpan;
            private set => _SqYSpan = value;
        }

        public FlexChessboard(RectPolygon rf, double sqx, double sqy)
        {
            _RectangleFrame = rf;
            _SqXSpan = sqx;
            _SqYSpan = sqy;

            if (RectangleFrame == null)
            {
                throw new ArgumentNullException(nameof(RectangleFrame));
            }
        }

        public FlexChessboard(ConvexPolygon Quadlat, double sqx, double sqy)
            : this(CreatRectFrame(Quadlat), sqx, sqy) { }

        private static RectPolygon CreatRectFrame(ConvexPolygon Quadlat)
        {
            var XCoords = (from vd in Quadlat.Vertices
                           select vd[0]);
            var YCoords = (from vd in Quadlat.Vertices
                           select vd[1]);
            VecDbl lowerLeftVertex = VecDbl.Build.DenseOfArray([XCoords.Min(), YCoords.Min()]);
            VecDbl upperRightVertex = VecDbl.Build.DenseOfArray([XCoords.Max(), YCoords.Max()]);
            return new RectPolygon(lowerLeftVertex, upperRightVertex);
        }

        private static ScottPlot.Color ColorInterpreter(bool isWhite)
        {
            return isWhite ? ScottPlot.Colors.LightGray : ScottPlot.Colors.Black;
        }

        public ScottPlot.Color this[double x, double y]
        {
            get
            {
                if (!RectangleFrame.ContainsPoint(VecDbl.Build.DenseOfArray([x, y]), out bool _))
                {
                    throw new Exception("The point of query is out of the rectangle");
                }

                double xdist = x - RectangleFrame.Vertices[0][0];
                double ydist = y - RectangleFrame.Vertices[0][1];
                bool FlagX = (xdist % (2 * SqXSpan)) < SqXSpan;
                bool FlagY = (ydist % (2 * SqYSpan)) < SqYSpan;
                bool FlagXY = FlagX ^ FlagY;
                return ColorInterpreter(FlagXY);
            }
        }

        public ScottPlot.Color this[VecDbl vd]
        {
            get
            {
                if (vd.Count != 2)
                {
                    throw new ArgumentException("The queried vector must be 2-dimensional");
                }
                return this[vd[0], vd[1]];
            }
        }
    }
}
