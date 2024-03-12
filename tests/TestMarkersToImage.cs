using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageDistorsion;
using System.Drawing;

namespace ImageDistorsion.Tests
{
    /// <summary>
    /// Test the following workflow:
    /// 1. Generate markers in the chessboard
    /// 2. Convert the markers to pixel array
    /// 3. Format the pixel array to bitmap image and save it.
    /// </summary>
    internal class TestMarkersToImage
    {
        public static void Run(string fileName = "test_MarkersToImage")
        {
            // Create the markers of the chessboard
            NumericLayer.Chessboard Chsbrd = new();
            NumericLayer.NumericVisualization.ChessboardMarkers markers = new(Chsbrd);

            // Convert the markers to a pixel array (2-dimensional)
            PixelLayer.MarkersToPixels Mkr2Pix = new(markers.NPointsH, markers.NPointsV);
            foreach(var nmk in markers)
            {
                Mkr2Pix.LoadMarker(
                    new NumericLayer.NumericVisualization.NuMarker<Color>(
                        nmk.x, nmk.y, Color.FromArgb( // Convert from Scottplot.Color to System.Drawing.Color
                            nmk.color.R, nmk.color.G, nmk.color.B
                        )
                    )
                );
            }
            Mkr2Pix.FinishLoading(); // This is necessary to create the pixel array

            // Save the pixel array to the bmp image file
            FormattingLayer.PixelArrayToBitmap PA2Bmp = new(Mkr2Pix.PixArray2D);
            PA2Bmp.SaveToBmp(GlobalConstants.ProjectTempFilePath +  fileName);

            Console.WriteLine("TestMarkersToImage finished");
        }
    }
}
