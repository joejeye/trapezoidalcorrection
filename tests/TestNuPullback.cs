using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageDistorsion.NumericLayer;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;
using ImageDistorsion.NumericLayer.NumericVisualization;

namespace ImageDistorsion.Tests
{
    using VecDbl = Vector<double>;

    public class TestNuPullback
    {
        public const string subfolder = @"C:\1Ligo10\BenJeye\learn-n-play\C#\CsharpProjects\ImageDistorsion\TempFiles\TestNuPullback";

        public static void Run()
        {
            /* The corrected domain */
            RectPolygon correctedDomain = new(new double[] { 0, 0 }, [2, 1]);

            /* Create the quadrilateral, i.e., the distorted domain */
            double[] A = [0, 5];
            double[] B = [5, 0];
            double[] C = [20, 5];
            double[] D = [5, 10];
            ConvexPolygon quadrilateral = new([A, B, C, D]);

            /* Create the visualization mapping */
            double[] center = [5, 5]; // The center of the circle
            Func<double, double, Color> visMap = (double x, double y) =>
            {                 
                if (!quadrilateral.ContainsPoint(VecDbl.Build.DenseOfArray([x, y]), out bool _))
                {
                    return Color.Black;
                } else
                {
                    if (Math.Pow(x - center[0], 2) + Math.Pow(y - center[1], 2) <= 10)
                    {
                        if (y <= 5)
                        {
                            return Color.Green;
                        } else
                        {
                            return Color.Blue;
                        }
                    } else
                    {
                        return Color.White;
                    }
                }
            };

            /* Create the pullback correction */
            ImageDistorsion.NumericLayer.PullbackCorrection nuPullBack = new(quadrilateral, correctedDomain, (double x, double y) => visMap(x, y));

            /* Plot the distorted domain */
            string filePath = Path.Combine(subfolder, "distortedDomain.png");
            ImagePlotter plotter = new();
            double spacingX = 0.1;
            double spacingY = 0.1;
            double x = 0, y = 0;
            while (x <= 20)
            {
                y = 0;
                while (y <= 10)
                {
                    plotter.AddMarker(x, y, visMap(x, y));
                    y += spacingY;
                }
                x += spacingX;
            }
            plotter.SaveImagePNG(saveFileFullPath: filePath);

            /* Plot the corrected domain */
            filePath = Path.Combine(subfolder, "correctedDomain.png");
            plotter = new();
            spacingX = 0.04;
            spacingY = 0.02;
            x = 0;
            y = 0;
            while (x <= correctedDomain.Xmax)
            {
                y = 0;
                while (y <= correctedDomain.Ymax)
                {
                    plotter.AddMarker(x, y, nuPullBack.CorrectedVisualization(x, y));
                    y += spacingY;
                }
                x += spacingX;
            }
            plotter.SaveImagePNG(saveFileFullPath: filePath);

            Console.WriteLine("TestNuPullback completed");
        }
    }
}
