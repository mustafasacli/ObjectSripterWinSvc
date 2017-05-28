using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectSripterWinCA.Source.Values
{
    internal static class AppValues
    {
        internal static string ConnStr
        { get { return ConfigurationManager.AppSettings["ConnStr"]; } }

        internal static string SaveFolder
        { get { return ConfigurationManager.AppSettings["SaveFolder"]; } }

        internal static string SaveFolderName { get { return "Metadata"; } }

        internal static string DateFormat
        {
            get
            {
                return "_yyyy_MM_dd_HH_mm_ss_ffffff";
                //return ConfigurationManager.AppSettings["DateFormat"];
            }
        }

        internal static string Types
        { get { return ConfigurationManager.AppSettings["Types"] ?? string.Empty; } }

        internal static bool WriteScriptToConsole
        {
            get
            {
                var sd = ConfigurationManager.AppSettings["WriteScriptToConsole"] ?? string.Empty;
                sd = sd.Trim();
                bool writeToC = string.Equals(sd, "1");
                return writeToC;
            }
        }
    }
}
