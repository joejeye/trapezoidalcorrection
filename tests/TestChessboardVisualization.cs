using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageDistorsion.NumericLayer.NumericVisualization;
using ImageDistorsion.NumericLayer;

namespace ImageDistorsion.tests
{
    internal class TestChessboardVisualization
    {
        public static void Run(string fileName = "test_ChessboardMarkers")
        {
            Chessboard ChsBrd = new();
            ChessboardMarkers CBMkrs = new(ChsBrd);
            ScottPlot.Plot myPlot = new();
            foreach (var nmkr in CBMkrs)
            {
                myPlot.Add.Marker(nmkr.x, nmkr.y, color: nmkr.color);
            }

            myPlot.SavePng(GlobalConstants.ProjectTempFilePath + fileName + ".png", 500, 500);
            Console.WriteLine("TestChessboardVisualization finished");
        }
    }
}
