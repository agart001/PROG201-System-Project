using PROG201_System_Project.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG201_System_Project.actors.plants
{
    public class Yucca : Plant, IFood
    {
        public override void PreContruct()
        {
            ActorID = 0;
            ImageFile = "yucca";
        }

        public override void PostContruct()
        {
            FlowerSeason = "Spring";
            FruitSeason = "Summer";
        }
    }
}
