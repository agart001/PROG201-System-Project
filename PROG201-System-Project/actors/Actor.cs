using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using static PROG201_System_Project.Utility;

namespace PROG201_System_Project
{
    public class Actor
    {
        public TextBlock Sprite = new TextBlock();

        public virtual string SpriteColor { get; set; }

        public virtual void PreContruct()
        {

        }

        public virtual void PostContruct()
        {

        }

        public Actor()
        {
            PreContruct();

            Sprite.Background = BrushFromString(SpriteColor);

            PostContruct();
        }
    }
}
