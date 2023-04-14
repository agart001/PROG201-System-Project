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

        public void MoveGridActor(Grid grid, TextBlock textblock, int move_y, int move_x)
        {
            int cur_y = Grid.GetRow(textblock);
            int cur_x = Grid.GetColumn(textblock);

            int final_x = cur_x + move_x;
            int final_y = cur_y + move_y;

            if (CheckGridCollision(grid, final_x, final_y) != null) return;

            Grid.SetRow(textblock, final_y);
            Grid.SetColumn(textblock, final_x);
        }

        public UIElement CheckGridCollision(Grid grid, int y, int x)
        {
            UIElement child = grid.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == x && Grid.GetColumn(e) == y);

            return child;
        }
    }
}
