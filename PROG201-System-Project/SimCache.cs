using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using PROG201_System_Project.actors.creatures;
using PROG201_System_Project.actors.landscapes;
using PROG201_System_Project.actors.plants;
using PROG201_System_Project.interfaces;
using Image = System.Windows.Controls.Image;

namespace PROG201_System_Project
{
    internal static class SimCache
    {

        #region Actor Caches
        public static List<Type> ActorCache = new List<Type>();

        public static List<Type> CreatureCache = new List<Type>();
        public static List<Type> LandscapeCache = new List<Type>();
        public static List<Type> PlantCache = new List<Type>();

        static List<Type> GetActorTypes()
        {
            //Sourec: https://stackoverflow.com/questions/5411694/get-all-inherited-classes-of-an-abstract-class
            List<Type> types = new List<Type>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(Actor)).GetTypes()
                .Where(myType => myType.IsSubclassOf(typeof(Actor))))
            {
                types.Add(type);
            }
            
            return types;
        }
        static List<Type> GetCreatureTypes()
        {
            List<Type> types = new List<Type>();
            foreach (Type type in ActorCache.Where(myType => myType.IsSubclassOf(typeof(Creature))))
            {
                types.Add(type);
            }

            return types;
        }
        static List<Type> GetLandscapeTypes()
        {
            List<Type> types = new List<Type>();
            foreach (Type type in ActorCache.Where(myType => myType.IsSubclassOf(typeof(Landscape))))
            {
                types.Add(type);
            }

            return types;
        }
        static List<Type> GetPlantTypes()
        {
            List<Type> types = new List<Type>();
            foreach (Type type in ActorCache.Where(myType => myType.IsSubclassOf(typeof(Plant))))
            {
                types.Add(type);
            }

            return types;
        }

        public static void GetTypeCache()
        {
            ActorCache = GetActorTypes();

            CreatureCache = GetCreatureTypes();
            LandscapeCache = GetLandscapeTypes();
            PlantCache = GetPlantTypes();
        }
        #endregion

        #region Image Cache
        public static Dictionary<string, Image> ImageCache = new Dictionary<string, Image>();

        public static void LoadImageCache()
        {
            DirectoryInfo dirinfo = new DirectoryInfo("images\\sprites");
            FileInfo[] allfiles = dirinfo.GetFiles();

            foreach (FileInfo file in allfiles)
            {
                Image img = new Image();
                BitmapImage bmp = new BitmapImage(new Uri(file.FullName));

                img.Source = bmp;
                img.Width = bmp.Width;

                ImageCache.Add(file.Name, img);
            }
        }
        #endregion
    }
}
