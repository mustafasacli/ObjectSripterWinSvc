using ObjectSripterWinCA.Source.Interfaces;
using System.Data;

namespace ObjectSripterWinCA.Source.Queries
{
    class OracleObjectListV3 : IQuery
    {
        public CommandType QueryType { get { return CommandType.Text; } }

        public string[] GetParameterList()
        {
            return new string[] { ":PTYPENAME" };
        }

        public string GetQuery()
        {
            return
            //@"SELECT AO.OWNER, AO.OBJECT_NAME AS NAME, AO.OBJECT_TYPE AS TYPENAME FROM ALL_OBJECTS AO WHERE AO.OWNER = :POWNER AND AO.OBJECT_TYPE = :PTYPENAME";
            @"SELECT AO.OWNER, AO.OBJECT_NAME AS NAME, AO.OBJECT_TYPE AS TYPENAME FROM ALL_OBJECTS AO WHERE AO.OBJECT_TYPE=:PTYPENAME AND AO.OWNER NOT LIKE '%SYS%' AND AO.OWNER NOT LIKE 'XDB%' AND AO.OWNER NOT LIKE 'APEX%' AND AO.OWNER NOT LIKE 'OUTLN%' AND AO.OWNER NOT LIKE 'ANONYMOUS%' AND AO.OWNER NOT LIKE '%SNMP%' AND AO.OWNER NOT LIKE '%ORDDATA%' AND AO.OWNER NOT LIKE '%ORDPLUGINS%' AND AO.OWNER NOT LIKE '%ORACLE_OCM%' ORDER BY AO.OWNER, AO.OBJECT_NAME";
        }
    }
}
