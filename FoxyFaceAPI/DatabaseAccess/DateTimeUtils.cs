using System;

namespace DatabaseAccess
{
    public static class DateTimeUtils
    {
        public static DateTime ConverTo(object datetime)
        {
            return DateTime.ParseExact((string) datetime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}