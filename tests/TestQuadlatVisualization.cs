using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using ImageDistorsion.NumericLayer;
using ImageDistorsion.NumericLayer.NumericVisualization;

namespace ImageDistorsion.Tests
{
    using VecDbl = Vector<double>;

    internal class TestQuadlatVisualization
    {
        public static void Run(string fileName = "test_QuadrilateralMarkers")
        {
            /* Create the quadrilateral to be inversely distorted
             */
            double[][] ABCD = [[0, 0], [40, -10], [50, 60], [5, 50]];
            ConvexPolygon Quadlat = new(ABCD);
            double squareXSpan = 3;
            double squareYSpan = 3;
            FlexChessboard flexCb = new(Quadlat, squareXSpan, squareYSpan);
            double spacingX = 1;
            double spacingY = 1;
            QuadChessMarkers QCM = new(flexCb, Quadlat, spacingX, spacingY);

            /* Draw each marker
             */
            ImagePlotter plotter = new(fileName);
            foreach (var nmk in QCM)
            {
                plotter.AddMarker(nmk.x, nmk.y, nmk.color);
            }

            plotter.SaveImagePNG();
            Console.WriteLine("TestQuadlatVisualization finished");
        }
    }
}
