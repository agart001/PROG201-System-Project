using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PROG201_System_Project.actors.landscapes
{
    public class Landscape : Actor
    {
        public int MaxWaterLevel { get; set; }
        public double WaterLevel { get; set; }
        public bool WaterDepleted { get; set; }

        public int MaxVegetationLevel { get; set; }
        public int VegetationLevel { get; set; }
        public bool VegetationDepleted { get; set; }

        public override void ParentPreConstruct()
        {
            TypeID = 0;
        }

        #region Water Control
        public void DecrementWaterLevel(double value)
        {
            if (WaterLevel - value <= 0)
            {
                WaterLevel = 0;
            }
            else { WaterLevel -= value; }
        }

        public void IncrementWaterLevel(double value)
        {
            if (WaterLevel + value >= MaxWaterLevel)
            {
                WaterLevel = WaterLevel;
            }
            else { WaterLevel += value; }
        }

        public void CheckWater()
        {
            if (WaterLevel <= 0) WaterDepleted = true;
        }

        public void DepletedWater(Grid grid, Dictionary<Image, Actor> actors)
        {
            if (WaterDepleted)
            {
                DeleteActor(grid, actors, this);
            }
        }
        #endregion

        public virtual void CheckDepletion() { }
        public virtual void Depleted(Grid grid, Dictionary<Image, Actor> actors) 
        {
            DepletedWater(grid, actors);
        }

    }
}
