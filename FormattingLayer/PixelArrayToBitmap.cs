using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.Versioning;

namespace ImageDistorsion.FormattingLayer
{
    /// <summary>
    /// Convert a 2D pixel array to a bmp file
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class PixelArrayToBitmap
    {
        /// <summary>
        /// The 2D pixel array to be converted to the bmp file
        /// </summary>
        public Color[,] PixArr { get; }

        /// <summary>
        /// The bitmap image created from the pixel array
        /// </summary>
        public Bitmap? ImgBmp { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pixArr">The 2D pixel array to be converted to the bmp file</param>
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

        /// <summary>
        /// Save the image to a bmp file with the given file path
        /// </summary>
        /// <param name="filePath"></param>
        public void SaveToBmp(string filePath)
        {
            if (!filePath.EndsWith(@".bmp", StringComparison.OrdinalIgnoreCase))
            {
                filePath = filePath + @".bmp";
            }

            ArgumentNullException.ThrowIfNull(ImgBmp);
            ImgBmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Bmp);
            ImgBmp.Dispose();
            Console.WriteLine($"Image saved to {filePath} successfully");
        }
    }
}
