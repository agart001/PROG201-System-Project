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
    public interface IMove
    {
        void Move(Grid grid, Image sprite, int move_y, int move_x);

        void MoveToActor(Grid grid, Actor actor);

        UIElement CheckGridCollision(Grid grid, int y, int x);

        void MoveRandom(Grid grid);
    }
}
