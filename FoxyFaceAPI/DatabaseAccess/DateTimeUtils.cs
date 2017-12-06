using System;
using System.Linq.Expressions;

namespace DatabaseAccess
{
    public static class DateTimeUtils
    {
        public static DateTime ConverTo(object datetime)
        {
            if (DateTime.TryParse((string) datetime, out DateTime ret))
                return ret;
            throw new ArgumentException("Couldn't find proper format for datetime: " + datetime);
        }
    }
}