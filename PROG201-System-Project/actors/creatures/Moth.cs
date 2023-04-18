using PROG201_System_Project.actors.plants;
using PROG201_System_Project.interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG201_System_Project.actors.creatures
{
    class Moth : Creature
    {
        public override void PreContruct()
        {
            //ImageFile = "default";
        }

        public override void PostContruct()
        {
            Vore = VoreType.Herbivore;

            Alive = true;

            MaxMovement = 5;

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
            PreferredFood = new Yucca();

            Calories = 5;
            Eaten = false;
        }


    }
}
