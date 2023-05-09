using PROG201_System_Project.actors.creatures;
using PROG201_System_Project.actors.landscapes;
using PROG201_System_Project.actors.plants;
using PROG201_System_Project.interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using static PROG201_System_Project.Utility;
using static PROG201_System_Project.SimCache;

namespace PROG201_System_Project
{
    public class Actor
    {
        private Image? sprite; public Image? Sprite { get => sprite; set => sprite = value; }

        private string? imgfile; public string? ImageFile { get => imgfile; set => imgfile = value; }
        private int actorid; public int ActorID { get => actorid; set => actorid = value; }

        private int y; public int Grid_Y { get => y; set => y = value; }
        private int x; public int Grid_X { get => x; set => x = value; }


        public virtual void ParentPreConstruct()
        {

        }

        public virtual void PreContruct()
        {

        }

        public virtual void PostContruct()
        {

        }

        public Actor()
        {
            ParentPreConstruct();

            PreContruct();

            Type t = GetType();

            if (ImageFile != null)
            {
                Image clone = new Image();
                clone.Source = ImageCache[ImageFile].Source;

                Sprite = clone;
            }
            else { Sprite = null; }

            PostContruct();
        }


        public virtual void TickAction()
        {

        }
        public virtual void TickAction(Grid grid)
        {

        }
        public virtual void TickAction(Grid grid, Dictionary<int, Actor> actors)
        {

        }


        public virtual void SpriteOpacity() { }

        public Actor Clone() => (Actor)MemberwiseClone();

        public void GetRandomID()
        {
            ActorID = Rand.Next(1,1000000);
        }

        public void DeleteActor(Grid grid, Dictionary<int, Actor> actors, Actor actor)
        {
            grid.Children.Remove(Sprite);
            actors.Remove(ActorID, out actor);
        }

        #region Position
        public void GetCurrentPosition()
        {
            Grid_Y = Grid.GetRow(this.Sprite);
            Grid_X = Grid.GetColumn(this.Sprite);
        }

        public void SpawnGridActor(Grid grid, Dictionary<int, Actor> actors, int spawn_y, int spawn_x)
        {
            Grid.SetRow(Sprite, spawn_y);
            Grid.SetColumn(Sprite, spawn_x);

            actors.Add(ActorID, this);
            grid.Children.Add(Sprite);

            GetCurrentPosition();
        }

        public Vector2 DistanceToActor(Actor actor)
        {
            Vector2 vector = new Vector2();
            vector.X = actor.Grid_X - this.Grid_X;
            vector.Y = actor.Grid_Y - this.Grid_Y;
            return vector;
        }

        public Actor FindNearest(IReadOnlyDictionary<int, Actor> read_actors)
        {
            List<Actor> values = ValueList((Dictionary<int, Actor>)read_actors);

            if (values.Count <= 0) return null;

            int[] distances = new int[values.Count];
            int index = 0;
            foreach (Actor actor in values)
            {
                distances[index] = Math.Abs((int)DistanceToActor(actor).Length());
                index++;
            }

            int nearestdist = distances.Min();
            int nearestindex = distances.ToList().FindIndex(i => i == nearestdist);
            Actor nearest = values[nearestindex];


            return nearest;
        }
        #endregion

        #region Actor Type
        public bool IsType(Type type)
        {
            bool confirm = false;
            if (this.GetType().IsSubclassOf(type))
            {
                confirm = true;
            }

            return confirm;
        }
        #endregion
    }
}
