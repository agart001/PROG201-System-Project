using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PROG201_System_Project.actors;
using static PROG201_System_Project.Utility;

namespace PROG201_System_Project.pages
{
    /// <summary>
    /// Interaction logic for game_page.xaml
    /// </summary>
    public partial class game_page : Page
    {
        Moth m;

        Water w;
        public game_page()
        {
            InitializeComponent();

            m = new Moth();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Image img = m.Sprite;

            MainWindow.UI.SpawnGridActor(grd_Board, m.Sprite, 5, 10);
            MainWindow.UI.SpawnGridActor(grd_Board, w.Sprite, 6, 10);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MainWindow.UI.MoveGridActor(grd_Board,m.Sprite, 0, 1);

        }
    }
}
