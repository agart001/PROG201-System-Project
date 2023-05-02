using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using PROG201_System_Project.actors.landscapes;
using static PROG201_System_Project.Utility;
using System.Numerics;
using PROG201_System_Project.actors.plants;
using PROG201_System_Project.interfaces;
using static PROG201_System_Project.interfaces.IProcreate;
using System.Runtime.Serialization;

namespace PROG201_System_Project.actors.creatures
{
    public class Creature : Actor, IMove, IFood, IProcreate
    {
        #region Creature
        public enum VoreType
        {
            Herbivore,
            Omnivore,
            Carnivore
        }

        public VoreType Vore { get; set; }

        public bool Alive { get; set; }

        public Actor Home { get; set; }

        public int MaxMovement { get; set; }

        public int MaxHealth { get; set; }
        public double Health { get; set; }
        public int AttackDamage { get; set; }

        public bool Thirsty { get; set; }
        public int MaxHydration { get; set; }
        public double Hydration { get; set; }
        public double HydrationMR { get; set; }
        public double WaterIntake { get; set; }
        Water NearestWater { get; set; }

        public bool Hungery { get; set; }
        public int MaxHunger { get; set; }
        public double Hunger { get; set; }
        public double HungerMR { get; set; }
        public IFood PreferredFood { get; set; }
        IFood NearestFood { get; set; }


        public override void ParentPreConstruct()
        {
            TypeID = 2;
        }
        #endregion

        #region IProcreate
        private ChromesomeType chromesome; public ChromesomeType Chromesome { get => chromesome; set => chromesome = value; }
        private BirthType birth; public BirthType Birth { get => birth; set => birth = value; }

        private Actor nearestmate; public Actor NearestMate { get => nearestmate; set => nearestmate = value; }

        private int maxoffspring; public int MaxOffspring { get => maxoffspring; set => maxoffspring = value; }

        private List<Actor> offspring; public List<Actor> Offspring { get => offspring; set => offspring = value; }

        private int maxgestation; public int MaxGestation { get => maxgestation; set => maxgestation = value; }
        private int gestation; public int Gestation { get => gestation; set => gestation = value; }
        private bool gestating; public bool Gestating { get => gestating; set => gestating = value; }
        private bool RToD; public bool ReadyToDeliver { get => RToD; set => RToD = value; }

        private bool RToM; public bool ReadyToMate { get => RToM; set => RToM = value; }

        private bool L4M; public bool LookForMate { get => L4M; set => L4M = value; }
        private int maxhappy; public int MaxHappy { get => maxhappy; set => maxhappy = value; }
        private int happy; public int Happy { get => happy; set => happy = value; }

        private string matingseason; public string MatingSeason { get => matingseason; set => matingseason = value; }
        private Actor birthplace; public Actor BirthPlace { get => birthplace; set => birthplace = value; }
        private int birthrange; public int BirthRange { get => birthrange; set => birthrange = value; }

        public void InSeason(string season)
        {
            if(MatingSeason == season)
            {
                ReadyToMate = true;
            }

        }
        public void IncreaseHappy()
        {
            if (ReadyToMate)
            {
                if(Happy++ >= MaxHappy)
                {
                    Happy = MaxHappy;
                }
                else { Happy++; }
            }
        }

        public void CheckHappy()
        {
            if(Happy == MaxHappy) LookForMate = true;
        }

        public Actor FindNearestBirthPlace(Grid grid, Dictionary<Image, Actor> actors) => FindNearest(BirthPlace, actors);

        public Actor FindNearestMate(Grid grid, Dictionary<Image, Actor> actors)
        {
            Type type = GetType();
            switch(Chromesome)
            {
                case ChromesomeType.X: return FindNearest(this, CreateReadOnlyDict(actors, type, this, a => a.Chromesome == ChromesomeType.X));
                case ChromesomeType.Y: return FindNearest(this, CreateReadOnlyDict(actors, type, this, a => a.Chromesome == ChromesomeType.Y));
                default: return null;
            }
        }

        public void Procreate(Actor NearestMate)
        {
            
        }

        public void GiveBirth(Grid grid, Dictionary<Image, Actor> actors)
        {
            int b = 5;
        }
        #endregion

        #region IFood

        private int calories;
        private bool eaten;

        public int Calories { get => calories; set => calories = value; }
        public bool Eaten { get => eaten; set => eaten = value; }
        #endregion

        #region IMove
        public void Move(Grid grid, Image sprite, int move_y, int move_x)
        {
            //40
            int total_x = grid.ColumnDefinitions.Count;
            //20
            int total_y = grid.RowDefinitions.Count;

            int cur_y = Grid_Y;
            int cur_x = Grid_X;

            //int final_x = cur_x + move_x;
            //int final_y = cur_y + move_y;

            Vector2 vec = new Vector2(move_x, move_y);
            Vector2 maxvec = new Vector2(MaxMovement, MaxMovement);

            int len = Math.Abs((int)vec.Length());
            int max_len = Math.Abs((int)maxvec.Length());

            if (len > max_len)
            {
                int mag_x = (int)(vec.X / Math.Abs(vec.X));
                int mag_y = (int)(vec.Y / Math.Abs(vec.Y));

                vec.X = cur_x + (MaxMovement * mag_x);
                vec.Y = cur_y + (MaxMovement * mag_y);
            }
            else
            {
                vec.X = cur_x + move_x;
                vec.Y = cur_y + move_y;
            }

                ///if (CheckGridCollision(grid, final_x, final_y) != null) return;

            if (vec.X < 0) vec.X = 0;
            if (vec.Y < 0) vec.Y = 0;

            if (vec.X > total_x) vec.X = total_x;
            if (vec.Y > total_y) vec.Y = total_y;

            Grid.SetRow(sprite, (int)vec.Y);
            Grid.SetColumn(sprite, (int)vec.X);
        }

        public void MoveToActor(Grid grid, Actor actor)
        {
            Vector2 vec = DistanceToActor(actor);

            Move(grid, Sprite, (int)vec.Y, (int)vec.X);
        }

        public UIElement CheckGridCollision(Grid grid, int y, int x)
        {
            UIElement child = grid.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == x && Grid.GetColumn(e) == y);

            return child;
        }

        public void MoveRandom(Grid grid)
        {
            int rand_y = Rand.Next(-MaxMovement, MaxMovement);
            int rand_x = Rand.Next(-MaxMovement, MaxMovement);

            Vector2 vec = new Vector2(rand_y, rand_x);

            Move(grid, Sprite, (int)vec.Y, (int)vec.X);
        }

        public void MoveThenExcute<T>(T obj, Action method, Grid grid)
        {
            if (obj != null)
            {
                Vector2 vec = DistanceToActor(obj as Actor);
                int dist = (int)vec.Length();
                if (dist > 0)
                {
                    MoveToActor(grid, obj as Actor);
                }
                else
                {
                    method();
                }
            }
        }

        #endregion

        #region Checks
        void ApplyMR()
        {
            Hydration -= HydrationMR;
            Hunger -= HungerMR;
        }

        void CheckThrist()
        {
            if(Hydration < MaxHydration * .6)
            {
                Thirsty = true;
            }
            else
            {
                Thirsty = false;
            }
        }
        void CheckHunger()
        {
            if (Hunger < MaxHunger * .6)
            {
                Hungery = true;
            }
            else
            {
                Hungery = false;
            }
        }

        void CheckEaten()
        {
            if (Eaten) Alive = false;
        }

        void CheckAlive(Grid grid, Dictionary<Image, Actor> actors)
        {
            if (!Alive)
            {
                DeleteActor(grid, actors, this);
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
        public Water FindNearestWater(Grid grid, Dictionary<Image, Actor> actors) => FindNearest(new Water(), CreateReadOnlyDict(actors, typeof(Water)));

        public IFood FindNearestFood(Grid grid, Dictionary<Image, Actor> actors) => FindNearest(PreferredFood, CreateReadOnlyDict(actors, PreferredFood.GetType()));

        #endregion

        #region Eat & Drink
        

        public void Drink(Water water)
        {
            if (water.WaterDepleted != true)
            {
                if(Hydration < MaxHydration)
                {
                    water.DecrementWaterLevel(WaterIntake);
                    Increment(Hydration, WaterIntake, MaxHydration);

                    IncreaseHappy();
                }
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
            
            IncreaseHappy();
        }

        void EatCreature(IFood food)
        {
            Creature creature = food as Creature;
            if (creature.Health - AttackDamage >= 0)
            {
                creature.Eaten = true;
                Increment(Hunger, creature.Calories, MaxHunger);
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
                if(plant.FruitAmount > 0)
                {
                    plant.FruitAmount--;
                    Increment(Hunger, plant.Calories, MaxHunger);
                }
            }
        }
        #endregion

        public override void TickAction(Grid grid, Dictionary<Image, Actor> actors)
        {
            GetCurrentPosition();

            ApplyMR();

            CheckStatus();
            CheckAlive(grid, actors);

            if (!Alive) return;

            if(!Thirsty && !Hungery)
            {
                MoveRandom(grid);
            }

            if(Thirsty)
            {
                NearestWater = FindNearestWater(grid, actors);
                MoveThenExcute(NearestWater, () => Drink(NearestWater), grid);
            }

            if (Hungery)
            {
                NearestFood = FindNearestFood(grid, actors);
                MoveThenExcute(NearestFood, () => Eat(NearestFood), grid);
            }

            if (LookForMate)
            {
                NearestMate = FindNearestMate(grid, actors);
                MoveThenExcute(NearestMate, () => Procreate(NearestMate), grid);
            }
        }
    }
}
