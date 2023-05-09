using PROG201_System_Project.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROG201_System_Project.actors.creatures;
using static PROG201_System_Project.Utility;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Resources;
using System.Windows;

namespace PROG201_System_Project.actors.plants
{
    public class Tree : Plant
    {
        public override void PreContruct()
        {
            GetRandomID();

            ImageFile = "tree.BMP";
        }

        public override void PostContruct()
        {
            Polinators = new List<Actor>();
            PreferredPolinator = typeof(Bird);

            DailySun = 5;
            SunRecieved = 5;

            DailyWater = 20;
            WaterRecieved = 10;

            Growing = true;

            FlowerSeason = "Fall";
            FruitSeason = "Spring";

            Fruiting = true;

            MaxFruitAmount = 4;
            FruitAmount = 2;

            
            #region IProcreate
            Chromesome = (IProcreate.ChromesomeType)Rand.Next(0, 1);

            MaxOffspring = 5;

            MaxGestation = 3;
            Gestation = 0;
            Gestating = false;
            ReadyToDeliver = false;

            ReadyToMate = false;
            LookForMate = false;

            MaxHappy = 10;
            Happy = 0;

            BirthRange = 5;
            #endregion
            
        }
    }
}
