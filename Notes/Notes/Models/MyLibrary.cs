using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Notes.Models
{
    public static class MyLibrary
    {
        public static string GetSimpleDate(DateTime DateForFind)
        {
            string Date = "";
            int Year = DateForFind.Year;
            int Month = DateForFind.Month;
            int Day = DateForFind.Day;
            int Hours = DateForFind.Hour;
            int Min = DateForFind.Minute;

            if (Year < 10) Date += "0" + Year + "-";
            else Date += Year + "-";

            if (Month < 10) Date += "0" + Month + "-";
            else Date += Month + "-";

            if (Day < 10) Date += "0" + Day;
            else Date += Day;

            Date += "T";

            if (Hours < 10) Date += "0" + Hours + ":";
            else Date += Hours + ":";

            if (Min < 10) Date += "0" + Min;
            else Date += Min;

            return Date;
        }

        public static string GetSimpleDateFormat2(DateTime DateForFind)
        {
            string MyDate = "";
            int Year = DateForFind.Year;
            int Month = DateForFind.Month;
            int Day = DateForFind.Day;
            int Hours = DateForFind.Hour;
            int Min = DateForFind.Minute;

            if (Day < 10) MyDate += "0" + Day + ".";
            else MyDate += Day + ".";

            if (Month < 10) MyDate += "0" + Month + ".";
            else MyDate += Month + ".";


            if (Year < 10) MyDate += "0" + Year;
            else MyDate += Year;

            MyDate += " ";

            if (Hours < 10) MyDate += "0" + Hours + ":";
            else MyDate += Hours + ":";

            if (Min < 10) MyDate += "0" + Min;
            else MyDate += Min;

            return MyDate;
        }
    }
}