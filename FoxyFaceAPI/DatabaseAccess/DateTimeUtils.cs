using System;
using System.Linq.Expressions;

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
                try
                {
                    return DateTime.ParseExact((string) datetime, "dd/MM/yyyy HH:mm:ss",
                        System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception )
                {
                    try
                    {
                        return DateTime.ParseExact((string) datetime, "yyyy-MM-dd HH:mm:ss",
                            System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            return DateTime.ParseExact((string) datetime, "MM/dd/yyyy HH:mm:ss",
                                System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch (Exception)
                        {
                            try
                            {
                                return DateTime.ParseExact((string) datetime, "MM/dd/yyyy hh:mm:ss tt",
                                    System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    return DateTime.ParseExact((string) datetime, "G",
                                        System.Globalization.CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                    try
                                    {
                                        if (DateTime.TryParse((string) datetime, out DateTime ret))
                                            return ret;
                                        throw new Exception();
                                    }
                                    catch (Exception)
                                    {
                                        throw new ArgumentException("Couldn't find proper format for datetime: " + datetime);   
                                    }                                      
                                }
                            } 
                        }
                    }
                }
            }
        }
    }
}