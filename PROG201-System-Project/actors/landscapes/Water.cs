using PROG201_System_Project.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PROG201_System_Project.actors.landscapes
{
    public class Water : Landscape
    {
        public override void PreContruct()
        {
            ActorID = 0;
            ImageFile = "water.BMP";
        }

        public override void PostContruct()
        {
            MaxWaterLevel = 50;
            WaterLevel = 50;
            WaterDepleted = false;

            MaxVegetationLevel = 0;
            VegetationLevel = 0;
            VegetationDepleted = true;
        }

        public override void SpriteOpacity()
        {
            double opacity;
            if (WaterLevel / MaxWaterLevel < .25)
            {
                opacity = .25;
            }
            else
            {
                opacity = WaterLevel / MaxWaterLevel;
            }

            Sprite.Opacity = opacity;
        }

        public override void CheckDepletion()
        {
            CheckWater();
        }

        public override void TickAction(Grid grid, Dictionary<Image, Actor> actors)
        {
            SpriteOpacity();

            CheckDepletion();

            Depleted(grid, actors);
        }
    }
}
