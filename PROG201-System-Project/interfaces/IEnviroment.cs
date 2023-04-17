using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG201_System_Project.interfaces
{
    internal interface IEnviroment
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
    }
}
