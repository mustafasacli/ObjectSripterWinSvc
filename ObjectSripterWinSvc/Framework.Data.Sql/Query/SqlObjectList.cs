using Framework.Data.Core.Interfaces;
using System.Data;

namespace Framework.Data.Sql.Query
{
    internal class SqlObjectList : IQuery
    {
        public CommandType QueryType { get { return CommandType.Text; } }

        public string[] GetParameterList()
        {
            return new string[] { "@PTYPENAME" };
        }

        public string GetQuery()
        {
            return @"SELECT
SCH.NAME AS OWNER,
--CASE SCH.NAME WHEN NULL THEN O.NAME
--ELSE SCH.NAME +'.'+ O.NAME END AS NAME
O.NAME AS NAME
FROM SYS.OBJECTS O
INNER JOIN SYS.SCHEMAS SCH ON O.SCHEMA_ID = SCH.SCHEMA_ID
                        WHERE O.TYPE = @PTYPENAME
                        GROUP BY O.NAME, SCH.NAME
                        ORDER BY SCH.NAME, O.NAME;";
        }
    }
}
