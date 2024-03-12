using ImageDistorsion.PlayGround;
using ImageDistorsion.Tests;

namespace ImageDistorsion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*Test the class ChessboardMarkers*/
            //TestChessboardVisualization.Run();

            /*Test class Distort*/
            //TestDistort.Run();

            /* Test markers in quadrilateral
             */
            //TestQuadlatVisualization.Run();

            /* Test the class InverseDistort
             */
            //TestInverseDistort.Run();

            /* Test the class TestPixelsToMarkers
             */
            //TestPixelsToMarkers.Run();

            /* Test the workflow of creating chessboard markers and
             * then converting the markers to pixels and then save the
             * 2D array of pixels to a bitmap-format image
             */
            //TestMarkersToImage.Run();

            /* Test save the bmp image of markers in a quadrilateral
             */
            //TestQuadToImage.Run();

            /* Test convex hull algorithm
             */
            //TestConvexHull.Run();
        }
}
}
