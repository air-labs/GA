using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace AIRLab.Thornado
{
    partial class TypeResolver
    {
        static void InitOverrides()
        {
            
            ClassResolver<DateTime>.OverrideFields("Year", "Month", "Day", "Hour", "Minute", "Second","Milisecond");
            ClassResolver<DateTime>.OverrideConstructors(
                new Func<int, int, int, int, int, int, int, DateTime>((Year, Month, Day, Hour, Minute, Second, Milisecond) => new DateTime(Year, Month, Day, Hour, Minute, Second, Milisecond)),
                new Func<int, int, int, int, int, int, DateTime>((Year, Month, Day, Hour, Minute, Second) => new DateTime(Year, Month, Day, Hour, Minute, Second)),
                new Func<int, int, int, int, int, DateTime>((Year, Month, Day, Hour, Minute) => new DateTime(Year, Month, Day, Hour, Minute, 0)),
                new Func<int, int, int, int, DateTime>((Year, Month, Day, Hour) => new DateTime(Year, Month, Day, Hour, 0, 0)),
                new Func<int, int, int, DateTime>((Year, Month, Day) => new DateTime(Year, Month, Day)),
                new Func<int, int, int, DateTime>((Hour, Minute, Second) => new DateTime(1, 1, 1, Hour, Minute, Second)),
                new Func<int, int, DateTime>((Hour,Minute)=>new DateTime(1,1,1,Hour,Minute,0))
                );

            ClassResolver<Color>.OverrideFields("R", "G", "B");
            ClassResolver<Color>.OverrideConstructors(
                new Func<int, int, int, Color>((R, G, B) => Color.FromArgb(R, G, B))
                );

        



        }
    }
}
