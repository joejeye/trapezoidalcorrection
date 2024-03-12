using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ImageDistorsion.NumericLayer.NumericVisualization;

namespace ImageDistorsion.PixelLayer
{
    internal class MarkersToPixels
    {
        /// <summary>
        /// Number of horiztonal pixels
        /// </summary>
        private int _NHrzntlPixs;
        public int NHrzntlPixs { get => _NHrzntlPixs; private set => _NHrzntlPixs = value; }

        /// <summary>
        /// Number of vertical pixels
        /// </summary>
        private int _NVertPixs;
        public int NVertPixs { get => _NVertPixs; private set => _NVertPixs = value; }

        /// <summary>
        /// The min x-coordinate of the markers
        /// </summary>
        private double _Xmin;
        public double Xmin { get => _Xmin; private set => _Xmin = value; }


        /// <summary>
        /// The max x-coordinate of the markers
        /// </summary>
        private double _Xmax;
        public double Xmax { get => _Xmax; private set => _Xmax = value; }


        /// <summary>
        /// The min y-coordinate of the markers
        /// </summary>
        private double _Ymin;
        public double Ymin { get => _Ymin; private set => _Ymin = value; }

        /// <summary>
        /// The max y-coordinate of the markers
        /// </summary>
        private double _Ymax;
        public double Ymax { get => _Ymax; private set => _Ymax = value; }

        /// <summary>
        /// Map each [i, j] pixel index to a set of pixel colors. This is 
        /// because there are chances that multiple markers get mapped to
        /// one pixel.
        /// </summary>
        private Dictionary<RowColIdx, HashSet<Color>> _BinColors;
        public Dictionary<RowColIdx, HashSet<Color>> BinColors
        {
            get => _BinColors;
            private set => _BinColors = value;
        }

        public MarkersToPixels(int NHPix, int NVPix)
        {
            if (NHPix <= 0 || NVPix <= 0)
            {
                throw new ArgumentException("The number of pixels along either axis must be a positive integer");
            }
            NHrzntlPixs = NHPix;
            NVertPixs = NVPix;
            _PixArray2D = new Color[NHPix, NVPix];
            InitPixArray();
            _BinColors = new(new CoordComparer());
            _NMKs = [];
            _Xmin = int.MaxValue;
            _Xmax = int.MinValue;
            _Ymin = int.MaxValue;
            _Ymax = int.MinValue;
            _LoadFinished = false;
        }

        /// <summary>
        /// Set all pixels to transparent
        /// </summary>
        private void InitPixArray()
        {
            for (int colIdx = 0; colIdx < NHrzntlPixs; colIdx++)
            {
                for (int rowIdx = 0; rowIdx < NVertPixs; rowIdx++)
                {
                    PixArray2D[colIdx, rowIdx] = Color.FromArgb(0, 0, 0, 0);
                }
            }
        }
        
        /// <summary>
        /// The list of markers temporarily stored in the memory
        /// </summary>
        private List<NuMarker<Color>> _NMKs;
        public List<NuMarker<Color>> NMKs {  get => _NMKs; private set => _NMKs = value; }

        private bool _LoadFinished;
        public bool LoadFinished { get => _LoadFinished; private set => _LoadFinished = value; }

        /// <summary>
        /// Load one marker into the memory
        /// </summary>
        /// <param name="nmk"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void LoadMarker(NuMarker<Color> nmk)
        {
            if (LoadFinished)
            {
                throw new InvalidOperationException("Cannot load once the load has finished");
            }
            Xmin = Math.Min(Xmin, nmk.x);
            Xmax = Math.Max(Xmax, nmk.x);
            Ymin = Math.Min(Ymin, nmk.y);
            Ymax = Math.Max(Ymax, nmk.y);
            NMKs.Add(nmk);
        }
        
        /// <summary>
        /// Load multiple markers into the memory
        /// </summary>
        /// <param name="nmks"></param>
        public void LoadMarker(NuMarker<Color>[] nmks)
        {
            foreach (var nmk in nmks) { LoadMarker(nmk); }
        }

        /// <summary>
        /// Mark the loading as finished. After this, the instance
        /// cannot load markers anymore.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void FinishLoading()
        {
            if (LoadFinished) 
                throw new InvalidOperationException("The loading is finished already");

            PopulateBinColors();
            PostProcessColors();
            LoadFinished = true;
        }

        private double SpacingX => (Xmax - Xmin) / (NHrzntlPixs - 1);
        private double SpacingY => (Ymax - Ymin) / (NVertPixs - 1);

        private Color[,] _PixArray2D;
        public Color[,] PixArray2D { get => _PixArray2D; private set => _PixArray2D = value; }

        /// <summary>
        /// Find the nearest lattice point to the given point
        /// </summary>
        /// <param name="x">The x-coordinate of the given point</param>
        /// <param name="y">The y-coordinate of the given point</param>
        /// <returns>The row-column indexes representation of the lattice point</returns>
        private RowColIdx NrstLatticPnt(double x, double y)
        {
            var rowIdx = (int) Math.Round((x - Xmin) / SpacingX);
            var colIdx = (int) Math.Round((y - Ymin) / SpacingY);
            return new RowColIdx(rowIdx, colIdx);
        }

        private void PopulateBinColors()
        {
            foreach (var nmk in NMKs)
            {
                RowColIdx rcIdx = NrstLatticPnt(nmk.x, nmk.y);
                if (BinColors.TryGetValue(rcIdx, out var colorSet))
                {
                    colorSet.Add(nmk.color);
                } else
                {
                    var cSet = new HashSet<Color>(new ColorComparer()) { nmk.color };
                    BinColors.Add(rcIdx, cSet);
                }
            }
        }

        private void PostProcessColors()
        {
            for (int colIdx = 0; colIdx < NHrzntlPixs; colIdx++)
            {
                for (int rowIdx = 0; rowIdx < NVertPixs; rowIdx++)
                {
                    RowColIdx rcIdx = new(colIdx, rowIdx);
                    /* Find the average color if multiple markers got
                     * mapped to one pixel
                     */
                    if (BinColors.TryGetValue(rcIdx, out var colorSet))
                    {
                        byte R = 0; byte G = 0; byte B = 0;
                        foreach (var color in colorSet)
                        {
                            R += color.R;
                            G += color.G;
                            B += color.B;
                        }
                        R = (byte)Math.Round((double)R / colorSet.Count);
                        G = (byte)Math.Round((double)G / colorSet.Count);
                        B = (byte)Math.Round((double)B / colorSet.Count);
                        PixArray2D[colIdx, rowIdx] = Color.FromArgb(R, G, B);
                    }
                }
            }  
            // WIP: Fill holes in the image. Holes are where the pixels should have been assigned a color
            // but were not.
        }

        public Color this[int colIdx, int rowIdx]
        {
            get
            {
                if (!LoadFinished)
                {
                    FinishLoading();
                }
                return PixArray2D[colIdx, rowIdx];
            }
        }
    }

    internal readonly struct RowColIdx(int rowIdx, int colIdx)
    {
        public int RowIdx { get; } = rowIdx;
        public int ColIdx { get; } = colIdx;
    }

    internal class CoordComparer : IEqualityComparer<RowColIdx>
    {
        public bool Equals(RowColIdx a, RowColIdx b)
        {
            return a.RowIdx == b.RowIdx && a.ColIdx == b.ColIdx;
        }

        public int GetHashCode(RowColIdx a) => (a.RowIdx.GetHashCode() + 1) ^ (a.ColIdx.GetHashCode() + 2);
    }

    internal class ColorComparer : IEqualityComparer<Color>
    {
        public bool Equals(Color a, Color b)
        { 
            return a.R == b.R && a.G == b.G && a.B == b.B;
        }

        public int GetHashCode(Color a)
        {
            return a.GetHashCode();
        }
    }
}
