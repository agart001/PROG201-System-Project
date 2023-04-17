using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG201_System_Project.interfaces
{
    internal interface ICreature
    {
        public enum VoreType
        {
            Herbivore,
            Omnivore,
            Carnivore
        }

        public VoreType Vore { get; set; }


        public bool Alive { get; set; }

        public int MaxHealth { get; set; }
        public double Health { get; set; }
        public int AttackDamage { get; set; }


        public int MaxHydration { get; set; }
        public double Hydration { get; set; }
        public double HydrationMR { get; set; }
        public int WaterIntake { get; set; }


        public int MaxHunger { get; set; }
        public double Hunger { get; set; }
        public double HungerMR { get; set; }
        public int CalorieIntake { get; set; }


        public void Drink(IEnviroment enviroment)
        {
            if (enviroment.WaterDepleted != true)
            {
                enviroment.WaterLevel -= this.WaterIntake;
                enviroment.CheckDepletion();
            }
        }

        public void Eat(IEnviroment enviroment, ICreature creature)
        {
            switch(this.Vore)
            {
                case VoreType.Herbivore:
                    if (enviroment != null)
                    {
                        EatVegetation(enviroment);
                    }
                    break;
                case VoreType.Carnivore:
                    if(creature != null)
                    {
                        EatCreature(creature);
                    }
                    break;
            }
        }

        void EatCreature(ICreature creature)
        {
            if(creature.Health - this.AttackDamage >= 0)
            {
                creature.Alive = false;
                this.Hunger += this.CalorieIntake;
            }
            else
            {
                creature.Health -= this.AttackDamage;
            }
        }

        void EatVegetation(IEnviroment enviroment)
        {
            if (enviroment.VegetationDepleted != true)
            {
                enviroment.VegetationLevel -= this.CalorieIntake;
                enviroment.CheckDepletion();
            }
        }
    }
}
