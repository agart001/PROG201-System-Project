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
    public class Plant : Actor, IFood
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


        public override void ParentPreConstruct()
        {
            TypeID = 1;
        }
    }
}
