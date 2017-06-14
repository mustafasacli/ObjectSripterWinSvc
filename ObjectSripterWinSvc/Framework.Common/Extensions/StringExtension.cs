namespace Framework.Common.Extensions
{
    internal static class StringExtension
    {
        public static string ConvertTurkishChars(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            string s = str;

            s = s.Replace("Ğ", "G");
            s = s.Replace("Ü", "U");
            s = s.Replace("Ş", "S");
            s = s.Replace("İ", "I");
            s = s.Replace("Ö", "O");
            s = s.Replace("Ç", "C");
            s = s.Replace("ğ", "g");
            s = s.Replace("ü", "u");
            s = s.Replace("ş", "s");
            s = s.Replace("ı", "i");
            s = s.Replace("ö", "o");
            s = s.Replace("ç", "c");

            return s;
        }

        public static string ClearString(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            string s = str;

            s = s.Replace(" ", string.Empty);
            s = s.Replace(".", string.Empty);
            s = s.Replace(",", string.Empty);
            s = s.Replace("!", string.Empty);
            s = s.Replace("'", string.Empty);
            s = s.Replace("^", string.Empty);
            s = s.Replace("+", string.Empty);
            s = s.Replace("%", string.Empty);
            s = s.Replace("&", string.Empty);
            s = s.Replace("/", string.Empty);
            s = s.Replace("(", string.Empty);
            s = s.Replace(")", string.Empty);
            s = s.Replace("=", string.Empty);
            s = s.Replace("\\", string.Empty);
            s = s.Replace("?", string.Empty);
            s = s.Replace("-", string.Empty);
            s = s.Replace("@", string.Empty);
            s = s.Replace("<", string.Empty);
            s = s.Replace(">", string.Empty);
            s = s.Replace("`", string.Empty);
            s = s.Replace(":", string.Empty);
            s = s.Replace("|", string.Empty);
            s = s.Replace("{", string.Empty);
            s = s.Replace("}", string.Empty);
            s = s.Replace("[", string.Empty);
            s = s.Replace("]", string.Empty);
            s = s.Replace("½", string.Empty);
            s = s.Replace("$", string.Empty);
            s = s.Replace("#", string.Empty);
            s = s.Replace("£", string.Empty);
            s = s.Replace("|", string.Empty);
            s = s.Replace("\"", string.Empty);
            s = s.Replace("\t", string.Empty);
            s = s.Replace("\n", string.Empty);
            s = s.Replace("\r", string.Empty);
            s = s.Replace("*", string.Empty);
            //s = s.Replace("", string.Empty);
            //s = s.Replace("", string.Empty);
            //s = s.Replace("", string.Empty);
            //s = s.Replace("", string.Empty);
            //s = s.Replace("", string.Empty);
            //s = s.Replace("", string.Empty);

            return s;
        }

        public static bool IsNotValid(this string s)
        {
            bool b = false;
            b = string.IsNullOrWhiteSpace(s);
            return b;
        }

        public static bool IsValid(this string s)
        {
            bool b = false;
            b = string.IsNullOrWhiteSpace(s);
            b ^= true;
            return b;
        }
    }
}
