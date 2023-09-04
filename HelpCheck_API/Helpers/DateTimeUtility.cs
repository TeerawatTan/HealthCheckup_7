using HelpCheck_API.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Helpers
{
    public class DateTimeUtility
    {
        public static DateTime GetCurrentDateTime()
        {
            DateTime serverTime = DateTime.Now;
            DateTime utcTime = serverTime.ToUniversalTime();
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(Constant.SE_Asia_Standard_Time);

            DateTime currentDt = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);

            return currentDt;
        }

        public static string ConvertDateToDateString(DateTimeOffset? date)
        {
            if (!date.HasValue) return "";
            DateTime _date = date.Value.DateTime;
            return _date.Day + " " + Constant.MASTER_MONTH_NAME[_date.Month] + " " + (_date.Year + 543);
        }

        public static string ConvertTimeToTimeString(DateTimeOffset? date)
        {
            if (!date.HasValue) return "";
            DateTime _date = date.Value.DateTime;
            return _date.Hour.ToString().PadLeft(2, '0') + ":" + _date.Minute.ToString().PadLeft(2, '0');
        }

        public static int CalculateAge(DateTime? birthDate)
        {
            if (!birthDate.HasValue)
            {
                return 0;
            }
            var today = DateTime.UtcNow;
            var age = today.Year - birthDate.Value.Year;
            if (birthDate.Value.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
