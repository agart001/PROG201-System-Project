using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace PROG201_System_Project.interfaces
{
    internal interface IMove
    {
        public void MoveGridActor(Grid grid, Image sprite, int move_y, int move_x);

        public UIElement CheckGridCollision(Grid grid, int y, int x);
    }
}
