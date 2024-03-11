using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageDistorsion.NumericLayer.NumericVisualization
{
    internal class ChessboardMarkers/*<TColor>*/ : IEnumerable
    {
        public Chessboard Chbd { get; }
        public int NPointsH { get; }
        public int NPointsV { get; }
        public int NPointsTotal
        {
            get
            {
                return NPointsH * NPointsV;
            }
        }
        public double SideLenH { get; }
        public double SideLenV { get; }
        public double SpacingH
        {
            get
            {
                return SideLenH / ((double)NPointsH - 1);
            }
        }
        public double SpacingV
        {
            get
            {
                return SideLenV / ((double)NPointsV - 1);
            }
        }

        public ChessboardMarkers(Chessboard Chdb, int NPointsH = 31, int NPointsV = 31)
        {
            this.Chbd = Chdb;
            this.SideLenH = Chdb.ChessboardLenH;
            this.SideLenV = Chbd.ChessboardLenV;
            this.NPointsH = NPointsH;
            this.NPointsV = NPointsV;
        }

        public ChsbdMrkrEnumr GetEnumerator()
        {
            return new ChsbdMrkrEnumr(this);
        }

        // Implement interface method
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    }

    class ChsbdMrkrEnumr/*<TColor>*/ : IEnumerator
    {
        int XIdx;
        int YIdx;
        ChessboardMarkers/*<TColor>*/ ChMkrs;
        public double x
        {
            get
            {
                return XIdx * ChMkrs.SpacingH;
            }
        }
        public double y
        {
            get
            {
                return YIdx * ChMkrs.SpacingV;
            }
        }

        public ChsbdMrkrEnumr(ChessboardMarkers/*<TColor>*/ ChMkrs)
        {
            this.ChMkrs = ChMkrs;
            XIdx = -1;
            YIdx = 0;
        }

        public void Reset()
        {
            XIdx = -1;
            YIdx = 0;
        }

        public bool MoveNext()
        {
            if (++XIdx <= ChMkrs.NPointsH - 1)
            {
                return true;
            } else if (++YIdx <= ChMkrs.NPointsV - 1)
            {
                XIdx = 0;
                return true;
            } else
            {
                return false;
            }
        }

        public NuMarker<ScottPlot.Color> Current
        {
            get
            {
                return new NuMarker<ScottPlot.Color>(
                    x: x,
                    y: y,
                    color: ChMkrs.Chbd[x, y]
                );
            }
        }


        // Implement interface property
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }
    }
}
