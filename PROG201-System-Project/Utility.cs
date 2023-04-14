using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG201_System_Project
{
    internal static class Utility
    {
        public static Random Rand = new Random();

        public static void SetRandomSeed(double seed) => Rand = new Random((int)seed);

        public static void CloseApp() => Environment.Exit(0);
    }
}
