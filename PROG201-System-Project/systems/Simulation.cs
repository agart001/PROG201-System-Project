using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using PROG201_System_Project.actors.creatures;
using PROG201_System_Project.actors.landscapes;
using PROG201_System_Project.actors.plants;
using PROG201_System_Project.interfaces;
using static PROG201_System_Project.Utility;
using static PROG201_System_Project.SimCache;


namespace PROG201_System_Project.systems
{
    public class Simulation : INotifyPropertyChanged
    {
        public Grid Board { get; set; }

        public Dictionary<int, Actor> Actors = new Dictionary<int, Actor>();

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

        Actor ActorAdd = null;
        Actor ActorSub = null;

        public List<Actor> ActiveCreatures = new List<Actor>();
        public List<Actor> ActiveLandscapes = new List<Actor>();
        public List<Actor> ActivePlants = new List<Actor>();

        List<Actor> GetActiveCreatures() => Actors.Values.Where(a => a.IsType(typeof(Creature))).ToList();


        List<Actor> GetActiveLandscapes() => Actors.Values.Where(a => a.IsType(typeof(Landscape))).ToList();

        List<Actor> GetActivePlants() => Actors.Values.Where(a => a.IsType(typeof(Plant))).ToList();

        void GetActorLists()
        {
            ActiveCreatures = GetActiveCreatures();
            ActiveLandscapes = GetActiveLandscapes();
            ActivePlants = GetActivePlants();
        }
        #endregion

        #region Actor Counts

        private Dictionary<Type, int> creaturecounts;
        private Dictionary<Type, int> landscapecounts;
        private Dictionary<Type, int> plantcounts;

        public Dictionary<Type, int> CreatureCounts { get { return creaturecounts; } set { creaturecounts = value; OnPropertyChanged(); } }
        public Dictionary<Type, int> LandscapeCounts { get { return landscapecounts; } set { landscapecounts = value; OnPropertyChanged(); } }
        public Dictionary<Type, int> PlantCounts { get { return plantcounts; } set { plantcounts= value; OnPropertyChanged(); } }

        void GetCount(Type type, List<Actor> list, Dictionary<Type, int> dict)
        {
            IList active = CreateList(type);
            active = list.FindAll(i => i.GetType() == type).ToList();
            dict.Add(type, active.Count);
        }

        Dictionary<Type, int> ResetCount(List<Type> type_list, List<Actor> active_list)
        { 
            Dictionary<Type, int> dict = CreateDict<Type, int>();

            type_list.ForEach(a => GetCount(a, active_list, dict));

            return dict;
            
            //IList list = (IList)Activator.CreateInstance(list_type);
        }

        void InitCounts()
        {
            CreatureCounts = new Dictionary<Type, int>();
            LandscapeCounts = new Dictionary<Type, int>();
            PlantCounts = new Dictionary<Type, int>();
        }

        void GetCounts()
        {
            CreatureCounts = ResetCount(CreatureCache, ActiveCreatures);
            LandscapeCounts = ResetCount(LandscapeCache, ActiveLandscapes);
            PlantCounts = ResetCount(PlantCache, ActivePlants);
        }
        #endregion

        public Simulation(Grid grid, double interval) 
        {
            InitCounts();

            DefaultInterval = TimeSpan.FromSeconds(interval);
            Timer = new DispatcherTimer();
            SetTimerInterval();
            Timer.Tick += Simulation_Tick;

            Board = grid;

            LoadActorsXML(Board, Actors);

            GetActorLists();
            GetCounts();

            Weather = new Weather(ActiveLandscapes.ConvertAll(l => (Landscape)l), ActivePlants.ConvertAll(p => (Plant)p));

            Start();
        }

        #region Actor Add / Sub
        public void FindActorToAdd(string content)
        {
            Type type = ActorCache.Find(a => a.Name == content);

            ActorAdd = CreateInstance<Actor>(type);
        }

        public void AddActor(int y, int x)
        {
            if(ActorAdd == null) return;

            ActorAdd.SpawnGridActor(Board, Actors, y, x);

            ActorAdd = null;
        }

        public void FindActorToSub(string content)
        {
            Type type = ActorCache.Find(a => a.Name == content);

            ActorSub = Actors.Values.First(a => ObjectIs(a, type));
        }

        public void SubActor()
        {
            if (ActorSub == null) return;

            ActorSub.DeleteActor(Board, Actors, ActorSub);

            ActorSub = null;
        }

        #endregion

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

        public void TimerPause()
        {
            Timer.Stop();
        }

        public void TimerPlay()
        {
            Timer.Start();
        }

        public void Start()
        {
            Weather.TickAction(CurrentSeason);
            WeatherType = Weather.CurrentType.ToString();
            Day = 1;
            Month = 1;
            Year = 1;
            Timer.Start();
        }

        #region Tick Actions
        void CreatureTick(Creature creature)
        {
            creature.TickAction(Board, Actors);
        }

        void LandscapeTick(Landscape landscape)
        {
            if (landscape.IsType(typeof(Water)))
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
            plant.TickAction(Board, Actors);
        }

        void CheckProcreators(List<Actor> actors, string season)
        {
            List<IProcreate> procreators = actors.Where(a => ObjectIs<IProcreate>(a)).Cast<IProcreate>().ToList();

            procreators.ForEach(p => p.InSeason(season));
        }

        void CheckGestation(List<Actor> actors)
        {
            List<IProcreate> procreators = actors.Where(a => ObjectIs<IProcreate>(a)).Cast<IProcreate>().ToList();

            procreators.Where(p => p.Gestating).ToList().ForEach(i => i.IncreaseGestation());
        }

        void Simulation_Tick(object sender, EventArgs e)
        {
            int pastday = Day;
            IncrementTime();

            if(Day != pastday)
            {
                Weather.TickAction(currentseason);

                WeatherType = Weather.CurrentType.ToString();

                Weather.ApplySunlight();

                CheckGestation(ValueList(Actors));
                CheckProcreators(ValueList(Actors), CurrentSeason);
            }

            Weather.ApplyEvaporation();
            Weather.ApplyPercipation();

            foreach(var actor in Actors.Values)
            {
                if (actor is Creature)
                {
                    CreatureTick((Creature)actor);
                }
                
                if (actor is Landscape)
                {
                    LandscapeTick((Landscape)actor);
                }

                if(actor is Plant)
                {
                    PlantTick((Plant)actor);
                }
            }

            GetActorLists();
            GetCounts();

            Weather.SetLandscapes(ActiveLandscapes.ConvertAll(l => (Landscape)l));
            Weather.SetPlants(ActivePlants.ConvertAll(p => (Plant)p));
        }
        #endregion
    }
}
