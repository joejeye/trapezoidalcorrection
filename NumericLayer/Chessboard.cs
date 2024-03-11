using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ImageDistorsion.NumericLayer
{
    using VecDbl = Vector<double>;

    internal class Chessboard
    {
        public int NumSquaresH { get; }
        public int NumSquaresV { get; }
        public double SqSideLenH { get; }
        public double SqSideLenV { get; }
        public double ChessboardLenH { get => NumSquaresH * SqSideLenH; }
        public double ChessboardLenV { get => NumSquaresV * SqSideLenV; }

        public Chessboard(int NSH = 10, int NSV = 10, int SLH = 3, int SLV = 3)
        {
            NumSquaresH = NSH;
            NumSquaresV = NSV;
            SqSideLenH = SLH;
            SqSideLenV = SLV;
        }
        
        // Retrive the color at the position (x, y)
        public ScottPlot.Color this[double x, double y]
        {
            get
            {
                ValidatePositions(x, y);
                bool FlagX = (x % (2 * SqSideLenH)) < SqSideLenH;
                bool FlagY = (y % (2 * SqSideLenV)) < SqSideLenV;
                bool FlagXY = FlagX ^ FlagY;
                if (FlagXY)
                {
                    return ScottPlot.Colors.LightGray;
                } else
                {
                    return ScottPlot.Colors.Black;
                }
            }
        }

        // Overload operator []
        public ScottPlot.Color this[VecDbl vd]
        {
            get
            {
                return this[vd[0], vd[1]];
            }
        }

        void ValidatePositions(double x, double y)
        {
            if (!(0 <= x && x <= ChessboardLenH))
            {
                throw new ArgumentException($"The value of x ({x}) is out of the range [0, {ChessboardLenH}]");
            }
            if (!(0 <= y && y <= ChessboardLenV))
            {
                throw new ArgumentException($"The value of y ({y}) is out of the range [0, {ChessboardLenV}]");
            }
        }
    }
}
