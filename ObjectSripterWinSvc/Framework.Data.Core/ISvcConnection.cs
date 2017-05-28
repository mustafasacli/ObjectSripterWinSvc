using System.Collections.Generic;
using System.Data;

namespace Framework.Data.Core
{
    public interface ISvcConnection
    {
        string Owner { get; }

        string ConnType { get; }

        string ConnectionString { get; set; }

        DataTable GetData(string sqlText, CommandType cmdType, Dictionary<string, object> parameters = null);

        void Test();
    }
}
