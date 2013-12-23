using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace AIRLab.Bulldozer
{
    public static class BulldozerHelper
    {
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);
        private const double Epsilon = 0.0001;

        public static string CleanTypeName(this Type type)
        {
            var name = type.Name;
            return !type.IsGenericType
                       ? name
                       : name.Remove(name.IndexOf('`'));
        }

        public static double Round(this double number, int frac)
        {
            return Math.Round(number, frac);
        }

        public static int IndexOf<T>(this IEnumerable<T> obj, T value)
        {
            return obj
                .Select((a, i) => (a.Equals(value)) ? i : -1)
                .Max();
        }

        public static int GetInt(int module)
        {
            return Random.Next() % module;
        }

        public static double GetDouble()
        {
            return Random.NextDouble();
        }
        
        public static IEnumerable<double> Range(double min, double max, double step)
        {
            for (var c = min; c <= max + Epsilon; c += step)
                yield return c;
        }
        
        public static T Copy<T>(this T source)
        {

            if (source.Equals(default(T)))
                return default(T);

            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();

            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static T Clone<T>(this T source) 
            where T : ICloneable
        {
            return (T)source.Clone();
        }

        public static T HasMinimal<T, TN>(this IEnumerable<T> enumerable, Func<T, TN> fieldSelector) 
            where TN : IComparable
        {
            var min = enumerable.Min(fieldSelector);
            return enumerable.First(e => fieldSelector(e).CompareTo(min) == 0);
        }
    }
}