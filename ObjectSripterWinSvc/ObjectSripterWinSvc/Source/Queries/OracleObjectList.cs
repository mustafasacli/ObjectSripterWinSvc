using ObjectSripterWinSvc.Source.Interfaces;
using System.Data;

namespace ObjectSripterWinSvc.Source.Queries
{
    class OracleObjectList : IQuery
    {
        public CommandType QueryType { get { return CommandType.Text; } }

        public string[] GetParameterList()
        {
            return new string[] { ":POWNER", ":PTYPENAME" };
        }

        public string GetQuery()
        {
            return @"SELECT
    AO.OWNER,
    AO.OBJECT_NAME AS NAME,
    AO.OBJECT_TYPE AS TYPENAME
FROM ALL_OBJECTS AO
WHERE
    AO.OWNER = :POWNER AND AO.OBJECT_TYPE = :PTYPENAME; ";
        }
    }
}
