using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ImageDistorsion.FormattingLayer
{
    public class PixelArrayToBitmap
    {
        public Color[,] PixArr { get; }

        public Bitmap ImgBmp { get; private set; }

        public PixelArrayToBitmap(Color[,] pixArr)
        {
            ArgumentNullException.ThrowIfNull(pixArr);
            PixArr = pixArr;
            Covert();
        }

        private void Covert()
        {
            ImgBmp = new(PixArr.GetLength(0), PixArr.GetLength(1));
            for (int colIdx = 0; colIdx < PixArr.GetLength(0); colIdx++)
            {
                int NRows = PixArr.GetLength(1);
                for (int rowIdx = 0; rowIdx < NRows; rowIdx++)
                {
                    ImgBmp.SetPixel(colIdx, rowIdx, PixArr[colIdx, rowIdx]);
                }
            }
        }

        public void SaveToBmp(string filePath)
        {
            if (!filePath.EndsWith(@".bmp", StringComparison.OrdinalIgnoreCase))
            {
                filePath = filePath + @".bmp";
            }

            ImgBmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Bmp);
            ImgBmp.Dispose();
            Console.WriteLine($"Image saved to {filePath} successfully");
        }
    }
}
