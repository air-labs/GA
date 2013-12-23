using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AIRLab.Thornado.Modules
{
     public class FullDateTimeFormat : BasicTypeFormat<DateTime>
    {
        public FullDateTimeFormat() : base(Write, Parse, "Дата и время") { }

        static string Write(DateTime time)
        {
            if (time == default(DateTime)) return "";
            if (time.Hour == 0 && time.Minute == 0)
                return string.Format("{0}.{1}.{2}", time.Day, time.Month, time.Year);
            return string.Format("{0}.{1}.{2} {3}:{4}", time.Day, time.Month, time.Year, time.Hour, time.Minute);
        }




        static Regex time = new Regex("([0-9]+) *[:-] *([0-9]+) +");
        static Regex date = new Regex("([0-9]+)[ ./]+([0-9]+)[ ./]+([0-9]*)");
       
        

        
        static DateTime Parse(string str)
        {
            if (string.IsNullOrEmpty(str)) return default(DateTime);
            str += " ";
            int hour = 0;
            int minute = 0;
            int day=1;
            int month=1;
            int year=1;
           

            var tm = time.Match(str);
            bool tSuc=tm.Success;
            if (tSuc)
            {
                hour = int.Parse(tm.Groups[1].Value);
                minute = int.Parse(tm.Groups[2].Value);
                str = str.Replace(tm.Value, "");
                str+=" ";
            }
            var dm = date.Match(str);
            bool dSuc=dm.Success;
            if (dSuc)
            {
                day = int.Parse(dm.Groups[1].Value);
                month = int.Parse(dm.Groups[2].Value);
                if (dm.Groups[3].Value != "")
                {
                    year = int.Parse(dm.Groups[3].Value);
                    if (year < 50) year += 2000;
                    if (year > 50 && year < 100) year += 1900;
                }
                else
                    year=DateTime.Now.Year;
                str = str.Replace(dm.Value, "");
            }

            if (!string.IsNullOrWhiteSpace(str))
                throw new Exception("");

            if (tSuc && dSuc) return new DateTime(year, month, day, hour, minute, 0);
            if (tSuc) return new DateTime(1, 1, 1, hour, minute, 0);
            if (dSuc) return new DateTime(year, month, day);
            throw new Exception("");
            
        }

    
    }
}
