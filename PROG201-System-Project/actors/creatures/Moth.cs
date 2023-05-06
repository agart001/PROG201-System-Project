using PROG201_System_Project.actors.plants;
using PROG201_System_Project.interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PROG201_System_Project.Utility;

namespace PROG201_System_Project.actors.creatures
{
    public class Moth : Creature
    {
        public override void PreContruct()
        {
            ActorID = 0;
            //ImageFile = "default";
        }

        public override void PostContruct()
        {
            #region Creature
            Vore = VoreType.Herbivore;

            Alive = true;

            MaxMovement = 5;

            MaxHealth = 15;
            Health = 15;
            AttackDamage = 2;

            MaxHydration = 20;
            Hydration = 15;
            HydrationMR = .5;
            WaterIntake = 5;

            MaxHunger = 20;
            Hunger = 20;
            HungerMR = .5;
            PreferredFood = new Yucca();
            #endregion

            #region IFood
            Calories = 5;
            Eaten = false;
            #endregion

            #region IProcreate
            Chromesome = (IProcreate.ChromesomeType)Rand.Next(0, 1);

            MaxOffspring = 5;

            MaxGestation = 5;
            Gestation = 0;
            Gestating = false;
            ReadyToDeliver = false;

            ReadyToMate = false;
            LookForMate = false;

            MaxHappy = 10;
            Happy = 0;

            MatingSeason = "Spring";
            BirthPlace = new Yucca();
            BirthRange = 5;
            #endregion

            //Thirsty = true;
        }


    }
}
