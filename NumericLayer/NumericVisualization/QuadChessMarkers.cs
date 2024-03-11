using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ImageDistorsion.NumericLayer.NumericVisualization
{
    using VecDbl = Vector<double>;

    internal class QuadChessMarkers : IEnumerable
    {
        public QuadChessMarkers(FlexChessboard fc, ConvexPolygon cp, double spx, double xpy)
        {
            Chsbrd = fc;
            Quadrilateral = cp;
            MarkerSpacingX = spx;
            MarkerSpacingY = xpy;

            if (_Chsbrd == null || _Quadrilateral == null)
            {
                throw new ArgumentNullException("");
            }
        }

        public QCMEnumerator GetEnumerator()
        {
            return new QCMEnumerator(this);
        }

        // Implement interface method
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        /// <summary>
        /// The background chessboard
        /// </summary>
        private FlexChessboard _Chsbrd;
        public FlexChessboard Chsbrd
        {
            get => _Chsbrd;
            private set => _Chsbrd = value;
        }
        
        /// <summary>
        /// The quadrilaterl to be filled with markers
        /// </summary>
        private ConvexPolygon _Quadrilateral;
        public ConvexPolygon Quadrilateral
        {
            get => _Quadrilateral;
            private set => _Quadrilateral = value;
        }

        private double _MarkerSpacingX;
        public double MarkerSpacingX
        {
            get => _MarkerSpacingX;
            private set => _MarkerSpacingX = value;
        }

        private double _MarkerSpacingY;
        public double MarkerSpacingY
        {
            get => _MarkerSpacingY;
            private set => _MarkerSpacingY = value;
        }
    }

    internal class QCMEnumerator : IEnumerator
    {
        private double _X;
        public double X
        {
            get => _X;
            private set => _X = value;
        }

        private double _Y;
        public double Y
        {
            get => _Y;
            private set => _Y = value;
        }

        private QuadChessMarkers _qcm;
        public QuadChessMarkers QCM
        {
            get => _qcm;
            private set => _qcm = value;
        }

        public QCMEnumerator(QuadChessMarkers qcm)
        {
            QCM = qcm;
            Reset();

            if (_qcm == null)
            {
                throw new ArgumentNullException(nameof(QCM));
            }
        }

        public void Reset()
        {
            X = QCM.Chsbrd.Xmin - QCM.MarkerSpacingX;
            Y = QCM.Chsbrd.Ymin;
        }

        public bool MoveNext()
        {
            if (X <= QCM.Chsbrd.Xmax - QCM.MarkerSpacingX)
            {
                X += QCM.MarkerSpacingX;
                if (QCM.Quadrilateral.ContainsPoint(VecDbl.Build.DenseOfArray([X, Y]), out _))
                {
                    return true;
                } else
                {
                    return MoveNext();
                }                
            } else if (Y <= QCM.Chsbrd.Ymax - QCM.MarkerSpacingY)
            {
                X = QCM.Chsbrd.Xmin;
                Y += QCM.MarkerSpacingY;
                if (QCM.Quadrilateral.ContainsPoint(VecDbl.Build.DenseOfArray([X, Y]), out _))
                {
                    return true;
                }
                else
                {
                    return MoveNext();
                }
            } else
            {
                return false;
            }
        }

        public NuMarker<ScottPlot.Color> Current
        {
            get => new(x: X, y: Y, color: QCM.Chsbrd[X, Y]);
        }

        // Implement interface property
        object IEnumerator.Current => Current;
    }
}
