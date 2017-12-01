using System;

namespace DatabaseAccess
{
    public static class DateTimeUtils
    {
        public static DateTime ConverTo(object datetime)
        {
            try
            {
                return DateTime.ParseExact((string) datetime, "yyyy. MM. dd. HH:mm:ss",
                    System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return DateTime.ParseExact((string) datetime, "dd/MM/yyyy HH:mm:ss",
                    System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }
}