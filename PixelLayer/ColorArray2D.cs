﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ImageDistorsion.PixelLayer
{
    /// <summary>
    /// The class `ColorArray2D` loads an image in bitmap format into
    /// the memory. It overloads the [] operator such that [i, j] queries
    /// the color information of the pixel at the i-th row and j-th
    /// column. The pixel at [0, 0] is located at the upper left corner
    /// of the image.
    /// </summary>
    public class ColorArray2D
    {
        private Bitmap _ImgBmp;
        public Bitmap ImgBmp { get => _ImgBmp; private set => _ImgBmp = value; }

        /// <summary>
        /// Number of pixels along the horizontal and the vertical directions
        /// </summary>
        public int NHorizontalPix => ImgBmp.Width;
        public int NVerticalPix => ImgBmp.Height;

        public ColorArray2D(string filePath)
        {
            try
            {
                _ImgBmp = new Bitmap(filePath);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (_ImgBmp == null)
            {
                throw new NullReferenceException("The bitmap image must not be empty");
            }
        }

        /// <summary>
        /// Overload the operator [int i, int j] to retrieve the pixel at
        /// i-th row and j-th column. Indexes are 0-based.
        /// </summary>
        /// <param name="i">The index to the row where the pixel under query is located at</param>
        /// <param name="j">The index to the column where the pixel under query is located at</param>
        /// <returns>The Color info</returns>
        public Color this[int i, int j] => ImgBmp.GetPixel(j, i);

        /// <summary>
        /// Crop the image to a smaller region pertaining to the specified
        /// upper-left and lower-right corners.
        /// </summary>
        /// <param name="upperLeftRowCol"></param>
        /// <param name="lowerRightRowCol"></param>
        /// <returns></returns>
        public void Crop(int[] upperLeftRowCol, int[] lowerRightRowCol)
        {
            int width = lowerRightRowCol[1] - upperLeftRowCol[1] + 1;
            int height = lowerRightRowCol[0] - upperLeftRowCol[0] + 1;
            if (width > NHorizontalPix || height > NVerticalPix)
            {
                throw new ArgumentException("The cropped region must be smaller than the original image");
            }   
            //ImgBmp = ImgBmp.Clone(new Rectangle(upperLeftRowCol[1], upperLeftRowCol[0], width, height), ImgBmp.PixelFormat);
            // Manually crop the image
            Bitmap newImg = new Bitmap(width, height);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var thePixel = ImgBmp.GetPixel(j + upperLeftRowCol[1], i + upperLeftRowCol[0]);
                    newImg.SetPixel(j, i, thePixel);
                }
            }
            ImgBmp = newImg;
        }
    }
}
