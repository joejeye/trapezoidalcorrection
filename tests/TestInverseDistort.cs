using ImageDistorsion.NumericLayer.NumericVisualization;
using ImageDistorsion.NumericLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageDistorsion.Tests
{
    internal class TestInverseDistort
    {
        public static void Run(string fileName = "test_InverseDistort")
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

            /* Inversely distort each marker and plot it
             */
            ImagePlotter plotter = new(fileName);
            InverseDistort invDist = new(Quadlat, Quadlat.MaxXDist, Quadlat.MaxYDist);
            foreach (var nmk in QCM)
            {
                var vd = invDist.Mapping(nmk.GetVecDouble());
                plotter.AddMarker(vd[0], vd[1], nmk.color);
            }

            plotter.SaveImagePNG();
            Console.WriteLine("TestInverseDistort finished");
        }
    }
}
