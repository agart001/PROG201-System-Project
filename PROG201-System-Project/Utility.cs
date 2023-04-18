using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PROG201_System_Project
{
    internal static class Utility
    {
        public static Random Rand = new Random();

        public static void SetRandomSeed(double seed) => Rand = new Random((int)seed);

        public static void CloseApp() => Environment.Exit(0);

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
    }
}
