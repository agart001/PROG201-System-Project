using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PROG201_System_Project.interfaces
{
    internal interface IProcreate
    {
        public enum ChromesomeType
        {
            X,
            Y
        }

        public ChromesomeType Chromesome { get; set; }

        public enum BirthType
        {
            Live,
            Eggs,
            Seeds
        };

        public BirthType Birth { get; set; }

        public Actor Mate { get; set; }

        public int MaxOffspring { get; set; }
        public List<Actor> Offspring { get; set; }

        public int MaxGestation { get; set; }
        public int Gestation{ get; set; }
        public bool Gestating { get; set; }
        public bool ReadyToDeliver { get; set; }

        public bool ReadyToMate { get; set; }
        public int MaxHappy { get; set; }
        public int Happy { get; set; }

        public string MatingSeason { get; set; }
        public Actor BirthPlace { get; set; }
        public int BirthRange { get; set; }

        void InSeason(string season);
        void IncreaseHappy();
        Actor FindNearestBirthPlace(Grid grid, Dictionary<Image, Actor> actors);
        Actor FindNearestMate(Grid grid, Dictionary<Image, Actor> actors);
        void GiveBirth(Grid grid, Dictionary<Image, Actor> actors);
    }
}
