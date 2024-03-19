using ImageDistorsion.PixelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ImageDistorsion.FormattingLayer
{
    /// <summary>
    /// Maps the pixel coordinates of the UI to the pixel coordinates of the image.
    /// </summary>
    /// <param name="picBoxWidth">The width of the pictureBox control in pixels</param>
    /// <param name="picBoxHeight">The height of the pictureBox control in pixels</param>
    /// <param name="imgWidth">The width of th imported image in pixels</param>
    /// <param name="imgHeight">The height of th imported image in pixels</param>
    public class UIPixToImgPix(int picBoxWidth, int picBoxHeight, int imgWidth, int imgHeight)
    {
        /* The pixel box width and height are the dimensions of the PictureBox control that displays the image.
         * The image width and height are the dimensions of the image.
         * The UI2Img function maps the pixel coordinates of the UI to the pixel coordinates of the image.
         * The ConvertAll function converts a list of points from the UI to a list of RowColForHash.
         */

        /// <summary>
        /// The width of the pictureBox control in pixels
        /// </summary>
        public int PicBoxWidth { get; private set; } = picBoxWidth;

        /// <summary>
        /// The height of the pictureBox control in pixels
        /// </summary>
        public int PiBoxHeight { get; private set; } = picBoxHeight;

        /// <summary>
        /// The width of th imported image in pixels
        /// </summary>
        public int ImgWidth { get; private set; } = imgWidth;

        /// <summary>
        /// The height of th imported image in pixels
        /// </summary>
        public int ImgHeight { get; private set; } = imgHeight;

        /// <summary>
        /// The delegate that maps the pixel coordinates of the UI to the pixel coordinates of the image.
        /// </summary>
        public Func<int, int, RowColForHash> UI2Img => (int UIRowIdx, int UIColIdx) =>
        {
            int imgRowIdx = (int)Math.Floor((double)UIRowIdx / (double)PiBoxHeight * ImgHeight);
            int imgColIdx = (int)Math.Floor((double)UIColIdx / (double)PicBoxWidth * ImgWidth);

            return new(imgRowIdx, imgColIdx);
        };

        /// <summary>
        /// Converts a list of positions from the UI to a list of positions in the image.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public List<RowColForHash> ConvertAll(IEnumerable<Point> points)
        {
            return points.Select(p => UI2Img(p.Y, p.X)).ToList();
        }
    }
}
