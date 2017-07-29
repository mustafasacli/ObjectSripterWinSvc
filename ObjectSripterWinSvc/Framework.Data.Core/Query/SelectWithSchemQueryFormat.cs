namespace Framework.Data.Core.Query
{
    public class SelectWithSchemQueryFormat
    {
        public string GetFormat()
        {
            return "SELECT #COLUMNS# FROM #SCHEMA_NAME#.#TABLE_NAME#";
        }

        public string[] GetFormatKeys()
        {
            return new string[] { "#COLUMNS#", "#SCHEMA_NAME#","#TABLE_NAME#" };
        }
    }
}
