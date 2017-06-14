namespace Framework.Configuration
{
    internal class ConfigDefaultValues
    {
        internal static string WriteErrorToLog = "0";
        internal static string WriteConnectionStringToConsole = "1";
        internal static string WriteScriptToConsole = "1";//"0";
        internal static readonly string WriteEventToLog = "0";
        internal static readonly string WriteEventToConsole = "1";
        internal static readonly string ErrorLogFile = "error.log";
        internal static readonly string EventLogFile = "service-events.log";

        internal static readonly string ObjectTypes = "TABLE,PROCEDURE,FUNCTION,TRIGGER,PACKAGE,SEQUENCE,VIEW,INDEX,TYPE";
    }
}
