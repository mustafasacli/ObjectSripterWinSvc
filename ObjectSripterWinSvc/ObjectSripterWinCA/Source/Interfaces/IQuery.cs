using System.Data;

namespace ObjectSripterWinCA.Source.Interfaces
{
    internal interface IQuery
    {
        string[] GetParameterList();

        string GetQuery();

        CommandType QueryType { get; }
    }
}
