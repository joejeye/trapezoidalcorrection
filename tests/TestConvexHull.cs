using ImageDistorsion.NumericLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageDistorsion.NumericLayer.NumericVisualization;
using MathNet.Numerics.LinearAlgebra;

namespace ImageDistorsion.Tests
{
    using VecDbl = Vector<double>;

    internal class TestConvexHull
    {
        public static void Run(string fileName = "test_ConvexHull")
        {
            /* Generate some random points on the 2D plane
             */
            int NumPnts = 20;
            byte[] XCoords = new byte[NumPnts];
            byte[] YCoords = new byte[NumPnts];
            int seed = 6666;
            Random rnd = new Random(seed);
            rnd.NextBytes(XCoords);
            rnd.NextBytes(YCoords);
            double[][] Points = new double[NumPnts][];
            for (int i = 0; i < NumPnts; i++)
            {
                Points[i] = [XCoords[i], YCoords[i]];
            }

            /* Compute the convex hull of the points
             */
            var CvxHl = ConvexPolygon.ConvexHullOf(Points);

            /* Visualize the points and the convex hull
             */
            ImagePlotter plotter = new(fileName);
            // Add all points to the plot
            foreach (var p in Points)
            {
                plotter.AddMarker(new NuMarker<ScottPlot.Color>(p[0], p[1], ScottPlot.Colors.Blue));
            }
            // Plot the convex hull boundaries
            List<VecDbl> loopPnts = new(CvxHl.Vertices)
            {
                CvxHl.Vertices[0]
            };
            for (int i = 0; i <= loopPnts.Count - 2; i++)
            {
                var line = plotter.MyPlot.Add.Line(loopPnts[i][0], loopPnts[i][1], loopPnts[i + 1][0], loopPnts[i + 1][1]);
                line.LineColor = ScottPlot.Colors.Red;
                line.LineWidth = 4;
            }
            plotter.SaveImagePNG();

            Console.WriteLine("TestConvexHull");
        }
    }
}
