using Framework.Data.Core.Interfaces;

namespace Framework.Data.Core.Query
{
    public class SelectQueryFormat : IQueryFormat
    {
        public string GetFormat()
        {
            return "SELECT #COLUMNS# FROM #TABLE_NAME#";
        }

        public string[] GetFormatKeys()
        {
            return new string[] { "#COLUMNS#", "#TABLE_NAME#" };
        }
    }
}
