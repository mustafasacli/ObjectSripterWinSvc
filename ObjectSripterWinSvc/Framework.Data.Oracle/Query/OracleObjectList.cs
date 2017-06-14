using Framework.Data.Core.Interfaces;
using System.Data;

namespace Framework.Data.Oracle.Query
{
    internal class OracleObjectList : IQuery
    {
        public CommandType QueryType { get { return CommandType.Text; } }

        public string[] GetParameterList() { return new string[] { ":PTYPENAME" }; }

        public string GetQuery()
        {
            return
                @"SELECT ASO.OWNER, ASO.NAME FROM ALL_SOURCE ASO WHERE ASO.TYPE = :PTYPENAME AND ASO.OWNER NOT LIKE '%SYS%' AND ASO.OWNER NOT LIKE 'XDB%' AND ASO.OWNER NOT LIKE 'APEX%' AND ASO.OWNER NOT LIKE 'OUTLN%' AND ASO.OWNER NOT LIKE 'ANONYMOUS%' AND ASO.OWNER NOT LIKE '%SNMP%' AND ASO.OWNER NOT LIKE '%ORDDATA%' AND ASO.OWNER NOT LIKE '%ORDPLUGINS%' AND ASO.OWNER NOT LIKE '%ORACLE_OCM%' GROUP BY ASO.OWNER, ASO.NAME ORDER BY ASO.OWNER, ASO.NAME";
            //@"SELECT AO.OWNER, AO.OBJECT_NAME AS NAME, AO.OBJECT_TYPE AS TYPENAME FROM ALL_OBJECTS AO WHERE AO.OWNER = :POWNER AND AO.OBJECT_TYPE = :PTYPENAME";
        }
    }
}
