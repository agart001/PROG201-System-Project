using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using PROG201_System_Project.systems;
using static PROG201_System_Project.Utility;
using static PROG201_System_Project.SimCache;

namespace PROG201_System_Project.pages
{
    /// <summary>
    /// Interaction logic for game_page.xaml
    /// </summary>
    public partial class game_page : Page
    {
        public Simulation Sim;
        public game_page()
        {
            InitializeComponent();

            Dictionary<string, Image> images = ImageCache;

            Sim = new Simulation(grd_Board, 1);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            grd_Page.DataContext = Sim;
        }

        private void Slow_Click(object sender, RoutedEventArgs e)
        {
            Sim.IncreaseInterval();
        }

        private void Fast_Click(object sender, RoutedEventArgs e)
        {
            Sim.DecreaseInterval();
        }

        private void Actor_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton button = sender as RadioButton;

            string groupname = (string)button.GroupName;
            string content = (string)button.Content;

            Sim.FindActor(content);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            int x = Convert.ToInt32(tb_AddX.Text);
            int y = Convert.ToInt32(tb_AddY.Text);

            Sim.AddActor(x, y);

            MainWindow.UI.UnCheckGridButtons(grd_ActorButtons);
        }
    }
}
