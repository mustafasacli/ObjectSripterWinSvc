using System.Data;

namespace Framework.Data.Core.Interfaces
{
    public interface IQuery
    {
        string[] GetParameterList();

        string GetQuery();

        CommandType QueryType { get; }
    }
}
