using ImageDistorsion.PlayGround;
using ImageDistorsion.Tests;

namespace ImageDistorsion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*Test the class ChessboardMarkers*/
            TestChessboardVisualization.Run();

            /*Test class Distort*/
            TestDistort.Run();            
        }
    }
}
