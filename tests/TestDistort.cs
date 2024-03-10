using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageDistorsion.NumericLayer;
using ImageDistorsion.NumericLayer.NumericVisualization;
using MathNet.Numerics.LinearAlgebra;

namespace ImageDistorsion.Tests
{
    using VecDbl = Vector<double>;

    internal class TestDistort
    {
        public static void Run(string fileName = "test_Distort")
        {
            /*
             * Create the coordinates of the four vertices of the distorted
             * quadrilateral. The order A->B->C->D is counter-clockwise
             */
            double[][] abcd = [[0, 0], [40, -10], [50, 60], [5, 50]];
            VecDbl[] ABCD_Prime = (
                from v2 in abcd
                select VecDbl.Build.DenseOfArray(v2)
                ).ToArray();

            // Create the chessboard
            Chessboard board = new();

            // Create the distortion mapping
            Distort dstrt = new(board.ChessboardLenH, board.ChessboardLenV, ABCD_Prime);

            // Create the enumerable chessboard markers
            ChessboardMarkers CBMkrs = new(board);

            // Create the blank plot
            var myPlot = new ScottPlot.Plot();
            
            // Map each marker from the chessboard to the distorted quadrilateral and plot the marker
            foreach (var nmk in CBMkrs)
            {
                var imgPnt = dstrt.DistortMapping(nmk.GetVecDouble());
                NuMarker<ScottPlot.Color> imgMkr = new(imgPnt[0], imgPnt[1], nmk.color);
                AddMkr(myPlot, imgMkr);
            }

            // Save the image
            myPlot.SavePng(GlobalConstants.ProjectTempFilePath + fileName + ".png", 500, 500);
            Console.WriteLine("TestDistort finished");
        }

        /// <summary>
        /// Add one marker to the plot
        /// </summary>
        /// <param name="myPlot">The plot where the marker is drawn</param>
        /// <param name="nmk">The marker to be drawn</param>
        public static void AddMkr(ScottPlot.Plot myPlot, NuMarker<ScottPlot.Color> nmk)
        {
            myPlot.Add.Marker(nmk.x, nmk.y, color: nmk.color);
        }
    }
}
