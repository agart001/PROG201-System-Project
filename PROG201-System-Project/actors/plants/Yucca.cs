using PROG201_System_Project.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG201_System_Project.actors.plants
{
    internal class Yucca : Plant, IFood
    {
        public override void PreContruct()
        {
            ImageFile = "yucca";
        }
    }
}
