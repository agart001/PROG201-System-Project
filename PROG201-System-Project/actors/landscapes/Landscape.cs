using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG201_System_Project.actors.landscapes
{
    internal class Landscape : Actor
    {
        public int WaterLevel { get; set; }
        public bool WaterDepleted { get; set; }

        public int VegetationLevel { get; set; }
        public bool VegetationDepleted { get; set; }

        public void CheckDepletion()
        {
            if (WaterLevel >= 0) WaterDepleted = true;
            if (VegetationLevel >= 0) VegetationDepleted = true;
        }

        public override void ParentPreConstruct()
        {
            TypeID = 0;
        }
    }
}
