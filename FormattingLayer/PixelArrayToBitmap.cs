using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ImageDistorsion.FormattingLayer
{
    internal class PixelArrayToBitmap(Color[,] pa)
    {
        public Color[,] PixArr { get; } = pa;

        public void SaveToBmp(string filePath)
        {
            Bitmap img = new(PixArr.GetLength(0), PixArr.GetLength(1));
            for (int colIdx = 0; colIdx < PixArr.GetLength(0); colIdx++)
            {
                int NRows = PixArr.GetLength(1);
                for (int rowIdx = 0; rowIdx < NRows; rowIdx++)
                {
                    img.SetPixel(colIdx, rowIdx, PixArr[colIdx, NRows - 1 - rowIdx]);
                }
            }

            if (!filePath.EndsWith(@".bmp", StringComparison.OrdinalIgnoreCase))
            {
                filePath = filePath + @".bmp";
            }

            img.Save(filePath, System.Drawing.Imaging.ImageFormat.Bmp);
            img.Dispose();
            Console.WriteLine($"Image saved to {filePath} successfully");
        }
    }
}
