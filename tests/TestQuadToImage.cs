using ImageDistorsion.NumericLayer.NumericVisualization;
using ImageDistorsion.NumericLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageDistorsion.PixelLayer;
using System.Drawing;
using ImageDistorsion.FormattingLayer;

namespace ImageDistorsion.Tests
{
    internal class TestQuadToImage
    {
        public static void Run(string fileName = "test_QuadToImage")
        {
            /* Create markers in a quadrilateral chessboard
             */
            double[][] ABCD = [[0, 0], [40, -10], [50, 60], [5, 50]];
            ConvexPolygon Quadlat = new(ABCD);
            double squareXSpan = 3;
            double squareYSpan = 3;
            FlexChessboard flexCb = new(Quadlat, squareXSpan, squareYSpan);
            double spacingX = 1;
            double spacingY = 1;
            QuadChessMarkers QCM = new(flexCb, Quadlat, spacingX, spacingY);

            /* Conver the markers to a pixel array
             */
            int NPointsX = (int)Math.Ceiling(QCM.Quadrilateral.MaxXDist / QCM.MarkerSpacingX);
            int NPointsY = (int)Math.Ceiling(QCM.Quadrilateral.MaxYDist / QCM.MarkerSpacingY);
            MarkersToPixels Mkr2Pix = new(NPointsX, NPointsY);
            foreach (var nmk in QCM)
            {
                Mkr2Pix.LoadMarker(new NuMarker<Color>(nmk.x, nmk.y, Color.FromArgb(
                    nmk.color.R, nmk.color.G, nmk.color.B)));
            }
            Mkr2Pix.FinishLoading();

            /* Save the pixel array to a bmp format image file
             */
            PixelArrayToBitmap PA2Bmp = new(Mkr2Pix.PixArray2D);
            PA2Bmp.SaveToBmp(GlobalConstants.ProjectTempFilePath + fileName);

            Console.WriteLine("TestQuadToImage finished");
        }
    }
}
