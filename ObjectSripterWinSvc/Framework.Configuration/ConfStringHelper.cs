namespace Framework.Configuration
{
    internal static class ConfStringHelper
    {
        internal static bool IsNotValid(this string s)
        {
            bool b = false;
            b = string.IsNullOrWhiteSpace(s);
            return b;
        }

        internal static bool IsValid(this string s)
        {
            bool b = false;
            b = string.IsNullOrWhiteSpace(s);
            b ^= true;
            return b;
        }
    }
}
