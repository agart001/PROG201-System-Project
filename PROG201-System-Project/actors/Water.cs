using PROG201_System_Project.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG201_System_Project.actors
{
    internal class Water : Actor, IEnviroment
    {
        #region Private Variables
        private int waterlevel;
        private bool waterdepleted;
        private int vegetationlevel;
        private bool vegetationdepleted;
        #endregion


        #region Public Variables
        public int WaterLevel { get => waterlevel; set => waterlevel = value; }
        public bool WaterDepleted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int VegetationLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool VegetationDepleted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion

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
