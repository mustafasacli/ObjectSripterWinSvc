using System;

namespace Framework.Data.Core.Utils
{
    public class ConvertUtil
    {
        public static bool IsNullOrDbNull(object obj)
        {
            return obj == null || obj == DBNull.Value;
        }

        public static DateTime ToDate(object obj)
        {
            try
            {
                return ToDate(obj.ToString());
            }
            catch (Exception)
            {
                return new DateTime();
            }
        }

        public static DateTime ToDate(string str)
        {
            try
            {
                return DateTime.Parse(str);
            }
            catch (Exception)
            {
                return new DateTime();
            }
        }

        public static string DateToStr(DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss FFF");
        }

        public static double ToDouble(object obj)
        {
            return ToDouble(string.Format("{0}", obj));
        }

        public static double ToDouble(string str)
        {
            double d;
            double.TryParse(str, out d);
            return d;
        }

        public static decimal ToDecimal(object obj)
        {
            return ToDecimal(string.Format("{0}", obj));
        }

        public static decimal ToDecimal(string str)
        {
            decimal d;
            decimal.TryParse(str, out d);
            return d;
        }

        public static float ToFloat(object obj)
        {
            return ToFloat(string.Format("{0}", obj));
        }

        public static float ToFloat(string str)
        {
            float f;
            float.TryParse(str, out f);
            return f;
        }

        public static int ToInt(object obj)
        {
            return ToInt(string.Format("{0}", obj));
        }

        public static int ToInt(string str)
        {
            int i;
            int.TryParse(str, out i);
            return i;
        }

        public static long ToLong(object obj)
        {
            return ToLong(string.Format("{0}", obj));
        }

        public static long ToLong(string str)
        {
            long l;
            long.TryParse(str, out l);
            return l;
        }

        public static bool ToBool(string str)
        {
            bool result = false;
            try
            {
                result = string.Format("{0}", str).ToLower().Equals("true");
            }
            catch (Exception)
            {
            }
            return result;
        }

        public static bool ToBool(object obj)
        {
            bool result = false;
            try
            {
                result = string.Format("{0}", obj).ToLower().Equals("true");
            }
            catch (Exception)
            {
            }
            return result;
        }
    }
}
