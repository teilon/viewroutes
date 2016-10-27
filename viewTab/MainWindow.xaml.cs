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
        Enterprise _ent;
        Timer _timer = null;
        List<item> _source;
        Dictionary<string, Label> depotLegends = new Dictionary<string, Label>();

        public MainWindow()
        {   
            InitializeComponent();              

            getTimeLine();
            PrintZones();
            setTimer();
            //Test();             
        }
        
        void setTimer()
        {
            button_time.Click += button_Click;
            _timer = Timer.GetInstance();
            _timer.AddPackList(_source);
            _timer.Step = TimerStep;
        }

        void getTimeLine()
        {
            _source = Source.getData();
            _ent = new Enterprise(@"C:\mok\park.json");
        }

        void Test()
        {                                          
            StringBuilder _sb_load = new StringBuilder();
            StringBuilder _sb_unload = new StringBuilder();
            StringBuilder _sb_park = new StringBuilder();
            StringBuilder _sb_event = new StringBuilder();

            bool new_event = true;
            bool start = true;
            bool end = false;                                                      
                                                                                   
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
                    if (new_event)
                    {
                        _sb_unload.AppendFormat("{0}\n", time2time.GetDateTimeByEcho(item.timestamp));
                        new_event = false;    
                    }
                }
                else if (res == "LL")
                {                                                         
                    addEllipse(item.longitude, item.latitude, item.deviceID, 10, "green");   
                    if (new_event)
                    {
                        _sb_load.AppendFormat("{0}\n", time2time.GetDateTimeByEcho(item.timestamp));
                        new_event = false;             
                    }
                }
                else
                {
                    if (!new_event)      
                        new_event = true;                        
                    if (end)
                        break;
                    addEllipse(item.longitude, item.latitude, item.deviceID);
                }                   
            }
            label_time.Content = string.Format("Count: {0}", Main_Canvas.Children.Count);
            Writer.Do(string.Format("load:\n{0}\nunload:\n{1}\npark:\n{2}\nevents:\n{3}", _sb_load.ToString(), _sb_unload.ToString(), _sb_park.ToString(), _sb_event.ToString()));

        }         
        
        public void TimerStep(item item)
        {
            if(item.deviceID == Source.Park["804"])
            {
                if (Main_Canvas.Children.Contains(currentEllipse))
                    Main_Canvas.Children.Remove(currentEllipse);
                GEOCoordinate coord = new GEOCoordinate(item.latitude, item.longitude);
                coord.TransferToUTM();
                double posTop = (coord.X - 7481.98202458583) / 5;
                double posLeft = (coord.Y - 85202.4156350847) / 5;
                Canvas.SetTop(currentEllipse, posTop + 50);
                Canvas.SetLeft(currentEllipse, posLeft + 50);
                Main_Canvas.Children.Add(currentEllipse);

                addEllipse(item.longitude, item.latitude, item.deviceID);
                DateTime _dt = time2time.GetDateTimeByEcho(item.timestamp);
                string res = _ent.AddMessage(item.deviceID, item.latitude, item.longitude, _dt, item.speedKPH);
                label_time.Content = string.Format("{0}[{1}]\t{2}\t{3}", 
                    item.deviceID, 
                    _dt,
                    res,
                    item.speedKPH
                    //States.FirstOrDefault(x => x.Value == item.statusCode).Key
                    );
                Writer.Do(string.Format("{0}\t{1}\t{2}\n", _dt, res, item.speedKPH));
            }  
            if (item.deviceID == Source.Park["125"] || item.deviceID == Source.Park["70"] || item.deviceID == Source.Park["134"] || item.deviceID == Source.Park["13"] || item.deviceID == Source.Park["157"] || item.deviceID == Source.Park["39"])
            {
                string res = _ent.AddMessage(item.deviceID, item.latitude, item.longitude, time2time.GetDateTimeByEcho(item.timestamp), item.speedKPH);
                addEllipse(item.longitude, item.latitude, item.deviceID, color: "red");
                if (depotLegends.ContainsKey(item.deviceID))
                {
                    //Main_Canvas.Children.Remove(depotLegends[item.deviceID]);
                    GEOCoordinate coord = new GEOCoordinate(item.latitude, item.longitude);
                    coord.TransferToUTM();
                    double posTop = (coord.X - 7481.98202458583) / 5;
                    double posLeft = (coord.Y - 85202.4156350847) / 5;
                    Canvas.SetTop(depotLegends[item.deviceID], posTop + 50);
                    Canvas.SetLeft(depotLegends[item.deviceID], posLeft + 50);
                }
                else
                {
                    depotLegends.Add(item.deviceID, new Label());
                    depotLegends[item.deviceID].Content = Source.Park.FirstOrDefault(x => x.Value == item.deviceID).Key;

                    GEOCoordinate coord = new GEOCoordinate(item.latitude, item.longitude);
                    coord.TransferToUTM();
                    double posTop = (coord.X - 7481.98202458583) / 5;
                    double posLeft = (coord.Y - 85202.4156350847) / 5;
                    Canvas.SetTop(depotLegends[item.deviceID], posTop + 50);
                    Canvas.SetLeft(depotLegends[item.deviceID], posLeft + 50);
                    Main_Canvas.Children.Add(depotLegends[item.deviceID]);
                }                                                                        

            }
            //label_title.Content = string.Format("tracker:\t{0}\ttime:\t{1}", name, time);
        }
                               
        void button_Click(object sender, RoutedEventArgs e)
        {
            _timer?.Start();
        }     
    }
}
