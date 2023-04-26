using PROG201_System_Project.actors.landscapes;
using PROG201_System_Project.actors.plants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PROG201_System_Project.Utility;

namespace PROG201_System_Project.systems
{
    public class Weather
    {
        public enum WeatherType
        {
            Sunny,
            Gloomy,
            Rainy,
            Brisk,
            Chilly
        }

        public WeatherType CurrentType { get; set; }

        #region Actor Lists
        List<Landscape> Landscapes { get; set; }
        List<Plant> Plants { get; set; }
        public void SetLandscapes(List<Landscape> landscapes) => Landscapes = landscapes;
        public void SetPlants(List<Plant> plants) => Plants = plants;
        #endregion

        int MaxSunlight = 10;
        public double CurrentSunlight { get; set; }

        public double PercipationValue { get; set; }
        public double EvaporationValue { get; set; }

        public double PercipationChance { get; set; }

        public Weather( List<Landscape> landscapes, List<Plant> plants) 
        {
            SetLandscapes(landscapes);
            SetPlants(plants);
        }

        #region Percipation
        double GetPercipationChance(string currentseason)
        {
            switch(currentseason)
            {
                case "Spring":
                    return (Rand.Next(7, 10) / 10.0) * 100.0;
                case "Summer":
                    return (Rand.Next(5, 10) / 10.0) * 100.0;
                case "Fall":
                    return (Rand.Next(6, 10) / 10.0) * 100.0;
                case "Winter":
                    return (Rand.Next(5, 10) / 10.0) * 100.0;
                default: return 0;
            }
        }

        WeatherType SpringPercipation(double chance)
        {
            switch(chance)
            {
                case 70.0: return WeatherType.Sunny;
                case 80.0: return WeatherType.Gloomy;
                case 90.0: return WeatherType.Rainy;
                case 100.0: return WeatherType.Rainy;
                default: return WeatherType.Gloomy;
            }
        }

        WeatherType SummerPercipation(double chance)
        {
            switch (chance)
            {
                case 50.0: return WeatherType.Sunny;
                case 60.0: return WeatherType.Sunny;
                case 70.0: return WeatherType.Sunny;
                case 80.0: return WeatherType.Gloomy;
                case 90.0: return WeatherType.Gloomy;
                case 100.0: return WeatherType.Rainy;
                default: return WeatherType.Sunny;
            }
        }

        WeatherType FallPercipation(double chance)
        {
            switch (chance)
            {
                case 60.0: return WeatherType.Brisk;
                case 70.0: return WeatherType.Brisk;
                case 80.0: return WeatherType.Gloomy;
                case 90.0: return WeatherType.Gloomy;
                case 100.0: return WeatherType.Rainy;
                default: return WeatherType.Brisk;
            }
        }

        WeatherType WinterPercipation(double chance)
        {
            switch (chance)
            {
                case 50.0: return WeatherType.Chilly;
                case 60.0: return WeatherType.Chilly;
                case 70.0: return WeatherType.Chilly;
                case 80.0: return WeatherType.Brisk;
                case 90.0: return WeatherType.Gloomy;
                case 100.0: return WeatherType.Rainy;
                default: return WeatherType.Chilly;
            }
        }

        WeatherType GetType(string currentseason,double chance)
        {
            switch (currentseason)
            {
                case "Spring":
                    return SpringPercipation(chance);
                case "Summer":
                    return SummerPercipation(chance);
                case "Fall":
                    return FallPercipation(chance);
                case "Winter":
                    return WinterPercipation(chance);
                default: return WeatherType.Sunny;
            }
        }

        void GetPerAndEvapValue(WeatherType type)
        {
            switch((int)type)
            {
                case 0: PercipationValue = 0; EvaporationValue = .25; break;
                case 1: PercipationValue = .15; EvaporationValue = .05; break;
                case 2: PercipationValue = .25; EvaporationValue = 0; break;
                case 3: PercipationValue = .1; EvaporationValue = .02; break;
                case 4: PercipationValue = 0; EvaporationValue = 0; break;
                default : return;
            }
        }
        #endregion


        #region Apply Percipation/Evaporation

        public void ApplyEvaporation()
        {
            if (EvaporationValue == 0) return;
            foreach(Landscape landscape in Landscapes)
            {
                landscape.DecrementWaterLevel(EvaporationValue);
            }
        }

        public void ApplyPercipation()
        {
            if (PercipationValue == 0) return;
            foreach (Landscape landscape in Landscapes)
            {
                landscape.IncrementWaterLevel(PercipationValue);
            }
        }
        #endregion

        public void TickAction(string currentseason)
        {
            PercipationChance = GetPercipationChance(currentseason);

            CurrentType = GetType(currentseason, PercipationChance);

            GetPerAndEvapValue(CurrentType);
        }

    }
}
