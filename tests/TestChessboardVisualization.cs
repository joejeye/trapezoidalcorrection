using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageDistorsion.NumericLayer.NumericVisualization;
using ImageDistorsion.NumericLayer;

namespace ImageDistorsion.Tests
{
    internal class TestChessboardVisualization
    {
        public static void Run(string fileName = "test_ChessboardMarkers")
        {
            Chessboard ChsBrd = new();
            ChessboardMarkers CBMkrs = new(ChsBrd);
            ImagePlotter plotter = new(fileName);
            foreach (var nmkr in CBMkrs)
            {
                plotter.AddMarker(nmkr);
            }

            plotter.SaveImagePNG();
            Console.WriteLine("TestChessboardVisualization finished");
        }
    }
}
