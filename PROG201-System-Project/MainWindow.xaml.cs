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
using static PROG201_System_Project.SimCache;

namespace PROG201_System_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Source: https://stackoverflow.com/questions/5411694/get-all-inherited-classes-of-an-abstract-class
        //Source: https://stackoverflow.com/questions/972636/casting-a-variable-using-a-type-variable
        public static UI UI { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            UI = new UI(Main);

            UI.UpdatePage("game");

            LoadImageCache();
            GetTypeCache();
        }
    }
}
