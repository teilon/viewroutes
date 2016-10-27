using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace viewTab
{
    public partial class MainWindow : Window
    {
        void PrintZones()
        {
            DepotPlaces _dp = new DepotPlaces();
            foreach (Line line in _dp.Depots)
            {
                if (line.Type == "dept")
                    drawPolygin(line, "orange");
                else
                    drawPolygin(line, "brown");
            }
        }

        void drawPolyline(Polyline pl)
        {
            Main_Canvas.Children.Add(pl);
        }

        void addPointToPolyline(double x, double y, ref Polyline pl)
        {
            double _x = x;
            double _y = y;
            GEOCoordinate coord = new GEOCoordinate(_y, _x);
            coord.TransferToUTM();

            _x = (coord.X - 7481.98202458583) / 5;
            _y = (coord.Y - 85202.4156350847) / 5;

            pl.Points.Add(new System.Windows.Point(_y + 50, _x + 50));
            //poly.Points.Add(new System.Windows.Point(_y + 50, _x + 50));

        }

        void drawPolygin(Line line, string color)
        {
            Polygon poly = new Polygon();
            poly.Stroke = (Brush)new BrushConverter().ConvertFrom(color);
            poly.StrokeThickness = 0.5;
            double _x = 0;
            double _y = 0;
            foreach (PointD point in line.Points)
            {
                _x = point.X;
                _y = point.Y;
                GEOCoordinate coord = new GEOCoordinate(_y, _x);
                coord.TransferToUTM();

                _x = (coord.X - 7481.98202458583) / 5;
                _y = (coord.Y - 85202.4156350847) / 5;

                poly.Points.Add(new System.Windows.Point(_y + 50, _x + 50));
            }
            //poly.Fill = (Brush)new BrushConverter().ConvertFrom("gray");

            Main_Canvas.Children.Add(poly);


            Label polyLabel = new Label();
            polyLabel.Content = line.Imei;
            Canvas.SetTop(polyLabel, _x + 50);
            Canvas.SetLeft(polyLabel, _y + 50);
            Main_Canvas.Children.Add(polyLabel);
        }

        double xmax = double.MinValue;
        double xmin = double.MaxValue;
        double ymax = double.MinValue;
        double ymin = double.MaxValue;
        void addEllipse(double lon, double lat, string imei, double radius = 2, string color = "green", bool pluslabel = false, string time = "")
        {
            GEOCoordinate coord = new GEOCoordinate(lat, lon);
            coord.TransferToUTM();

            if (coord.X > xmax) xmax = coord.X;
            if (coord.X < xmin) xmin = coord.X;
            if (coord.Y > ymax) ymax = coord.Y;
            if (coord.Y < ymin) ymin = coord.Y;

            //double posTop = GetLat(coord.X);  
            //double posLeft = GetLon(coord.Y);
            double posTop = (coord.X - 7481.98202458583) / 5;
            double posLeft = (coord.Y - 85202.4156350847) / 5;

            double width = radius;
            double height = radius;
            //var ell = new Ellipse();
            ell = new Ellipse();
            ell.Height = height;
            ell.Width = width;
            ell.Fill = (Brush)new BrushConverter().ConvertFrom(color);
            ell.Name = "nameEll";

            Canvas.SetTop(ell, posTop - (width / 2) + 50);
            Canvas.SetLeft(ell, posLeft - (height / 2) + 50);

            Main_Canvas.Children.Add(ell);

            //

            if (pluslabel)
            {
                Label label = new Label();
                label.Content = time;
                Canvas.SetTop(label, posTop - (width / 2) + 50);
                Canvas.SetLeft(label, posLeft - (height / 2) + 50);

                Main_Canvas.Children.Add(label);
            }
        }

        Ellipse ell;

        Ellipse currentEllipse = new Ellipse()
        {
            Height = 5,
            Width = 5,
            Fill = (Brush)new BrushConverter().ConvertFrom("Black"),
            Name = "CurrentEllipse"
        };
    }
}
