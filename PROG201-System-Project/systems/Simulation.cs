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
            if(name == "LandscapeCounts")
            {
                int b = 5;
            }
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

        Actor NewActor = new Actor();

        public List<Creature> ActiveCreatures = new List<Creature>();
        public List<Landscape> ActiveLandscapes = new List<Landscape>();
        public List<Plant> ActivePlants = new List<Plant>();

        List<Creature> GetActiveCreatures()
        {
            List<Actor> list = Actors.Values.Where(a => a.IsType(typeof(Creature))).ToList();
            List<Creature> converted_list = list.ConvertAll(c => (Creature)c);

            return converted_list;
        }

        List<Landscape> GetActiveLandscapes()
        {
            List<Actor> list = Actors.Values.Where(a => a.IsType(typeof(Landscape))).ToList();
            List<Landscape> converted_list = list.ConvertAll(l => (Landscape)l);

            return converted_list;
        }

        List<Plant> GetActivePlants()
        {
            List<Actor> list = Actors.Values.Where(a => a.IsType(typeof(Plant))).ToList();
            List<Plant> converted_list = list.ConvertAll(p => (Plant)p);

            return converted_list;
        }

        void GetActorLists()
        {
            ActiveCreatures = GetActiveCreatures();
            ActiveLandscapes = GetActiveLandscapes();
            ActivePlants = GetActivePlants();
        }
        #endregion

        #region Actor Types

        List<Actor> ActorTypes = new List<Actor>();

        List<Creature> CreatureTypes = new List<Creature>();
        List<Landscape> LandscapeTypes = new List<Landscape>();
        List<Plant> PlantTypes = new List<Plant>();

        List<Actor> GetActorTypes()
        {
            //Sourec: https://stackoverflow.com/questions/5411694/get-all-inherited-classes-of-an-abstract-class
            List<Actor> actors = new List<Actor>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(Actor)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Actor))))
            {
                actors.Add((Actor)Activator.CreateInstance(type));
            }
            //


            actors = actors.FindAll(a =>
            a.GetType().Name == "Creature" ||
            a.GetType().Name == "Landscape" ||
            a.GetType().Name == "Plant").ToList();


            return actors;
        }
        List<Creature> GetCreatureTypes()
        {
            List<Creature> types = new List<Creature>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(Creature)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Creature))))
            {
                types.Add((Creature)Activator.CreateInstance(type));
            }

            return types;
        }
        List<Landscape> GetLandscapeTypes()
        {
            List<Landscape> types = new List<Landscape>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(Landscape)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Landscape))))
            {
                types.Add((Landscape)Activator.CreateInstance(type));
            }

            return types;
        }
        List<Plant> GetPlantTypes()
        {
            List<Plant> types = new List<Plant>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(Plant)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Plant))))
            {
                types.Add((Plant)Activator.CreateInstance(type));
            }

            return types;
        }

        void GetAllTypes()
        {
            ActorTypes = GetActorTypes();

            CreatureTypes = GetCreatureTypes();
            LandscapeTypes = GetLandscapeTypes();
            PlantTypes = GetPlantTypes();
        }
        #endregion

        #region Actor Counts

        private Dictionary<Creature, int> creaturecounts;
        private Dictionary<Landscape, int> landscapecounts;
        private Dictionary<Plant, int> plantcounts;

        public Dictionary<Creature, int> CreatureCounts { get { return creaturecounts; } set { creaturecounts = value; OnPropertyChanged(); } }
        public Dictionary<Landscape, int> LandscapeCounts { get { return landscapecounts; } set { landscapecounts = value; OnPropertyChanged(); } }
        public Dictionary<Plant, int> PlantCounts { get { return plantcounts; } set { plantcounts= value; OnPropertyChanged(); } }

        void GetCount<T>(T obj, List<T> list, Dictionary<T, int> dict)
        {
            Type type = obj.GetType();
            IList active = CreateList(type);
            active = list.FindAll(i => i.GetType() == obj.GetType()).ToList();
            dict.Add(obj, active.Count);
        }

        Dictionary<T, int> ResetCount<T>(List<T> type_list, List<T> active_list) where T : class
        {
            Type type = typeof(T);
            Dictionary<T, int> dict = CreateDict<T, int>();

            type_list.ForEach(a => GetCount(a, active_list, dict));

            return dict;
            
            //IList list = (IList)Activator.CreateInstance(list_type);
        }

        void InitCounts()
        {
            CreatureCounts = new Dictionary<Creature, int>();
            LandscapeCounts = new Dictionary<Landscape, int>();
            PlantCounts = new Dictionary<Plant, int>();
        }

        void GetCounts()
        {
            CreatureCounts = ResetCount(CreatureTypes, ActiveCreatures);
            LandscapeCounts = ResetCount(LandscapeTypes, ActiveLandscapes);
            PlantCounts = ResetCount(PlantTypes, ActivePlants);
        }
        #endregion

        public Simulation(Grid grid, double interval) 
        {
            GetAllTypes();
            InitCounts();

            DefaultInterval = TimeSpan.FromSeconds(interval);
            Timer = new DispatcherTimer();
            SetTimerInterval();
            Timer.Tick += Simulation_Tick;

            Board = grid;

            LoadActorsXML(Board, Actors);

            GetActorLists();
            GetCounts();

            Weather = new Weather(ActiveLandscapes, ActivePlants);

            Start();
        }

        #region Actor Add
        Actor GetActor(string type, string name)
        {
            Actor actor = new Actor();
            Type actortype = actor.GetType();
            switch (type)
            {
                case "Creature":
                    actortype = CreatureTypes.Find(c => c.GetType().Name == name).GetType();
                    break;
                case "Landscape":
                    actortype = LandscapeTypes.Find(c => c.GetType().Name == name).GetType();
                    break;
                case "Plant":
                    actortype = PlantTypes.Find(c => c.GetType().Name == name).GetType();
                    break;
                default:
                    actor = null;
                    break;
            }

            actor = (Actor)Activator.CreateInstance(actortype);

            return actor;
        }

        public void FindActor(string groupname ,string content)
        {
            Actor type = ActorTypes.Find(a => a.GetType().Name == groupname);

            NewActor = GetActor(type.GetType().Name, content);
        }

        public void AddActor(int y, int x)
        {
            if(NewActor == null) return;

            NewActor.SpawnGridActor(Board, Actors, y, x);

            NewActor = null;
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
            plant.TickAction();
        }

        void CheckProcreators(List<Actor> actors, string season)
        {
            List<IProcreate> procreators = new List<IProcreate>((IEnumerable<IProcreate>)actors.Where(a => ObjectIs<IProcreate>(a)));

            procreators.ForEach(p => p.InSeason(season));
        }

        void CheckGestation(List<Actor> actors)
        {
            List<IProcreate> procreators = new List<IProcreate>((IEnumerable<IProcreate>)actors.Where(a => ObjectIs<IProcreate>(a)));

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

                CheckGestation(ValueList(Actors));
            }

            Weather.ApplyEvaporation();
            Weather.ApplyPercipation();

            foreach(var actor in Actors.Values)
            {
                if (ObjectIs<Creature>(actor))
                {
                    CreatureTick((Creature)actor);
                }
                
                if (ObjectIs<Landscape>(actor))
                {
                    LandscapeTick((Landscape)actor);
                }

                if(ObjectIs<Plant>(actor))
                {
                    PlantTick((Plant)actor);
                }
            }

            GetActorLists();
            GetCounts();

            CheckProcreators(ValueList(Actors), CurrentSeason);

            Weather.SetLandscapes(ActiveLandscapes);
            Weather.SetPlants(ActivePlants);
        }
        #endregion
    }
}
