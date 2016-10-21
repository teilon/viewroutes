using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using TimeLine;

namespace viewTab
{
    public delegate void Step(Pack pack);
                                   
    public class Timer
    {
        static Timer _timer = null;
        public static Timer GetInstance()
        {
            if (_timer == null)
                _timer = new Timer();     
            return _timer;           
        }

        List<Pack> _packList;
        DispatcherTimer timer;
        Step _step = null;
        public Step Step { set { _step = value; } }
        //
        int _cur;

        Timer()
        {
            _packList = null;
            _cur = 0;
            //
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20);
            //timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timerTick;
        }                    
        public void AddPackList(List<Pack> packList)
        {
            _packList = packList;
        }

        void timerTick(object sender, EventArgs e)
        {
            if (_packList == null)
                return;
            if (_cur >= _packList.Count)
                return;
            Pack p = _packList[_cur];
            if(_step != null)
                _step(p);              
            _cur++;
        }

        public void Start()
        {           
            timer.Start();
        }
        public void Stop()
        {
            timer.Stop();
        }
        public void Reset()
        {
            _cur = 0;
            //Start();
        }
    }
}
