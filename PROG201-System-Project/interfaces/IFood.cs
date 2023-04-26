using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG201_System_Project.interfaces
{
    public interface IFood
    {
        public int Calories { get; set; }

        public bool Eaten { get; set; }
    }
}
