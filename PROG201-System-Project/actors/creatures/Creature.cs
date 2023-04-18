using PROG201_System_Project.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using PROG201_System_Project.actors.landscapes;
using System.Windows.Media.Imaging;
using static PROG201_System_Project.Utility;

namespace PROG201_System_Project.actors.creatures
{
    internal class Creature : Actor, IMove
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
        Water NearestWater { get; set; }

        public int MaxHunger { get; set; }
        public double Hunger { get; set; }
        public double HungerMR { get; set; }
        public int CalorieIntake { get; set; }

        #region IMove
        public void MoveGridActor(Grid grid, Image sprite, int move_y, int move_x)
        {
            int cur_y = Grid.GetRow(sprite);
            int cur_x = Grid.GetColumn(sprite);

            int final_x = cur_x + move_x;
            int final_y = cur_y + move_y;

            ///if (CheckGridCollision(grid, final_x, final_y) != null) return;

            Grid.SetRow(sprite, final_y);
            Grid.SetColumn(sprite, final_x);
        }

        public UIElement CheckGridCollision(Grid grid, int y, int x)
        {
            UIElement child = grid.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == x && Grid.GetColumn(e) == y);

            return child;
        }
        #endregion

        #region Pathing
        public void FindNearestWater(Grid grid, Dictionary<Image, Actor> actors)
        {
            GetCurrentPosition();

            //BitmapImage waterbmp = new BitmapImage(new Uri("{pack://application:,,,/images/water.BMP}"));

            List<Image> images = grid.Children.Cast<Image>().ToList();
            List<Image> watersprites = images.FindAll(i => ImageFileFromPath(i.Source.ToString()) == "water.BMP");

            int[] distances = new int[watersprites.Count];
            int index = 0;
            foreach(Image watersprite in watersprites)
            {
                Water water = (Water)actors[watersprite];
                distances[index] = (int)DistanceToActor(water).Length;
                index++;
            }

            int smallestdist = distances.Min();
            int smallestindex = distances.ToList().FindIndex(i=> i == smallestdist);
            Image smallestsprite = watersprites[smallestindex];
            NearestWater = (Water)actors[smallestsprite];

            MessageBox.Show("Dist: " + smallestdist);

        }

        public void MoveToWater()
        {
           
        }

        #endregion


        #region Eat & Drink
        public void Drink(Landscape enviroment)
        {
            if (enviroment.WaterDepleted != true)
            {
                enviroment.WaterLevel -= WaterIntake;
                enviroment.CheckDepletion();
            }
        }

        public void Eat(Landscape enviroment, Creature creature)
        {
            switch (Vore)
            {
                case VoreType.Herbivore:
                    if (enviroment != null)
                    {
                        EatVegetation(enviroment);
                    }
                    break;
                case VoreType.Carnivore:
                    if (creature != null)
                    {
                        EatCreature(creature);
                    }
                    break;
            }
        }

        void EatCreature(Creature creature)
        {
            if (creature.Health - AttackDamage >= 0)
            {
                creature.Alive = false;
                Hunger += CalorieIntake;
            }
            else
            {
                creature.Health -= AttackDamage;
            }
        }

        void EatVegetation(Landscape enviroment)
        {
            if (enviroment.VegetationDepleted != true)
            {
                enviroment.VegetationLevel -= CalorieIntake;
                enviroment.CheckDepletion();
            }
        }
        #endregion
    }
}
