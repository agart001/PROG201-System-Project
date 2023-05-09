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
    public class Yucca : Plant
    {
        public override void PreContruct()
        {
            GetRandomID();

            ImageFile = "yucca.BMP";
        }

        public override void PostContruct()
        {
            Polinators = new List<Actor>();
            PreferredPolinator = new Moth();

            DailySun = 5;
            SunRecieved = 5;

            DailyWater = 20;
            WaterRecieved = 10;

            Growing = true;

            FlowerSeason = "Spring";
            FruitSeason = "Summer";

            Fruiting = true;

            MaxFruitAmount = 10;
            FruitAmount = 5;

            /*
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
            */
        }
    }
}
