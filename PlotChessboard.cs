using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageDistorsion
{
    internal class PlotChessboard
    {       
        public static void Execute(string fileName = "test_PlotChessboard")
        {
            ChessBoard chsbd = new();
            ScottPlot.Plot myPlot = new();
            int circSize = 10;
            var mkrShape = ScottPlot.MarkerShape.FilledCircle;

            for (int i = 0; i < chsbd.SizeH; i++)
            {
                for (int j = 0; j < chsbd.SizeW; j++)
                {
                    if (chsbd[i, j])
                    {
                        myPlot.Add.Marker(i, j, color: ScottPlot.Colors.LightGray, size: circSize, shape: mkrShape);
                    } else
                    {
                        myPlot.Add.Marker(i, j, color: ScottPlot.Colors.Black, size: circSize, shape: mkrShape);
                    }
                }
            }

            string pathPrefix = @"C:\1Ligo10\BenJeye\learn-n-play\C#\CsharpProjects\ImageDistorsion\";
            myPlot.SavePng(pathPrefix + fileName + ".png", 500, 500);
        }
    }
}
