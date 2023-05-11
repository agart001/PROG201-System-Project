using PROG201_System_Project.actors.plants;
using PROG201_System_Project.actors.landscapes;
using PROG201_System_Project.interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using static PROG201_System_Project.Utility;

namespace PROG201_System_Project.actors.creatures
{
    public class Rabbit : Creature
    {
        public override void PreContruct()
        {
            GetRandomID();

            ImageFile = "rabbit.BMP";
        }

        public override void PostContruct()
        {
            #region Creature
            Vore = VoreType.Herbivore;

            Alive = true;

            MaxMovement = 4;

            MaxHealth = 15;
            Health = 15;
            AttackDamage = 2;

            MaxHydration = 20;
            Hydration = Rand.Next(MaxHydration / 2, MaxHydration);
            HydrationMR = .45;
            WaterIntake = 5;

            MaxHunger = 25;
            Hunger = Rand.Next(MaxHunger / 2, MaxHunger);
            HungerMR = .45;
            PreferredFood = typeof(Yucca);
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

            MatingSeason = "Summer";
            BirthPlace = typeof(Hole);
            BirthRange = 5;
            #endregion

            //Thirsty = true;
        }


    }
}
