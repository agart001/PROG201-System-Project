using PROG201_System_Project.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG201_System_Project.actors.landscapes
{
    internal class Water : Landscape
    {
        public override void PreContruct()
        {
            ImageFile = "water";
        }

        public override void PostContruct()
        {
            WaterLevel = 100;
            WaterDepleted = false;

            VegetationLevel = 0;
            VegetationDepleted = true;
        }
    }
}
