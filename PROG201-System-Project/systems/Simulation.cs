using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using static PROG201_System_Project.Utility;

namespace PROG201_System_Project.systems
{
    public class Simulation : INotifyPropertyChanged
    {
        public Grid Board { get; set; }

        public Dictionary<Image, Actor> Actors = new Dictionary<Image, Actor>();

        public DispatcherTimer Timer { get; set; }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Date Variables
        private int hour;
        public int Hour { get { return hour; } set {  hour = value; OnPropertyChanged(); } }

        private int day;
        public int Day { get { return day; } set { day = value; OnPropertyChanged(); } }

        private int month;
        public int Month { get { return month; } set { month = value; OnPropertyChanged(); } }

        private int year;
        public int Year { get { return year; } set { year = value; OnPropertyChanged(); } }
        #endregion

        public Simulation(Grid grid, double interval) 
        {
            Board = grid;

            LoadActorsXML(Board, Actors);

            TimeSpan ts = TimeSpan.FromSeconds(interval);
            Timer = new DispatcherTimer();
            Timer.Interval = ts;
            Timer.Tick += Simulation_Tick;

            Start();
        }

        #region Date Increment
        void IncrementHour() => hour++;

        void IncrementDay()
        {
            if(hour > 24)
            {
                hour = 0;
                day++;
            }
        }

        void IncrementMonth()
        {
            if(day > 30)
            {
                day = 0;
                month++;
            }
        }

        void IncrementYear()
        {
            if (month > 12)
            {
                month = 0;
                year++;
            }
        }

        void IncrementTime()
        {
            IncrementHour();
            IncrementDay();
            IncrementMonth();
            IncrementYear();
        }
        #endregion

        public void Start()
        {
            Timer.Start();
        }

        void Simulation_Tick(object sender, EventArgs e)
        {
            IncrementTime();

            foreach(var actor in Actors.Values)
            {
                if (actor.IsCreature())
                {
                    actor.TickAction(Board, Actors);
                }
                else
                {
                    actor.TickAction();
                }
            }
        }

    }
}
