namespace Framework.Configuration
{
    internal static class ConfigurationConstants
    {
        internal static readonly string confFileName = "service-conf.xml";

        // configuration/connections/connection
        internal static readonly string ConnectionNode = "configuration/connections/connection";

        //connection-string-format
        internal static readonly string ConnectionStringFormat = "connection-string-format";

        //connection-string
        internal static readonly string ConnectionString = "connection-string";

        internal static readonly string ConnSettings = "conn-settings/setting";

        // format-keys --> format-key
        internal static readonly string FormatKeysNode = "format-keys";

        internal static readonly string FormatKey = "format-key";

        //setting
        internal static readonly string SettingNode = "setting";

        internal static readonly string Keys = "keys";

        //key --> value
        internal static readonly string Key = "key";

        internal static readonly string Value = "value";
        public static readonly string name = "name";
        public static readonly string typename = "type";

        // configuration/imported-assemblies/assembly
        internal static readonly string AssembliesNode = "configuration/imported-assemblies/assembly";

        // type namespace class
        internal static readonly string NdType = "type";

        internal static readonly string NdNamespace = "namespace";
        internal static readonly string NdClass = "class";
    }
}
