using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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

namespace PROG201_System_Project
{
    internal static class Utility
    {
        public static Random Rand = new Random();

        public static void SetRandomSeed(double seed) => Rand = new Random((int)seed);

        public static void CloseApp() => Environment.Exit(0);

        public static int GetHypotenuse(int a, int b) => (int)Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));

        public static void Increment(dynamic inc, dynamic val, dynamic max)
        {
            if (inc + val > max)
            {
                inc = max;
            }
            else { inc += val; }
        }

        public static Brush BrushFromString(string str)
        {
            BrushConverter converter = new BrushConverter();
            Brush brush = (Brush)converter.ConvertFromString(str);
            return brush;
        }

        public static BitmapImage ImageFromString(string str)
        {
            string path = $"/../images/{str}";
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

        #region XML Load Actors
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
        #endregion

        #region Cast
        private static Func<object, object> MakeCastDelegate(Type from, Type to)
        {
            var p = Expression.Parameter(typeof(object)); //do not inline
            return Expression.Lambda<Func<object, object>>(
                Expression.Convert(Expression.ConvertChecked(Expression.Convert(p, from), to), typeof(object)),
                p).Compile();
        }

        private static readonly Dictionary<Tuple<Type, Type>, Func<object, object>> CastCache
        = new Dictionary<Tuple<Type, Type>, Func<object, object>>();

        public static Func<object, object> GetCastDelegate(Type from, Type to)
        {
            lock (CastCache)
            {
                var key = new Tuple<Type, Type>(from, to);
                Func<object, object> cast_delegate;
                if (!CastCache.TryGetValue(key, out cast_delegate))
                {
                    cast_delegate = MakeCastDelegate(from, to);
                    CastCache.Add(key, cast_delegate);
                }
                return cast_delegate;
            }
        }

        public static object Cast(Type t, object o)
        {
            return GetCastDelegate(o.GetType(), t).Invoke(o);
        }
        #endregion

        #region Create Numerable
        public static O CreateInstance<O>()
        {
            Type t = typeof(O);
            return (O)Activator.CreateInstance(t);
        }

        public static IList CreateList(Type myType)
        {
            Type genericListType = typeof(List<>).MakeGenericType(myType);
            return (IList)Activator.CreateInstance(genericListType);
        }

        public static Dictionary<K, V> CreateDict<K, V>()
        {
            Dictionary<K, V> dict = new Dictionary<K, V>();
            return dict;
        }

        public static List<V> ValueList<K, V>(Dictionary<K, V> dict)
        {
            return dict.Values.ToList();
        }

        public static List<K> KeyList<K, V>(Dictionary<K, V> dict)
        {
            return dict.Keys.ToList();
        }

        public static IReadOnlyDictionary<K, V> CreateReadOnlyDict<K, V>(Dictionary<K, V> dict) => dict;

        public static IReadOnlyDictionary<K, V> CreateReadOnlyDict<K, V>(Dictionary<K, V> dict, Type type) => 
            dict.Where(a => ObjectIs(a.Value, type)).
                 ToDictionary(p => p.Key, p => p.Value);

        public static IReadOnlyDictionary<K, V> CreateReadOnlyDict<K, V>(Dictionary<K, V> dict, Type type, Actor caller, Func<V, bool> method) =>
            dict.Where(a => !a.Value.Equals(caller) && ObjectIs(a.Value, type) && method(a.Value)).
                 ToDictionary(p => p.Key, p => p.Value);

        public static IReadOnlyDictionary<K, V> CreateReadOnlyDict<K, V>(Dictionary<K, V> dict, Type type, Actor caller, Func<IProcreate, bool> method) =>
            dict.Where(a => !a.Value.Equals(caller) && ObjectIs(a.Value, type) && method(a.Value as IProcreate)).
                 ToDictionary(p => p.Key, p => p.Value);

        #endregion

        #region Type
        public static bool ObjectIs<T>(object obj)
        {
            bool confirm = false;
            Type type = obj.GetType();

            if (type.GetInterfaces().Contains(typeof(T)))
            {
                confirm = true;
            }

            return confirm;
        }

        public static bool ObjectIs(object obj, Type type)
        {
            bool confirm = false;

            if (obj.GetType() == type)
            {
                confirm = true;
            }

            return confirm;
        }
        #endregion
    }

    public class ObjectToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.GetType().Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
