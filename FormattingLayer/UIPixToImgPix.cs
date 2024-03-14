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
    /// <param name="picBoxWidth"></param>
    /// <param name="picBoxHeight"></param>
    /// <param name="imgWidth"></param>
    /// <param name="imgHeight"></param>
    public class UIPixToImgPix(int picBoxWidth, int picBoxHeight, int imgWidth, int imgHeight)
    {
        /* The pixel box width and height are the dimensions of the PictureBox control that displays the image.
         * The image width and height are the dimensions of the image.
         * The UI2Img function maps the pixel coordinates of the UI to the pixel coordinates of the image.
         * The ConvertAll function converts a list of points from the UI to a list of RowColForHash.
         */
        public int PicBoxWidth { get; private set; } = picBoxWidth;
        public int PiBoxHeight { get; private set; } = picBoxHeight;

        public int ImgWidth { get; private set; } = imgWidth;
        public int ImgHeight { get; private set; } = imgHeight;

        public Func<int, int, RowColForHash> UI2Img => (int UIRowIdx, int UIColIdx) =>
        {
            int imgRowIdx = (int)Math.Floor((double)UIRowIdx / (double)PiBoxHeight * ImgHeight);
            int imgColIdx = (int)Math.Floor((double)UIColIdx / (double)PicBoxWidth * ImgWidth);

            return new(imgRowIdx, imgColIdx);
        };

        public List<RowColForHash> ConvertAll(IEnumerable<Point> points)
        {
            return points.Select(p => UI2Img(p.Y, p.X)).ToList();
        }
    }
}
