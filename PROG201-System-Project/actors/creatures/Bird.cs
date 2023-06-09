﻿using PROG201_System_Project.actors.plants;
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
    public class Bird : Creature
    {
        public override void PreContruct()
        {
            GetRandomID();

            ImageFile = "bird.BMP";
        }

        public override void PostContruct()
        {
            #region Creature
            Vore = VoreType.Carnivore;

            Alive = true;

            MaxMovement = 6;

            MaxHealth = 15;
            Health = 15;
            AttackDamage = 2;

            MaxHydration = 15;
            Hydration = Rand.Next(MaxHydration / 2, MaxHydration);
            HydrationMR = .15;
            WaterIntake = 5;

            MaxHunger = 25;
            Hunger = Rand.Next(MaxHunger / 2, MaxHunger);
            HungerMR = .25;
            PreferredFood = typeof(Moth);
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

            MatingSeason = "Fall";
            BirthPlace = typeof(Tree);
            BirthRange = 5;
            #endregion

            Hungery = true;
        }


    }
}
