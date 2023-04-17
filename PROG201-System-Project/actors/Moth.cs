using PROG201_System_Project.interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG201_System_Project.actors
{
    class Moth : Actor, ICreature
    {
        #region Private Variables
        private ICreature.VoreType vore;

        private bool alive;
        private int maxhealth;
        private double health;
        private int attackdamage;

        private int maxhydration;
        private double hydration;
        private double hydrationmr;
        private int waterintake;

        private int maxhunger;
        private double hunger;
        private double hungermr;
        private int calorieintake;
        #endregion

        #region Public Variables
        public ICreature.VoreType Vore { get => vore; set => vore = value; }
        public bool Alive { get => alive; set => alive = value; }

        public int MaxHealth { get => maxhealth; set => maxhealth = value; }
        public double Health { get => health; set => health = value; }
        public int AttackDamage { get => attackdamage; set => attackdamage = value; }

        public int MaxHydration { get => maxhydration; set => maxhydration = value; }
        public double Hydration { get => hydration; set => hydration = value; }
        public double HydrationMR { get => hydrationmr; set => hydrationmr = value; }
        public int WaterIntake { get => waterintake; set => waterintake = value; }

        public int MaxHunger { get => maxhunger; set => maxhunger = value; }
        public double Hunger { get => hunger; set => hunger = value; }
        public double HungerMR { get => hungermr; set => hungermr = value; }
        public int CalorieIntake { get => calorieintake; set => calorieintake = value; }
        #endregion

        public override void PreContruct()
        {
            //ImageFile = "default";
        }

        public override void PostContruct()
        {
            Vore = ICreature.VoreType.Carnivore;
            Alive = true;

            MaxHealth = 15;
            Health = 15;
            AttackDamage = 2;

            MaxHydration = 20;
            Hydration = 15;
            HydrationMR = .25;
            WaterIntake = 2;

            MaxHunger = 20;
            Hunger = 15;
            HungerMR = .25;
            CalorieIntake = 2;
        }
    }
}
