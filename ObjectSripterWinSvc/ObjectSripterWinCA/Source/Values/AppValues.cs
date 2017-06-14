namespace ObjectSripterWinCA.Source.Values
{
    internal static class AppValues
    {
        internal static string SaveFolderName { get { return "Metadata"; } }

        internal static string DateFormat
        {
            get
            {
                return "_yyyy_MM_dd_HH_mm_ss_ffffff";
                //return ConfigurationManager.AppSettings["DateFormat"];
            }
        }
    }
}
