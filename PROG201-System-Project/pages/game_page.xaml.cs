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
using PROG201_System_Project.actors.creatures;
using PROG201_System_Project.actors.landscapes;
using PROG201_System_Project.actors.plants;
using static PROG201_System_Project.Utility;

namespace PROG201_System_Project.pages
{
    /// <summary>
    /// Interaction logic for game_page.xaml
    /// </summary>
    public partial class game_page : Page
    {
        public Dictionary<Image, Actor> Actors;

        Moth m;

        Water w;
        Water w2;

        Yucca y;
        public game_page()
        {
            InitializeComponent();

            m = new Moth();
            w = new Water();
            w2 = new Water();
            y = new Yucca();

            Actors = new Dictionary<Image, Actor>();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Image img = m.Sprite;

            w2.SpawnGridActor(grd_Board, Actors, 12, 15);
            w.SpawnGridActor(grd_Board,Actors, 10, 12);

            y.SpawnGridActor(grd_Board, Actors, 12, 20);

            m.SpawnGridActor(grd_Board,Actors, 14, 18);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            m.MoveToFood(grd_Board, Actors);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            m.MoveToWater(grd_Board, Actors);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            m.MoveRandom(grd_Board);
        }
    }
}
