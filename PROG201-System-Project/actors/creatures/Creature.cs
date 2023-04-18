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
using System.Numerics;
using PROG201_System_Project.actors.plants;

namespace PROG201_System_Project.actors.creatures
{
    internal class Creature : Actor, IMove, IFood
    {

        public enum VoreType
        {
            Herbivore,
            Omnivore,
            Carnivore
        }

        public VoreType Vore { get; set; }


        public bool Alive { get; set; }

        public int MaxMovement { get; set; }

        public int MaxHealth { get; set; }
        public double Health { get; set; }
        public int AttackDamage { get; set; }

        public bool Thirsty { get; set; }
        public int MaxHydration { get; set; }
        public double Hydration { get; set; }
        public double HydrationMR { get; set; }
        public int WaterIntake { get; set; }
        Water NearestWater { get; set; }

        public bool Hungery { get; set; }
        public int MaxHunger { get; set; }
        public double Hunger { get; set; }
        public double HungerMR { get; set; }
        public IFood PreferredFood { get; set; }
        IFood NearestFood { get; set; }


        #region IFood

        private int calories;
        private bool eaten;

        public int Calories { get => calories; set => calories = value; }
        public bool Eaten { get => eaten; set => eaten = value; }
        #endregion

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

        #region Checks

        void CheckThrist()
        {
            if(Hydration < MaxHydration)
            {
                Thirsty = true;
            }
        }
        void CheckHunger()
        {
            if (Hunger < MaxHunger)
            {
                Hungery = true;
            }
        }

        void CheckEaten()
        {
            if (Eaten) Alive = false;
        }

        void CheckAlive(Grid grid)
        {
            if (!Alive)
            {
                grid.Children.Remove(Sprite);
            }
        }

        void CheckStatus()
        {
            CheckEaten();

            CheckThrist();
            CheckHunger();
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
                distances[index] = Math.Abs((int)DistanceToActor(water).Length());
                index++;
            }

            int[] travelabledistances = distances.Where(i => i <= MaxMovement).ToArray();
            int smallestdist = travelabledistances.Min();
            int smallestindex = distances.ToList().FindIndex(i=> i == smallestdist);
            Image smallestsprite = watersprites[smallestindex];
            NearestWater = (Water)actors[smallestsprite];

            MessageBox.Show("Dist: " + smallestdist);

        }

        public void FindNearestFood(Grid grid, Dictionary<Image, Actor> actors)
        {
            GetCurrentPosition();

            //BitmapImage waterbmp = new BitmapImage(new Uri("{pack://application:,,,/images/water.BMP}"));
            Actor FoodActor = PreferredFood as Actor;
            List<Image> images = grid.Children.Cast<Image>().ToList();
            List<Image> foodsprites = images.FindAll(i => ImageFileFromPath(i.Source.ToString()) == ImageFileFromPath(FoodActor.Sprite.Source.ToString()));

            int[] distances = new int[foodsprites.Count];
            int index = 0;
            foreach (Image watersprite in foodsprites)
            {
                IFood food = (IFood)actors[watersprite];
                distances[index] = Math.Abs((int)DistanceToActor(food as Actor).Length());
                index++;
            }

            int[] travelabledistances = distances.Where(i => i <= MaxMovement).ToArray();
            int smallestdist = travelabledistances.Min();
            int smallestindex = distances.ToList().FindIndex(i => i == smallestdist);
            Image smallestsprite = foodsprites[smallestindex];
            NearestFood = (IFood)actors[smallestsprite];

            MessageBox.Show("Dist: " + smallestdist);

        }

        public void MoveToWater(Grid grid, Dictionary<Image, Actor> actors)
        {
            FindNearestWater(grid, actors);

            Vector2 vectowater = DistanceToActor(NearestWater);

            MoveGridActor(grid, Sprite, (int)vectowater.Y, (int)vectowater.X);

            Drink(NearestWater);

            int B = 5;
        }

        public void MoveToFood(Grid grid, Dictionary<Image, Actor> actors)
        {
            FindNearestFood(grid, actors);

            Vector2 vectofood = DistanceToActor(NearestFood as Actor);

            MoveGridActor(grid, Sprite, (int)vectofood.Y, (int)vectofood.X);

            Eat(NearestFood);

            int B = 5;
        }

        public void MoveRandom(Grid grid)
        {
            int rand_y = Rand.Next(-MaxMovement, MaxMovement);
            int rand_x = Rand.Next(-MaxMovement, MaxMovement);

            Vector2 vec = new Vector2(rand_y, rand_x);
            int MaxHypot = GetHypotenuse(MaxMovement, MaxMovement);

            if(vec.Length() == MaxHypot)
            {
                rand_y = Rand.Next(-MaxMovement, MaxMovement);
                rand_x = Rand.Next(-MaxMovement, MaxMovement);
            }


            MoveGridActor(grid, Sprite, rand_y, rand_x);
        }

        #endregion

        #region Eat & Drink
        public void Drink(Water water)
        {
            if (water.WaterDepleted != true)
            {
                if(Hydration < MaxHydration)
                {
                    water.WaterLevel -= WaterIntake;
                    Hydration += WaterIntake;
                }
                water.CheckDepletion();
            }
        }

        public void Eat(IFood food)
        {
            switch (Vore)
            {
                case VoreType.Herbivore:
                    EatVegetation(food);
                    break;
                case VoreType.Carnivore:
                    EatCreature(food);
                    break;
            }
        }

        void EatCreature(IFood food)
        {
            Creature creature = food as Creature;
            if (creature.Health - AttackDamage >= 0)
            {
                creature.Eaten = true;
                Hunger += creature.Calories;
            }
            else
            {
                creature.Health -= AttackDamage;
            }

            CheckEaten();
        }

        void EatVegetation(IFood food)
        {
            Plant plant = food as Plant;
            if(plant.Eaten != true)
            {
                plant.FruitAmount -= 1;
                Hunger += plant.Calories;
            }
        }
        #endregion

        public override void TickAction(Grid grid)
        {
            CheckStatus();
            CheckAlive(grid);

            if (!Alive) return;


        }
    }
}
