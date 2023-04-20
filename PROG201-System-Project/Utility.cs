using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using PROG201_System_Project.actors.creatures;
using PROG201_System_Project.actors.landscapes;
using PROG201_System_Project.actors.plants;

namespace PROG201_System_Project
{
    internal static class Utility
    {
        public static Random Rand = new Random();

        public static void SetRandomSeed(double seed) => Rand = new Random((int)seed);

        public static void CloseApp() => Environment.Exit(0);

        public static int GetHypotenuse(int a, int b) => (int)Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));

        public static Brush BrushFromString(string str)
        {
            BrushConverter converter = new BrushConverter();
            Brush brush = (Brush)converter.ConvertFromString(str);
            return brush;
        }

        public static BitmapImage ImageFromString(string str)
        {
            string path = $"/../images/{str}.BMP";
            Uri uri = new Uri(path, UriKind.Relative);
            BitmapImage image = new BitmapImage(uri);
            return image;
        }

        public static string ImageFileFromPath(string str)
        {
            char[] chars = str.ToCharArray();
            char[] tofolder = chars.SkipWhile(i => i != 's').ToArray();
            string folder = new string(tofolder);
            char[] tofile = tofolder.SkipWhile(i => i != '/').ToArray();
            tofile = tofile.Where(i => i != '/').ToArray();
            string file = new string(tofile);
            return file;
        }

        static Actor ParseLandscape(int id)
        {
            Actor actor = null;
            switch (id)
            {
                case 0: actor = new Water(); break;
            }

            return actor;
        }

        static Actor ParsePlant(int id)
        {
            Actor actor = null;
            switch (id)
            {
                case 0: actor = new Yucca(); break;
            }

            return actor;
        }

        static Actor ParseCreature(int id)
        {
            Actor actor = null;
            switch (id)
            {
                case 0: actor = new Moth(); break;
            }

            return actor;
        }

        static Actor ParseActor(int type, int id)
        {
            Actor actor = null;
            switch(type)
            {
                case 0: actor = ParseLandscape(id); break;
                case 1: actor = ParsePlant(id); break;
                case 2: actor = ParseCreature(id); break;
            }
            return actor;
        }

        public static void LoadActorsXML(Grid grid, Dictionary<Image, Actor> actors)
        {
            string path = "../../../xml/actors.xml";
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            XmlNode root = xml.DocumentElement;
            XmlNodeList ActorList = root.SelectNodes("/actors/actor");
            xml.AppendChild(root);
            foreach (XmlElement actor in ActorList)
            {
                int typeID = Convert.ToInt32(actor.GetAttribute("typeid"));
                int actorID = Convert.ToInt32(actor.GetAttribute("actorid"));

                Actor Actor = ParseActor(typeID, actorID);

                if (Actor == null) return;

                int spawn_x = Convert.ToInt32(actor.GetAttribute("x"));
                int spawn_y = Convert.ToInt32(actor.GetAttribute("y"));

                if(spawn_x == 0 && spawn_y ==0)
                {
                    //40
                    int total_x = grid.ColumnDefinitions.Count;
                    //20
                    int total_y = grid.RowDefinitions.Count;

                    spawn_x = Rand.Next(0, total_x);
                    spawn_y = Rand.Next(0, total_y);
                }

                Actor.SpawnGridActor(grid, actors, spawn_y, spawn_x);

            }
        }
    }
}
