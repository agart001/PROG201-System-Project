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
    }
}
