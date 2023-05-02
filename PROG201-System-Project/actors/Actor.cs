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

namespace PROG201_System_Project
{
    public class Actor
    {
        public int TypeID { get; set; }
        public int ActorID { get; set; }

        public Image Sprite = new Image();

        public string ImageFile = "default.BMP";

        public int Grid_Y { get; set; }
        public int Grid_X { get; set; }


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

            Sprite.Source = ImageFromString(ImageFile);

            PostContruct();
        }


        public virtual void TickAction()
        {

        }
        public virtual void TickAction(Grid grid)
        {

        }
        public virtual void TickAction(Grid grid, Dictionary<Image, Actor> actors)
        {

        }


        public virtual void SpriteOpacity() { }

        public Actor Clone() => (Actor)this.MemberwiseClone();

        public void DeleteActor(Grid grid, Dictionary<Image, Actor> actors, Actor actor)
        {
            grid.Children.Remove(Sprite);
            actors.Remove(Sprite, out actor);
        }

        #region Position
        public void GetCurrentPosition()
        {
            Grid_Y = Grid.GetRow(this.Sprite);
            Grid_X = Grid.GetColumn(this.Sprite);
        }

        public void SpawnGridActor(Grid grid, Dictionary<Image, Actor> actors, int spawn_y, int spawn_x)
        {
            Grid.SetRow(this.Sprite, spawn_y);
            Grid.SetColumn(this.Sprite, spawn_x);

            actors.Add(this.Sprite, this);
            grid.Children.Add(this.Sprite);

            GetCurrentPosition();
        }

        public Vector2 DistanceToActor(Actor actor)
        {
            Vector2 vector = new Vector2();
            vector.X = actor.Grid_X - this.Grid_X;
            vector.Y = actor.Grid_Y - this.Grid_Y;
            return vector;
        }

        public T FindNearest<T, K, V>(T search_object, IReadOnlyDictionary<K, V> read_actors)
        {
            Type search_type = search_object.GetType();
            Actor search_actor = (Actor)Cast(typeof(Actor), search_object);

            List<K> keys = KeyList((Dictionary<K, V>)read_actors);

            if (keys.Count <= 0) return default;

            int[] distances = new int[keys.Count];
            int index = 0;
            foreach (K key in keys)
            {
                T instance = (T)Cast(typeof(T), read_actors[key]);
                distances[index] = Math.Abs((int)DistanceToActor(instance as Actor).Length());
                index++;
            }

            int smallestdist = distances.Min();
            int smallestindex = distances.ToList().FindIndex(i => i == smallestdist);
            K smallestkey = keys[smallestindex];


            return (T)Cast(typeof(T), read_actors[smallestkey]);
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
