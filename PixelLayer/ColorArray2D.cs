using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.Versioning;

namespace ImageDistorsion.PixelLayer
{
    /// <summary>
    /// The class `ColorArray2D` loads an image in bitmap format into
    /// the memory. It overloads the [] operator such that [i, j] queries
    /// the color information of the pixel at the i-th row and j-th
    /// column. The pixel at [0, 0] is located at the upper left corner
    /// of the image.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class ColorArray2D
    {
        private Bitmap _ImgBmp;

        /// <summary>
        /// The bitmap image
        /// </summary>
        public Bitmap ImgBmp { get => _ImgBmp; private set => _ImgBmp = value; }

        /// <summary>
        /// Number of pixels along the horizontal direction
        /// </summary>
        public int NHorizontalPix => ImgBmp.Width;

        /// <summary>
        /// Number of pixels along the vertical direction
        /// </summary>
        public int NVerticalPix => ImgBmp.Height;

        /// <summary>
        /// Construct the class by loading the image from the file path
        /// </summary>
        /// <param name="filePath"></param>
        public ColorArray2D(string filePath)
        {
            _ImgBmp = new Bitmap(filePath);
        }

        /// <summary>
        /// Construct the class by loading the image from the bitmap
        /// </summary>
        /// <param name="imgBmp"></param>
        public ColorArray2D(Bitmap imgBmp)
        {
            _ImgBmp = new(imgBmp);
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
        /// <param name="VertsOnImg">The variable where the result is to be stored</param>
        /// <returns></returns>
        public void Crop(int[] upperLeftRowCol, int[] lowerRightRowCol, ref List<RowColForHash> VertsOnImg)
        {
            int width = lowerRightRowCol[1] - upperLeftRowCol[1] + 1;
            int height = lowerRightRowCol[0] - upperLeftRowCol[0] + 1;
            if (width > NHorizontalPix || height > NVerticalPix)
            {
                throw new ArgumentException("The cropped region must be smaller than the original image");
            }   
            //ImgBmp = ImgBmp.Clone(new Rectangle(upperLeftRowCol[1], upperLeftRowCol[0], width, height), ImgBmp.PixelFormat);
            // Manually crop the image
            Bitmap newImg = new(width, height);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var thePixel = ImgBmp.GetPixel(j + upperLeftRowCol[1], i + upperLeftRowCol[0]);
                    newImg.SetPixel(j, i, thePixel);
                }
            }
            ImgBmp = newImg;

            // Update the vertices on the image
            for (int i = 0; i < VertsOnImg.Count; i++)
            {
                VertsOnImg[i] = new(VertsOnImg[i].RowIdx - upperLeftRowCol[0], VertsOnImg[i].ColIdx - upperLeftRowCol[1]);
            }
        }
    }
}
