using Framework.Data.Core.Interfaces;

namespace Framework.Data.Core.Query
{
    public class InsertQueryFormat : IQueryFormat
    {
        public string GetFormat()
        {
            return @"INSERT INTO #SCHEMA_NAME#.#TABLE_NAME# 
(#COLUMNS#)
VALUES
(#COLUMN_VALUES#);";
        }

        public string[] GetFormatKeys()
        {
            return new string[] { "#SCHEMA_NAME#", "#TABLE_NAME#", "#COLUMNS#", "#COLUMN_VALUES#" };
        }
    }
}
