using PROG201_System_Project.actors.creatures;
using PROG201_System_Project.actors.landscapes;
using PROG201_System_Project.actors.plants;
using PROG201_System_Project.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        public string ImageFile = "default";

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
        #endregion

        #region Actor Type
        public bool IsCreature() 
        {
            bool confirm = false;
            if(this as Creature != null)
            {
                confirm = true;
            }

            return confirm;
        }

        public bool IsPlant()
        {
            bool confirm = false;
            if (this as Plant != null)
            {
                confirm = true;
            }

            return confirm;
        }

        public bool IsLandscape()
        {
            bool confirm = false;
            if (this as Landscape != null)
            {
                confirm = true;
            }

            return confirm;
        }
        #endregion
    }
}
