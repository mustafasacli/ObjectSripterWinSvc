using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common.Factory
{
    internal static class StringHelper
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
