using PROG201_System_Project.actors.creatures;
using PROG201_System_Project.actors.landscapes;
using PROG201_System_Project.actors.plants;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static PROG201_System_Project.Utility;

namespace PROG201_System_Project
{
    public class DictionaryConverter : IValueConverter
    {

        object GetType(object value)
        {
            object obj = null;
            if (value as Dictionary<Creature, int> != null) obj = value as Dictionary<Creature, int>;
            if (value as Dictionary<Landscape, int> != null) obj = value as Dictionary<Landscape, int>;
            if (value as Dictionary<Plant, int> != null) obj = value as Dictionary<Plant, int>;

            return obj;
        }

        Type GetKey(object value)
        {
            Type type = null;
            if (value is Dictionary<Creature, int>) type = typeof(Creature);
            if (value is Dictionary<Landscape, int>) type = typeof(Landscape);
            if (value is Dictionary<Plant, int>) type = typeof(Plant);

            return type;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object type = GetType(value);
            if (type != null)
            {
                Type key = GetKey(type);
                object dict = type;
                if (dict != null)
                {
                    object cast = Cast(key, parameter);
                    return dict[cast];
                }
            }
            throw new NotImplementedException();
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
