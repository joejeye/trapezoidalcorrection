using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ImageDistorsion.NumericLayer.NumericVisualization
{
    internal class ImagePlotter
    {
        private double _Xmin, _Xmax, _Ymin, _Ymax;
        public double Xmin { get { return _Xmin; } private set { _Xmin = value; } }
        public double Xmax { get { return _Xmax; } private set { _Xmax = value; } }
        public double Ymin { get { return _Ymin; } private set { _Ymin = value; } }
        public double Ymax { get { return _Ymax; } private set { _Ymax = value; } }
        public double XSpan { get => Xmax - Xmin; }
        public double YSpan { get => Ymax - Ymin; }  

        private ScottPlot.Plot _myPlot;
        public ScottPlot.Plot MyPlot { get => _myPlot; private set => _myPlot = value; }

        public string FileName { get; set; }

        public string SavePath { get; set; }

        public ImagePlotter(string fileName, string savePath = "")
        {
            FileName = fileName;
            if (savePath.Length > 0)
                SavePath = savePath;
            else
                SavePath = GlobalConstants.ProjectTempFilePath;

            _myPlot = new();
            MyPlot.Axes.Margins(0, 0);

            Xmin = double.MaxValue;
            Xmax = double.MinValue;
            Ymin = double.MaxValue;
            Ymax = double.MinValue;
        }

        public void AddMarker(NuMarker<ScottPlot.Color> nmk)
        {
            AddMarker(nmk.x, nmk.y, nmk.color);
        }

        public void AddMarker(double x, double y, ScottPlot.Color color)
        {
            _myPlot.Add.Marker(x, y, color: color);
            Xmin = Math.Min(Xmin, x);
            Xmax = Math.Max(Xmax, x);
            Ymin = Math.Min(Ymin, y);
            Ymax = Math.Max(Ymax, y);
        }

        public void AddMarker(double x, double y, System.Drawing.Color color)
        {
            ScottPlot.Color theColor = new(color.R, color.G, color.B);
            AddMarker(x, y, theColor);
        }

        public void AddMarker(NuMarker<System.Drawing.Color> nmk)
        {
            AddMarker(nmk.x, nmk.y, nmk.color);
        }

        public void SaveImagePNG(int targetX = 500)
        {
            int targetY = (int)Math.Floor(500 / XSpan * YSpan);
            string fileFullName = SavePath + FileName + ".png";
            _myPlot.SavePng(fileFullName, targetX, targetY);
        }
    }
}
