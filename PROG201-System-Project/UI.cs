using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PROG201_System_Project
{
    public class UI
    {
        Frame main;

        public UI(Frame frame)
        {
            main = frame;
        }

        //Navigate to page, set frame source
        public void UpdatePage(string _page)
        {
            string page = "pages/" + _page + "_page.xaml";
            Uri uri = new Uri(page, UriKind.Relative);
            main.Source = uri;
        }

        public void SpawnGridActor(Grid grid, Image sprite, int spawn_y, int spawn_x)
        {
            Grid.SetRow(sprite, spawn_y);
            Grid.SetColumn(sprite, spawn_x);

            grid.Children.Add(sprite);
        }

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
    }
}
