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
using static PROG201_System_Project.SimCache;
using Image = System.Windows.Controls.Image;

namespace PROG201_System_Project
{
    internal static class Utility
    {
        public static Random Rand = new Random();


        public static void SetRandomSeed(double seed) => Rand = new Random((int)seed);

        public static void CloseApp() => Environment.Exit(0);

        public static int GetHypotenuse(int a, int b) => (int)Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));

        public static void Increment(dynamic val, dynamic inc, dynamic max)
        {
            if (val + inc > max)
            {
                val = max;
            }
            else { val += inc; }
        }

        public static void Decrement(dynamic val, dynamic dec, dynamic min)
        {
            if (val - dec < min)
            {
                val = min;
            }
            else { val -= dec; }
        }

        public static Brush BrushFromString(string str)
        {
            BrushConverter converter = new BrushConverter();
            Brush brush = (Brush)converter.ConvertFromString(str);
            return brush;
        }

        public static Image ImageFromCache(string key) => ImageCache[key];

        #region XML Load Actors
        static Actor ParseActor(string type)
        {
            Actor actor = null;
            Type t = ActorCache.Find(a => a.Name == type);

            if (t == null) return null;

            actor = CreateInstance<Actor>(t);

            return actor;
        }

        public static void LoadActorsXML(Grid grid, Dictionary<int, Actor> actors)
        {
            string path = "../../../xml/actors.xml";
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            XmlNode root = xml.DocumentElement;
            XmlNodeList ActorList = root.SelectNodes("/actors/actor");
            xml.AppendChild(root);
            foreach (XmlElement actor in ActorList)
            {
                string type = actor.GetAttribute("type");

                Actor Actor = ParseActor(type);

                if (Actor == null) return;

                if (ObjectIs<IProcreate>(Actor))
                {
                    IProcreate cast = (IProcreate)Actor;
                    int sex = Convert.ToInt32(actor.GetAttribute("sex"));

                    switch(sex)
                    {
                        case 1: cast.Chromesome = IProcreate.ChromesomeType.Y; break;
                        case 2: cast.Chromesome = IProcreate.ChromesomeType.X; break;
                        default: break;
                    }
                }

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
        public static O CreateInstance<O>(Type t) => (O)Activator.CreateInstance(t.Assembly.FullName, t.FullName).Unwrap();

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

        public static IReadOnlyDictionary<K, V> CreateReadOnlyDict<K, V>(Dictionary<K, V> dict, string type) =>
            dict.Where(a => ObjectIs(a.Value, type)).
                 ToDictionary(p => p.Key, p => p.Value);

        public static IReadOnlyDictionary<K, V> CreateReadOnlyDict<K, V>(Dictionary<K, V> dict, Type type, Actor caller, Func<V, bool> method) =>
            dict.Where(a => !a.Value.Equals(caller) && ObjectIs(a.Value, type) && method(a.Value)).
                 ToDictionary(p => p.Key, p => p.Value);

        public static IReadOnlyDictionary<K, V> CreateReadOnlyDict<K, V>(Dictionary<K, V> dict, Type type, Actor caller, Func<IProcreate, bool> method) =>
            dict.Where(a => !a.Value.Equals(caller) && ObjectIs(a.Value, type) && method((IProcreate)a.Value)).
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

        public static bool ObjectIs(object obj, string type)
        {
            bool confirm = false;

            if (obj.GetType().Name == type)
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
            string str = value.ToString();
            int dot = str.LastIndexOf(".");
            string name = str.Remove(0, dot+1);
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
