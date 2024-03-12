using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageDistorsion.NumericLayer.NumericVisualization;
using ImageDistorsion.PixelLayer;

namespace ImageDistorsion.Tests
{
    internal class TestPixelsToMarkers
    {
        public static void Run(string imgFilePath = @"C:\1Ligo10\BenJeye\learn-n-play\C#\CsharpProjects\ImageDistorsion\TempFiles\Leetcode50DaysBadge2024.bmp")
        {
            PixelsToMarkers PTM = new(imgFilePath);
            ImagePlotter plotter = new("test_TestPixelsToMarkers");
            foreach (var nmk in PTM)
            {
                plotter.AddMarker(nmk);
            }
            plotter.SaveImagePNG();
            Console.WriteLine("TestPixelsToMarkers finished");
        }
    }
}
