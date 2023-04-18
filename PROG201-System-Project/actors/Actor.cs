using PROG201_System_Project.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static PROG201_System_Project.Utility;

namespace PROG201_System_Project
{
    public class Actor
    {
        public Image Sprite = new Image();

        public string ImageFile = "default";

        public virtual string SpriteColor { get; set; }

        public int Grid_Y { get; set; }
        public int Grid_X { get; set; }

        public virtual void PreContruct()
        {

        }

        public virtual void PostContruct()
        {

        }

        public Actor()
        {
            PreContruct();

            Sprite.Source = ImageFromString(ImageFile);

            PostContruct();
        }

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

        public Vector DistanceToActor(Actor actor)
        {
            Vector vector = new Vector();
            vector.X = Math.Abs((actor.Grid_X - this.Grid_X));
            vector.Y = Math.Abs((actor.Grid_Y - this.Grid_Y));
            return vector;
        }
    }
}
