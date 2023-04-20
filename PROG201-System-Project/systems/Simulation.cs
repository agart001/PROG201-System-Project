using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using static PROG201_System_Project.Utility;

namespace PROG201_System_Project.systems
{
    internal class Simulation
    {
        public Grid Board { get; set; }

        public Dictionary<Image, Actor> Actors = new Dictionary<Image, Actor>();

        public DispatcherTimer Timer { get; set; }

        public Simulation(Grid grid, double interval) 
        {
            Board = grid;

            LoadActorsXML(Board, Actors);

            TimeSpan ts = TimeSpan.FromSeconds(interval);
            Timer = new DispatcherTimer();
            Timer.Interval = ts;
            Timer.Tick += Simulation_Tick;

            Start();
        }

        public void Start()
        {
            
            Timer.Start();
        }

        void Simulation_Tick(object sender, EventArgs e)
        {
            foreach(var actor in Actors.Values)
            {
                actor.TickAction(Board);
            }
        }

    }
}
