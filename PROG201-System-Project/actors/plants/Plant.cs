using PROG201_System_Project.actors.creatures;
using PROG201_System_Project.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static PROG201_System_Project.interfaces.IProcreate;
using static PROG201_System_Project.Utility;

namespace PROG201_System_Project.actors.plants
{
    public class Plant : Actor, IFood, IProcreate
    {
        #region Private Variables
        private int calories;
        private bool eaten;
        #endregion

        public List<Actor> Polinators { get; set; }
        public Actor PreferredPolinator { get; set; }

        public int DailySun{ get; set; }
        public int SunRecieved { get; set; }
        public double DailyWater { get; set; }
        public double WaterRecieved { get; set; }

        public bool Growing { get; set; }

        public string FlowerSeason { get; set;}
        public string FruitSeason { get; set; }
        public bool Fruiting { get; set; }

        public int MaxFruitAmount { get; set; }
        public int FruitAmount { get; set; }

        public int Calories { get => calories; set => calories = value; }
        public bool Eaten { get => eaten; set => eaten = value; }

        
        #region IProcreate
        private ChromesomeType chromesome; public ChromesomeType Chromesome { get => chromesome; set => chromesome = value; }
        private int maxoffspring; public int MaxOffspring { get => maxoffspring; set => maxoffspring = value; }

        private List<Actor> offspring; public List<Actor> Offspring { get => offspring; set => offspring = value; }

        private int maxgestation; public int MaxGestation { get => maxgestation; set => maxgestation = value; }
        private int gestation; public int Gestation { get => gestation; set => gestation = value; }
        private bool gestating; public bool Gestating { get => gestating; set => gestating = value; }
        private bool RToD; public bool ReadyToDeliver { get => RToD; set => RToD = value; }

        private bool RToM; public bool ReadyToMate { get => RToM; set => RToM = value; }

        private bool L4M; public bool LookForMate { get => L4M; set => L4M = value; }
        private int maxhappy; public int MaxHappy { get => maxhappy; set => maxhappy = value; }
        private int happy; public int Happy { get => happy; set => happy = value; }

        private string matingseason; public string MatingSeason { get => matingseason; set => matingseason = value; }
        private Type birthplace; public Type BirthPlace { get => birthplace; set => birthplace = value; }
        private int birthrange; public int BirthRange { get => birthrange; set => birthrange = value; }

        public void InSeason(string season)
        {
            if (FlowerSeason == season)
            {
                ReadyToMate = true;
            }
            else
            {
                ReadyToMate = false;
            }

        }

        public void IncreaseHappy()
        {
            if (ReadyToMate)
            {
                Increment(Happy, 1, MaxHappy);
            }
        }

        public void IncreaseGestation()
        {
            if (Gestating)
            {
                Increment(Gestation, 1, MaxGestation);
            }
        }

        public void CheckHappy() => LookForMate = Happy.Equals(MaxHappy);

        public void CheckGestation() => ReadyToDeliver = Gestation.Equals(MaxGestation);

        public Actor FindNearestBirthPlace(Grid grid, Dictionary<int, Actor> actors) => FindNearest(CreateReadOnlyDict(actors, BirthPlace.GetType()));

        public Actor FindNearestMate(Grid grid, Dictionary<int, Actor> actors)
        {
            Type type = GetType();
            switch (Chromesome)
            {
                case ChromesomeType.X: return FindNearest(CreateReadOnlyDict(actors, type, this, a => a.Chromesome == ChromesomeType.Y));
                case ChromesomeType.Y: return FindNearest(CreateReadOnlyDict(actors, type, this, a => a.Chromesome == ChromesomeType.X));
                default: return null;
            }
        }

        public void CreateOffspring(int amount)
        {
            Type t = GetType();
            for (int i = 0; i < amount; i++)
            {
                Offspring.Add(CreateInstance<Actor>(t));
            }
        }

        public void Procreate()
        {
            if (Chromesome == ChromesomeType.X) return;

            CreateOffspring(Rand.Next(1, MaxOffspring));
        }

        public void GiveBirth(Grid grid, Dictionary<int, Actor> actors)
        {
            foreach (Actor offspring in Offspring)
            {
                int dist_x = Rand.Next(-BirthRange, BirthRange);
                int dist_y = Rand.Next(-BirthRange, BirthRange);

                offspring.SpawnGridActor(grid, actors, Grid_X + dist_x, Grid_Y + dist_y);
            }
        }
        #endregion
        

        public void SetSunRecieved (int sunlight)
        {
            SunRecieved = sunlight;
        }

        public void IncreaseWater(double percipation)
        {
            Increment(WaterRecieved, percipation, DailyWater);
        }

        public void DecreaseWater(double evaporation)
        {
            Decrement(WaterRecieved, evaporation, 0);
        }

        void CheckGrow()
        {
            if(SunRecieved >= 5 && WaterRecieved >= DailyWater * .6)
            {
                Growing= true;
                IncreaseHappy();
            }
            else
            {
                Growing = false;
            }
        }

        public void CheckFruiting(string season)
        {
            Fruiting = FruitSeason.Equals(season);
        }

        void GrowNewFruit()
        {
            FruitAmount = Rand.Next(FruitAmount + 1, MaxFruitAmount);
        }

        public void GrowFruit()
        {
            bool[] checks = new bool[] { Fruiting, Growing};

            int chance = Rand.Next(1, 4);

            switch(checks[0], checks[1])
            {
                case (true, true):
                    GrowNewFruit();
                    break; 
                case (true, false):
                    if(chance > 1)
                    {
                        GrowNewFruit();
                    }
                    break;
                case (false, true):
                    if (chance > 2)
                    {
                        GrowNewFruit();
                    }
                    break;
                case (false, false):
                    if (chance > 3)
                    {
                        GrowNewFruit();
                    }
                    break;

            }

        }

        
        void AttachPollen(Grid grid, Dictionary<int, Actor> actors)
        {
            List<Actor> preferred = ValueList(actors).Where(a => ObjectIs(a, PreferredPolinator.GetType())).ToList();

            foreach(Actor actor in preferred)
            {
                if(Math.Abs(DistanceToActor(actor).Length()) <= 3)
                {
                    Polinators.Add(actor);
                }
            }
        }

        void TrackPolinators(Actor nearestmate)
        {
            foreach(Actor polinator in Polinators)
            {
                if(Math.Abs(polinator.DistanceToActor(nearestmate).Length()) <= 3)
                {
                    IProcreate castedmate = (IProcreate)nearestmate;
                    castedmate.Procreate();
                }
            }
        }

        public override void TickAction(Grid grid, Dictionary<int, Actor> actors)
        {
            
            if(ReadyToMate)
            {
                AttachPollen(grid, actors);
            }

            if(LookForMate)
            {
                Actor nearestmate = FindNearestMate(grid, actors);
                TrackPolinators(nearestmate);
            }

            if(ReadyToDeliver)
            {
                if (Chromesome == ChromesomeType.Y)
                {
                    GiveBirth(grid, actors);
                }
            }
            

            CheckGrow();

            GrowFruit();
        }
    }
}
