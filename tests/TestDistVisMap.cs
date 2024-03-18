using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ImageDistorsion.PixelLayer;
using ImageDistorsion.NumericLayer;
using ImageDistorsion.NumericLayer.NumericVisualization;

namespace ImageDistorsion.Tests
{
    /// <summary>
    /// Test the class DistortedVisualizationMapping
    /// </summary>
    public class TestDistVisMap
    {
        private const string subfolder = @"TestDistVisMap\";
        /// <summary>
        /// Run the test
        /// </summary>
        /// <param name="fileName"></param>
        public static void Run(string fileName = subfolder + "test_DistortedVisualizationMapping")
        {
            /* Construct the mapping from the pixels row-col indexes to the corrseponding
             * color.
             * 
             * The size of the pixel array is 21 in width and 11 in height. The four vertices
             * of the quadrilaterl are [5, 10], [11, 10], [5, 21], and [0, 10].
             * 
             * Pixels inside the quadrilateral are colored white, while those outside are
             * colored black.
             */
            Color Pix2ColorMapping(int row, int col)
            {
                var sideA = (int i, int j) => 2 * i + j >= 10;
                var sideB = (int i, int j) => 5 * i - 3 * j <= 25;
                var sideC = (int i, int j) => 11 * i + 6 * j <= 181;
                var sideD = (int i, int j) => -11 * i + 5 * j <= 50;
                bool flagA = sideA(row, col); ;
                bool flagB = sideB(row, col);
                bool flagC = sideC(row, col);
                bool flagD = sideD(row, col);
                if (flagA && flagB && flagC && flagD)
                {
                    return Color.White;
                }
                else
                {
                    return Color.Black;
                }
            }

            /* Visualize the pixel array */
            Bitmap bmp = new(21, 11);
            for (int colIdx = 0; colIdx < 21; colIdx++)
            {
                for (int rowIdx = 0; rowIdx < 11; rowIdx++)
                {
                    bmp.SetPixel(colIdx, rowIdx, Pix2ColorMapping(rowIdx, colIdx));
                }
            }
            // Save the bitmap image
            string filePath = GlobalConstants.ProjectTempFilePath + subfolder + "PixelLayerView";
            bmp.Save(filePath + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);

            /* Construct the mapping from the numeric layer Euclidean plane coordinates to the
             * pixels row-col indexes.
             * 
             * The width and length of the bounding box of the quadrilaterral in the Euclidean
             * plane is by default 20 and 10 respectively. The lower-left corner of the bounding
             * box is by default (0, 0).
             */
            RowCol_Coord_Mapping Coord2RowCol = new(21, 11);

            /* Create the pullback
             */
            DistortedVisualizationMapping DistVisMap = new(Pix2ColorMapping, Coord2RowCol);

            /* Set the horizontal and vertical image resolution
             */
            double spacingX = 0.5;
            double spacingY = 0.5;

            /* Create the image plotter
             */
            ImagePlotter plotter = new(fileName);

            /* Add markers to the plotter
             */
            for (double x = 0; x <= 20; x += spacingX)
            {
                for (double y = 0; y <= 10; y += spacingY)
                {
                    plotter.AddMarker(x, y, DistVisMap.Map(x, y));
                }
            }

            /* Save the plot */
            plotter.SaveImagePNG();

            /* Construct the numeric layer pullback
             */
            RectPolygon correctedDomain = new(new double[] { 0, 0 }, new double[] { 1, 1 });
            int[][] PixVerts = [[5, 10], [11, 10], [5, 21], [0, 10]];
            Coord2ForHash<double>[] NuVerts = (from pv in PixVerts
                                               select DistVisMap.Coord2RowColMapping.RowColToCoordMapping(pv[0], pv[1])).ToArray();
            ConvexPolygon quadlat = new(
                (from nv in NuVerts
                 select new double[] { nv.x, nv.y }
                ).ToArray()
            );
            NumericLayer.PullbackCorrection NuPullback = new(quadlat, correctedDomain, (double x, double y) => DistVisMap.Map(x, y));

            /* Visualize the numeric layer pullback
             */
            spacingX = 0.05;
            spacingY = 0.05;
            plotter = new(GlobalConstants.ProjectTempFilePath + subfolder + "test_NuPullback");
            for (double x = 0; x <= 1; x += spacingX)
            {
                for (double y = 0; y <= 1; y += spacingY)
                {
                    plotter.AddMarker(x, y, NuPullback.VisMap(x, y));
                }
            }
            // Save the image for inspection
            plotter.SaveImagePNG();


            Console.WriteLine("TestDistVisMap completed");
        }
    }
}
