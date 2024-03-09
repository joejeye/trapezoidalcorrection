using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ImageDistorsion.tests
{
    using VecDbl = Vector<double>;

    internal class TestDistort
    {
        public static void Run(string fileName = "test_Distort")
        {
            ChessBoard board = new();
            VecDbl[] ABCD_Prime;
            double[,] abcd = { { 0, 0 }, { 5, 50 }, { 50, 60 }, { 40, -10 } };
            List<VecDbl> ABCD_L = [];
            for (int i = 0; i < abcd.GetLength(0); i++)
            {
                ABCD_L.Add(VecDbl.Build.DenseOfArray(new double[] { abcd[i,0], abcd[i,1] }));
            }
            ABCD_Prime = [.. ABCD_L];

            Distort dstrt = new(board.SizeW, board.SizeH, ABCD_Prime);

            var myPlot = new ScottPlot.Plot();
            for (int i = 0; i < board.SizeH; i++)
            {
                for (int j = 0; j < board.SizeW; j++)
                {
                    var vd = VecDbl.Build.DenseOfArray(new double[] {i, j});
                    vd = dstrt.DistortMapping(vd);
                    var x = vd[0];
                    var y = vd[1];
                    ScottPlot.Color circColor;
                    if (board[i, j])
                    {
                        circColor = ScottPlot.Colors.LightGray;
                    }
                    else
                    {
                        circColor = ScottPlot.Colors.Black;
                    }
                    myPlot.Add.Marker(x, y, color: circColor);
                }
            }

            myPlot.SavePng(GlobalConstants.ProjectRootPath + fileName + ".png", 500, 500);
        }
    }
}
