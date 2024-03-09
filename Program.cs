using ImageDistorsion.PlayGround;
using ImageDistorsion.tests;

namespace ImageDistorsion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*Try first simple plot*/
            //Console.WriteLine("Let's plot!");
            //FirstPlot.SimpleScatterPlot();
            //Console.WriteLine("Finished Plotting");

            /*Try create some matrices*/
            //FirstLinearAlgebra.DoSomething();

            /*Try plot a chessboard*/
            //PlotChessboard.Execute();

            /*Try get project path
             Well... it cannot.
             */
            //PrintPath.Run();

            /*Test class Distort*/
            //TestDistort.Run();

            /*Test the class ChessboardMarkers*/
            TestChessboardVisualization.Run();
        }
    }
}
