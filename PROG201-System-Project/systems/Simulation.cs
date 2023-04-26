using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using PROG201_System_Project.actors.creatures;
using PROG201_System_Project.actors.landscapes;
using PROG201_System_Project.actors.plants;
using static PROG201_System_Project.Utility;


namespace PROG201_System_Project.systems
{
    public class Simulation : INotifyPropertyChanged
    {
        public Grid Board { get; set; }

        public Dictionary<Image, Actor> Actors = new Dictionary<Image, Actor>();

        public DispatcherTimer Timer { get; set; }
        public TimeSpan DefaultInterval { get; set; }
        double MinInterval = .2;
        double MaxInterval = 5;

        public Weather Weather { get; set; }

        private string weathertype;
        public string WeatherType { get { return weathertype; } set { weathertype = value; OnPropertyChanged(); } }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Date Variables

        string[] Seasons = new string[] { "Spring", "Summer", "Fall", "Winter"};
        int SeasonIndex = 0;

        private string currentseason;
        public string CurrentSeason { get { return currentseason; } set { currentseason = value; OnPropertyChanged(); } }

        private int hour;
        public int Hour { get { return hour; } set {  hour = value; OnPropertyChanged(); } }

        private int day;
        public int Day { get { return day; } set { day = value; OnPropertyChanged(); } }

        private int month;
        public int Month { get { return month; } set { month = value; OnPropertyChanged(); } }

        private int year;
        public int Year { get { return year; } set { year = value; OnPropertyChanged(); } }
        #endregion

        #region Actor Lists

        public List<Creature> Creatures = new List<Creature>();

        public List<Landscape> Landscapes = new List<Landscape>();

        public List<Plant> Plants = new List<Plant>();

        void GetActorLists()
        {
            foreach(Actor actor in Actors.Values)
            {
                if(actor.IsCreature())
                {
                    Creatures.Add(actor as Creature);
                }

                if(actor.IsLandscape())
                {
                    Landscapes.Add(actor as Landscape);
                }

                if(actor.IsPlant())
                {
                    Plants.Add(actor as Plant);
                }
            }
        }
        #endregion

        public Simulation(Grid grid, double interval) 
        {
            DefaultInterval = TimeSpan.FromSeconds(interval);
            Timer = new DispatcherTimer();
            SetTimerInterval();
            Timer.Tick += Simulation_Tick;

            Board = grid;

            LoadActorsXML(Board, Actors);

            GetActorLists();

            Weather = new Weather(Landscapes, Plants);

            Start();
        }


        #region Date Increment
        void IncrementHour() => Hour++;

        void IncrementDay()
        {
            if(Hour > 24)
            {
                Hour = 0;
                Day++;
            }
        }

        void IncrementMonth()
        {
            if(Day > 30)
            {
                Day = 1;
                Month++;
            }
        }

        void IncrementYear()
        {
            if(Month % 3 == 0)
            {
                if(SeasonIndex + 1 >= Seasons.Length)
                {
                    SeasonIndex = 0;
                }
                else { SeasonIndex++; }
            }

            if (Month > 12)
            {
                Month = 1;
                Year++;
            }
        }

        void IncrementTime()
        {
            IncrementHour();
            IncrementDay();
            IncrementMonth();
            IncrementYear();

            CurrentSeason = Seasons[SeasonIndex];
        }
        #endregion

        #region Interval Control
        void SetTimerInterval()
        {
            Timer.Interval = DefaultInterval;
        }

        public void IncreaseInterval()
        {
            double increase = .25;

            if (DefaultInterval.Seconds + increase >= MaxInterval)
            {
                DefaultInterval = TimeSpan.FromSeconds(MaxInterval);
            }
            else
            {
                DefaultInterval = TimeSpan.FromSeconds(DefaultInterval.Seconds + increase);
            }

            SetTimerInterval();
        }

        public void DecreaseInterval()
        {
            double decrease = .25;

            if(DefaultInterval.Seconds - decrease <= MinInterval)
            {
                DefaultInterval = TimeSpan.FromSeconds(MinInterval);
            }
            else
            {
                DefaultInterval = TimeSpan.FromSeconds(DefaultInterval.Seconds - decrease);
            }

            SetTimerInterval();
        }
        #endregion

        public void Start()
        {
            Weather.TickAction(CurrentSeason);
            WeatherType = Weather.CurrentType.ToString();
            Day = 1;
            Month = 1;
            Year = 1;
            Timer.Start();
        }

        void CreatureTick(Creature creature)
        {
            creature.TickAction(Board, Actors);
        }

        void LandscapeTick(Landscape landscape)
        {
            if (landscape.IsWater())
            {
                landscape.TickAction(Board, Actors);
            }
            else
            {
                landscape.TickAction();
            }
        }

        void PlantTick(Plant plant)
        {
            plant.TickAction();
        }

        void Simulation_Tick(object sender, EventArgs e)
        {
            int pastday = Day;
            IncrementTime();

            if(Day != pastday)
            {
                Weather.TickAction(currentseason);

                WeatherType = Weather.CurrentType.ToString();
            }

            Weather.ApplyEvaporation();
            Weather.ApplyPercipation();

            foreach(var actor in Actors.Values)
            {
                if (actor.IsCreature())
                {
                    CreatureTick(actor as Creature);
                }
                
                if (actor.IsLandscape())
                {
                    LandscapeTick(actor as Landscape);
                }

                if(actor.IsPlant())
                {
                    PlantTick(actor as Plant);
                }
            }
        }

    }
}
