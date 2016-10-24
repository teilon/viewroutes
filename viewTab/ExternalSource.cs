using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace viewTab
{   
    public class DepotPlaces
    {
        List<Line> _depots;
        public List<Line> Depots { get { return _depots; } }
        public DepotPlaces()
        {
            List<Line> list = OpenJson(@"c:\mok\depots.json");
            _depots = new List<Line>();
            foreach (Line tm in list)
            {
                Line line = new Line();
                foreach (PointD p in tm.Points)
                {
                    line.Points.Add(new PointD(p.X, p.Y));
                }
                line.Imei = tm.Imei;
                line.Type = tm.Type;
                _depots.Add(line);
            }
        }

        static List<Line> OpenJson(string fileName)
        {
            List<Line> items = null;
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd(); 
                items = JsonConvert.DeserializeObject<List<Line>>(json);
            }
            return items;
        }
    }

    public class ExcavatorPlaces
    {
        List<Line> _excavators;
        public List<Line> Excavators { get { return _excavators; } }
        public ExcavatorPlaces()
        {
            List<Line> list = OpenJson("excv.json");
            _excavators = new List<Line>();
            foreach (Line tm in list)
            {
                Line line = new Line();
                foreach (PointD p in tm.Points)
                {
                    line.Points.Add(new PointD(p.X, p.Y));
                }
                _excavators.Add(line);
            }
        }

        static List<Line> OpenJson(string fileName)
        {
            List<Line> items = null;
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<Line>>(json);
            }
            return items;
        }
    }

    public class ParkingPlaces
    {
        List<Line> _parking;
        public List<Line> Parkings { get { return _parking; } }
        public ParkingPlaces()
        {
            List<Line> list = OpenJson(@"c:\mok\parking.json");
            _parking = new List<Line>();
            foreach (Line tm in list)
            {
                Line line = new Line();
                foreach (PointD p in tm.Points)
                {
                    line.Points.Add(new PointD(p.X, p.Y));
                }
                _parking.Add(line);
            }
        }

        static List<Line> OpenJson(string fileName)
        {
            List<Line> items = null;
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<Line>>(json);
            }
            return items;
        }
    }
}  

public class PointD
{
    public double X;
    public double Y;         

    public PointD(double x, double y)
    {
        X = x;
        Y = y;
    }
} 
 
public class Line
{
    public string Imei;
    public string Type;
    public List<PointD> Points;
    public Line()
    {
        Points = new List<PointD>();
    }
}
