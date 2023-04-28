using PROG201_System_Project.actors.creatures;
using PROG201_System_Project.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PROG201_System_Project.actors.plants
{
    public class Plant : Actor, IFood, IProcreate
    {
        #region Private Variables
        private int calories;
        private bool eaten;
        #endregion

        public Creature Polinator { get; set; }

        public int SunAmount { get; set; }
        public int WaterAmount { get; set; }

        public string FlowerSeason { get; set;}
        public string FruitSeason { get; set; }

        public int MaxFruitAmount { get; set; }
        public int FruitAmount { get; set; }

        public int MaxSeedTravel { get; set; }
        public int MaxSeedAmount { get; set; }
        public int SeedAmount { get; set; }



        public int Calories { get => calories; set => calories = value; }
        public bool Eaten { get => eaten; set => eaten = value; }

        #region IProcreate
        IProcreate.ChromesomeType IProcreate.Chromesome { get; set; }
        IProcreate.BirthType IProcreate.Birth { get; set; }

        private Actor mate; public Actor Mate { get => mate; set => mate = value; }

        private int maxoffspring; public int MaxOffspring { get => maxoffspring; set => maxoffspring = value; }

        private List<Actor> offspring; public List<Actor> Offspring { get => offspring; set => offspring = value; }

        private int maxgestation; public int MaxGestation { get => maxgestation; set => maxgestation = value; }
        private int gestation; public int Gestation { get => gestation; set => gestation = value; }
        private bool gestating; public bool Gestating { get => gestating; set => gestating = value; }
        private bool RToD; public bool ReadyToDeliver { get => RToD; set => RToD = value; }

        private bool RToM; public bool ReadyToMate { get => RToM; set => RToM = value; }

        private int maxhappy; public int MaxHappy { get => maxhappy; set => maxhappy = value; }
        private int happy; public int Happy { get => happy; set => happy = value; }

        private string matingseason; public string MatingSeason { get => matingseason; set => matingseason = value; }
        private Actor birthplace; public Actor BirthPlace { get => birthplace; set => birthplace = value; }
        private int birthrange; public int BirthRange { get => birthrange; set => birthrange = value; }


        public bool InSeason(string season)
        {
            return true;
        }
        public void IncreaseHappy()
        {

        }
        public Actor FindNearestBirthPlace(Grid grid, Dictionary<Image, Actor> actors)
        {

        }
        public Actor FindNearestMate(Grid grid, Dictionary<Image, Actor> actors)
        {
            return new Actor();
        }
        public void GiveBirth(Grid grid, Dictionary<Image, Actor> actors)
        {

        }
        #endregion

        public override void ParentPreConstruct()
        {
            TypeID = 1;
        }
    }
}
