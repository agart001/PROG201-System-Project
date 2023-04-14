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
using static PROG201_System_Project.Utility;

namespace PROG201_System_Project.pages
{
    /// <summary>
    /// Interaction logic for game_page.xaml
    /// </summary>
    public partial class game_page : Page
    {
        public game_page()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MainWindow.UI.MoveGridActor(grd_Board,tb_char, 0, 1);

        }
    }
}
