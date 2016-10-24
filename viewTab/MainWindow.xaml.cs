using disp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TimeLine;

namespace viewTab
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /*
        List<Pack> _packList;
        List<Board> _boardList;
        List<Card> _deck;
        */
        public MainWindow()
        {   
            InitializeComponent();
            PrintZones();
            Test();
            //test_intersect();
        }
        void PrintZones()
        {
            DepotPlaces _dp = new DepotPlaces();            
            foreach (Line line in _dp.Depots)
            {
                if(line.Type == "dept")
                    drawPolygin(line, "orange");
                else
                    drawPolygin(line, "brown");
            }

            /*
            foreach (Line line in _dp.Depots)
                foreach (PointD point in line.Points)
                    addEllipse(point.X, point.Y, "dept", 7, "orange");
              
            ParkingPlaces _pp = new ParkingPlaces();
            foreach (Line line in _pp.Parkings)
                foreach (PointD point in line.Points)
                    addEllipse(point.X, point.Y, "park", 7, "brown");
                    */
        }
        void Test()
        {                                                            
            Enterprise _ent = new Enterprise(@"C:\mok\park.json");

            //label_title.Content = "";
            _source = getData();
            StringBuilder _sb_load = new StringBuilder();
            StringBuilder _sb_unload = new StringBuilder();
            StringBuilder _sb_park = new StringBuilder();
            StringBuilder _sb_event = new StringBuilder();

            bool new_event = true;
            bool start = true;
            bool end = false;
            //Polyline pl = null;
            //bool isPolyline = false;

            //addEllipse(62.759122, 52.536982, "0000", 10, "red", pluslabel: true);
            foreach (item item in _source)
            {             
                if (item.longitude == 0 && item.latitude == 0)
                    continue;
                string res = _ent.AddMessage(Source.Park[item.deviceID], item.latitude, item.longitude, time2time.GetDateTimeByEcho(item.timestamp));
                
                if (item.deviceID == "125")
                    addEllipse(item.longitude, item.latitude, item.deviceID, 7, "yellow");
                if (item.deviceID == "71")
                    addEllipse(item.longitude, item.latitude, item.deviceID, 13, "pink");
                if (item.deviceID == "134")
                    addEllipse(item.longitude, item.latitude, item.deviceID, 7, "red");
                if (item.deviceID == "13")
                    addEllipse(item.longitude, item.latitude, item.deviceID, 7, "blue");
                if (item.deviceID == "157")
                    addEllipse(item.longitude, item.latitude, item.deviceID, 7, "pink");
                if (item.deviceID == "39")
                    addEllipse(item.longitude, item.latitude, item.deviceID, 7, "red");
                //else addEllipse(item.longitude, item.latitude, item.deviceID);
                                

                if (item.deviceID != "804")
                    continue;

                //if (item.speedKPH == 0)
                //{
                //    addEllipse(item.longitude, item.latitude, item.deviceID, 10, "red", pluslabel: true, time: string.Format("{0}\n{1}",time2time.GetDateTimeByEcho(item.timestamp), item.timestamp));
                //}
                    

                if (res == "PP")
                {
                    if(item.timestamp > Dates.oct13_day6)
                        start = true;
                    if (item.timestamp > Dates.oct13_night6 - 10000)
                        end = true;
                    if (new_event)
                    {
                        _sb_park.AppendFormat("{0}\n", time2time.GetDateTimeByEcho(item.timestamp));                        
                        new_event = false;
                    }   
                }

                if (!start)
                    continue;

                _sb_event.AppendFormat("{0}[{1}]\t", res, time2time.GetDateTimeByEcho(item.timestamp));

                if (res == "UU")
                {                             
                    addEllipse(item.longitude, item.latitude, item.deviceID, 10, "black");
                    //addPointToPolyline(item.longitude, item.latitude, ref pl);
                    if (new_event)
                    {
                        _sb_unload.AppendFormat("{0}\n", time2time.GetDateTimeByEcho(item.timestamp));
                        new_event = false;
                        //isPolyline = true;
                    }
                }
                else if (res == "LL")
                {                                                         
                    addEllipse(item.longitude, item.latitude, item.deviceID, 10, "green");
                    //addPointToPolyline(item.longitude, item.latitude, ref pl);
                    if (new_event)
                    {
                        _sb_load.AppendFormat("{0}\n", time2time.GetDateTimeByEcho(item.timestamp));
                        new_event = false;
                        //isPolyline = true;
                    }
                }
                else
                {
                    if (!new_event)
                    {
                        /*
                        if (isPolyline)
                        {
                            drawPolyline(pl);
                            isPolyline = false;
                        }
                        */    
                        new_event = true;
                        //pl = new Polyline();
                        //addPointToPolyline(item.longitude, item.latitude, ref pl);
                    }
                    
                    if (end)
                        break;
                    addEllipse(item.longitude, item.latitude, item.deviceID);
                }
                
            }
            label_time.Content = string.Format("Count: {0}", Main_Canvas.Children.Count);
            Writer.Do(string.Format("load:\n{0}\nunload:\n{1}\npark:\n{2}\nevents:\n{3}", _sb_load.ToString(), _sb_unload.ToString(), _sb_park.ToString(), _sb_event.ToString()));

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

        List<item> _source;
        List<item> getData()
        {
            //List<item> _list = Source.GetTestData(Dates.oct13_day6 - 10000, Dates.oct13_night6 + 10000);
            List<item> _list = Source.GetTestData(1476367493, 1476369568);
            _list.Sort(delegate (item x, item y)
            {
                if (x.timestamp == 0 && y.timestamp == 0) return 0;
                else if (x.timestamp == 0) return -1;
                else if (y.timestamp == 0) return 1;
                else return x.timestamp.CompareTo(y.timestamp);
            });
            return _list;
        }
        Ellipse ell;

        void test_intersect()
        {
            addEllipse(62.761, 52.535293, "park", 7, "black");

            
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
        
        double GetLat(double lat)
        {
            //double y = lat / 10;
            double y = lat;

            double y0 = Math.Floor(y);
            double t = (y - y0);
            double res = t * 500;
            return res;
        }

        double GetLon(double lon)
        {
            //double x = lon / 10;
            double x = lon;

            double x0 = Math.Floor(x);
            double t = (x - x0);
            double res = t * 500;
            return res;
        }
        public void NextStep(double lon, double lat, string name, string time)
        {
            addEllipse(lon, lat, name);

            //label_title.Content = string.Format("tracker:\t{0}\ttime:\t{1}", name, time);
        }
                   


        /*
        Canvas GetNewCanvas(string name)
        {
            Canvas _c = new Canvas();
            _c.Visibility = Visibility.Visible;
            _c.Name = name;
            return _c;
        }                                                                   

        void label_Click(object sender, RoutedEventArgs e)
        {
            Board _board = null;
            foreach (Board b in _boardList)
            {
                if ((sender as Button).Name == "b" + b.Imei)
                    _board = b;                                
            }
            if (_board == null)
            {
                MessageBox.Show(string.Format("не найден трекер: {0}", (sender as Button).Name));
                return;
            }
            PrepareTimer(_board.Messages);
            timer?.Start();
        }


        double maxLat = 0;
        double minLat = double.MaxValue;
        double maxLon = 0;
        double minLon = double.MaxValue;
        void setMaxMin(double lat, double lon)
        {
            if (lat > maxLat) maxLat = lat;
            if (lat < minLat) minLat = lat;
            if (lon > maxLon) maxLon = lon;
            if (lon < minLon) minLon = lon;
        }

        void Print(List<Pack> sourceList)
        {       
            foreach (Pack p in sourceList)
            {
                Regex regex = new Regex(@".{4}$");
                string _s = regex.Match(p.msg.Imei).Value;

                setMaxMin(p.msg.Latitude, p.msg.Longitude);
                                
                if (_s == "9877" || _s == "2117" || _s == "6216" || _s == "6108")
                    continue;
                CreateEllipse(p.msg.Latitude, 
                              p.msg.Longitude,
                              "",//p.msg.Imei,
                              5,
                              "Green",
                              !(_s == "0410" || _s == "3956" || _s == "1860" || _s == "6330" || _s == "1648"

                               //_s == "3915" || _s == "2009" || _s == "4980" || _s == "5944" || _s == "3877" || _s == "0966" || _s == "7085" || _s == "3107" || _s == "8337" || _s == "9877" || _s == "2117" || _s == "6216" || _s == "6108"
                               )
                              );
            }
        }


        public void NextStep(Pack pack)
        {                     
            Regex regex = new Regex(@".{4}$");
            string _s = regex.Match(pack.msg.Imei).Value;

            if (_s == "9877" || _s == "2117" || _s == "6216" || _s == "6108")
                return;
            if(_s != "0875")
                return;

            CreateEllipse(pack.msg.Latitude,
                            pack.msg.Longitude,
                            pack.msg.Imei,
                            5,
                            "Red",
                              !(_s == "0410" || _s == "3956" || _s == "1860" || _s == "6330" || _s == "1648"

                               //_s == "3915" || _s == "2009" || _s == "4980" || _s == "5944" || _s == "3877" || _s == "0966" || _s == "7085" || _s == "3107" || _s == "8337" || _s == "9877" || _s == "2117" || _s == "6216" || _s == "6108"
                               )
                            );

            label_imei_content.Content = pack.msg.Imei;
            label_time_content.Content = pack.msg.TimeCreated.ToString();
        }

        Ellipse lastEllipse = new Ellipse() { Name = "lactel", Height = 10, Width = 10, Fill = (Brush)new BrushConverter().ConvertFrom("black") };
        //Ellipse lightEllipse = new Ellipse() { Name = "activia", Height = 15, Width = 15, Fill = (Brush)new BrushConverter().ConvertFrom("orange"), Opacity = 30 };
        void CreateEllipse(double lon, double lat, string imei, double radius = 5, string color = "green", bool isTruck = true)
        {   
            if (Main_Canvas.Children.Contains(lastEllipse))
                Main_Canvas.Children.Remove(lastEllipse);

            double posTop = GetLat(lat);
            double posLeft = GetLon(lon);

            Canvas.SetTop(lastEllipse, posTop - (lastEllipse.Width / 2) + 50);
            Canvas.SetLeft(lastEllipse, posLeft - (lastEllipse.Height / 2) + 50);
            Main_Canvas.Children.Add(lastEllipse);

            if (!isTruck)
            {             
                
                Ellipse lightEllipse = new Ellipse() {
                    Name = "activia",
                    Height = 15,
                    Width = 15,
                    Fill = (Brush)new BrushConverter().ConvertFrom("orange")
                };
                Canvas.SetTop(lightEllipse, posTop - (lightEllipse.Width / 2) + 50);
                Canvas.SetLeft(lightEllipse, posLeft - (lightEllipse.Height / 2) + 50);
                Main_Canvas.Children.Add(lightEllipse);
            }

            double width = radius;
            double height = radius;
            var ell = new Ellipse();
            ell.Height = height;
            ell.Width = width;
            //ell.Fill = (isTruck) ? (Brush)new BrushConverter().ConvertFrom(color) : (Brush)new BrushConverter().ConvertFrom("blue");
            ell.Fill = (Brush)new BrushConverter().ConvertFrom(color);
            ell.Name = "nameEll";

            //double posTop = GetLat(lat);
            //double posLeft = GetLon(lon);

            Canvas.SetTop(ell, posTop - (width / 2) + 50);
            Canvas.SetLeft(ell, posLeft - (height / 2) + 50);

            var label = new Label();
            label.Content = "Ellipse";

            Canvas.SetTop(label, posTop + 5);
            Canvas.SetLeft(label, posLeft + 5);
            Main_Canvas.Children.Add(ell);            
            //if (ellClass != null) ellClass.ell = ell;
        }
        */    
        /*
        void DrawPlaces(DepotPlaces dp)
        {                             
            foreach(Line line in dp.Depots)
                foreach(Point point in line.Points)
                {
                    CreateEllipse(point.Y, point.X, line.Imei, 15, "blue", false);
                }
        }
        void DrawPlaces(ExcavatorPlaces ep)
        {
            foreach (Line line in ep.Excavators)
                foreach (Point point in line.Points)
                {
                    CreateEllipse(point.Y, point.X, line.Imei, 15, "yellow", false);
                }
        }
        void DrawPlaces(ParkingPlaces pp)
        {
            foreach (Line line in pp.Parking)
                foreach (Point point in line.Points)
                {
                    CreateEllipse(point.Y, point.X, line.Imei, 15, "brown", false);
                }
        }


        Timer timer;
        void PrepareTimer(List<Pack> sourceList)
        {            
            timer.AddPackList(sourceList);
            timer.Step = NextStep;
        } 

        private void button_Click_1(object sender, RoutedEventArgs e)
        {               
            timer?.Start();
        }

        private void button_stop_Click(object sender, RoutedEventArgs e)
        {
            timer?.Stop();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {                              
            timer?.Reset();
        }
        */
    }
}
