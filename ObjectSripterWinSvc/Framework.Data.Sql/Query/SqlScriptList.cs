using Framework.Data.Core.Interfaces;
using System.Data;

namespace Framework.Data.Sql.Query
{
    internal class SqlScriptList : IQuery
    {
        public CommandType QueryType { get { return CommandType.StoredProcedure; } }

        public string[] GetParameterList()
        {
            return new string[] { "@objname" };
        }

        public string GetQuery()
        {
            return "sp_helptext";
        }
    }
}
