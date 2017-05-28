using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectSripterWinSvc.Source.Interfaces
{
    interface IQuery
    {
        string[] GetParameterList();

        string GetQuery();

        CommandType QueryType { get; }
    }
}
