using PROG201_System_Project.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PROG201_System_Project.actors.landscapes
{
    internal class Water : Landscape
    {
        public override void PreContruct()
        {
            ActorID = 0;
            ImageFile = "water";
        }

        public override void PostContruct()
        {
            MaxWaterLevel = 100;
            WaterLevel = 100;
            WaterDepleted = false;

            MaxVegetationLevel = 0;
            VegetationLevel = 0;
            VegetationDepleted = true;
        }

        public override void CheckDepletion()
        {
            if(WaterLevel <= 0)
            {
                WaterDepleted = true;
            }

            double opacity;
            if(WaterLevel / MaxWaterLevel < .25)
            {
                opacity = .25;
            }
            else
            {
                opacity = WaterLevel / MaxWaterLevel;
            }

            Sprite.Opacity = opacity;
        }
    }
}
