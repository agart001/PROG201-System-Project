using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG201_System_Project.actors.landscapes
{
    internal class Landscape : Actor
    {
        public int MaxWaterLevel { get; set; }
        public int WaterLevel { get; set; }
        public bool WaterDepleted { get; set; }

        public int MaxVegetationLevel { get; set; }
        public int VegetationLevel { get; set; }
        public bool VegetationDepleted { get; set; }

        public virtual void CheckDepletion()
        {
        }

        public override void ParentPreConstruct()
        {
            TypeID = 0;
        }

        public override void TickAction()
        {
            CheckDepletion();
        }
    }
}
