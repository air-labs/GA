using System;
using System.Linq;
using System.Net;
namespace AIRLab.Thornado.TypeFormats
{
    [PrimaryFormat]
    public class GuidFormat : BasicTypeFormat<Guid>
    {
        public GuidFormat()
            : base(a => a.ToString(), Parse, "Guid")
        {
        }

        static Guid Parse(string str)
        {
            try
            {
                return Guid.Parse(str);
            }
            catch { }

            try
            {
                int i = int.Parse(str);
                var str1 = String.Format("00000000-0000-0000-0000-00000000{0}", i.ToString("0000"));
                return Guid.Parse(str1);
            }
            catch
            {
                throw;
            }
        }

    }
   

    [PrimaryFormat]
    public class IPAddressFormat : BasicTypeFormat<IPAddress>
    {
        public IPAddressFormat()
            : base(
                z => z.ToString(),
                str => IPAddress.Parse(str),
                "IP Address")
        {
        }
    }

    [PrimaryFormat]
    public class IPEndPointFormat : BasicTypeFormat<IPEndPoint>
    {
        public IPEndPointFormat()
            : base(
                z => z.ToString(),
                str =>
                    {
                        string[] parts = str.Split(':');
                        if (parts.Length != 2) throw new Exception("");
                        return new IPEndPoint(Formats.IPAddress.Parse(parts[0]), int.Parse(parts[1]));
                    },
                "IP End Point"
                )
        {
        }
    }

    //public class DateTimeSlashFormat : BasicTypeFormat<DateTime> {
    //    public DateTimeSlashFormat() : base(Write, Parse, Description) {}

    //    public static  DateTime Parse (string str) {
    //        var strs = str.Split('/');
    //        if (strs.Length != 3) throw new Exception(string.Format("DataTimeSlashFormat exception: uncorrect format '{0}'", str));
    //        var day = 0;
    //        var month = 0;
    //        var year = 0;
    //        int.TryParse(strs[0], out day);
    //        int.TryParse(strs[1], out month);
    //        int.TryParse(strs[2], out year);
    //        if (day == 0 || month == 0 || year == 0)
    //            throw new Exception(string.Format("DataTimeSlashFormat exception: uncorrect number '{0}'", str));
    //        return new DateTime(year, month, day);
    //    }
    //    public static string Write (DateTime obj) {
    //        return obj.ToShortDateString().Replace('.', '/');
    //    }
    //    public static string Description {
    //        get {
    //            return "DateTmeBySlash";
    //        }
    //    }
    //}
    public class DateTimeAndTimeSpanFormat : BasicTypeFormat<DateTime>
    {
        public DateTimeAndTimeSpanFormat() : base(Write, Parse, Description) { }

        /// <summary>
        /// Парсер даты и времени
        /// </summary>
        /// <param name="str">01.02.2003 11:57</param>
        /// <returns>Дата и время</returns>
        public new static DateTime Parse(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new DateTime();
            try
            {// Обвязка Try/catch это конечно пиздец... Надо раскурить как по хорошему
                // Убираю лишние пробелы
                str = str.Trim().Replace("  ", " ");

                // Разделяю дату и время
                var dateAndTime = str.Split(' ');
                if (dateAndTime.Length == 0 || dateAndTime.Length > 2)
                    throw new WrongFormatException(str);

                #region Дата

                var date = dateAndTime[0].Split('.');
                if (date.Length != 3)
                    throw new WrongFormatException(str);
                var day = -1;
                var month = -1;
                var year = -1;
                int.TryParse(date[0], out day);
                int.TryParse(date[1], out month);
                int.TryParse(date[2], out year);

                #endregion
                var hour = 0;
                var minute = 0;
                if (dateAndTime.Length == 2)
                {
                    #region Время

                    var time = dateAndTime[1].Split(':');
                    if (time.Length != 2)
                        throw new WrongFormatException(str);
                    int.TryParse(time[0], out hour);
                    int.TryParse(time[1], out minute);

                    #endregion
                }
                if (day == 0 || month == 0 || year == 0 || hour == -1 || minute == -1)
                    throw new WrongFormatException(str);

                return new DateTime(year, month, day, hour, minute, 0);
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }
        public new static string Write(DateTime dateTime)
        {
            if (dateTime == default(DateTime))
                return "";
            if (dateTime.Minute == 0 && dateTime.Hour == 0)
                return dateTime.ToShortDateString();
            return string.Format("{0} {1}", dateTime.ToShortDateString(), dateTime.ToShortTimeString());
        }
        public new static string Description
        {
            get
            {
                return "DateAndTime";
            }
        }
    }


    // todo убрать копипаст
    public class NewDateFormat : BasicTypeFormat<DateTime>
    {
        public NewDateFormat() : base(Write, Parse, Description) { }

        public new static DateTime Parse(string str)
        {
            if (string.IsNullOrEmpty(str)) return default(DateTime);
            try
            {// Обвязка Try/catch это конечно пиздец... Надо раскурить как по хорошему
                // Убираю лишние пробелы
                str = str.Trim().Replace("  ", "");

                #region Дата

                var date = str.Split('.');
                if (date.Length != 3)
                    throw new WrongFormatException(str);
                var day = -1;
                var month = -1;
                var year = -1;
                int.TryParse(date[0], out day);
                int.TryParse(date[1], out month);
                int.TryParse(date[2], out year);

                #endregion

                if (day == 0 || month == 0 || year == 0)
                    throw new WrongFormatException(str);

                return new DateTime(year, month, day);
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }

        public new static string Write(DateTime dateTime)
        {
            if (dateTime == default(DateTime)) return "";
            return string.Format("{0}", dateTime.ToShortDateString());
        }

        public new static string Description
        {
            get
            {
                return "Date";
            }
        }
    }
}