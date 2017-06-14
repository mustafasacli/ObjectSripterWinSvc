using Framework.Common.Types;
using System.Collections.Generic;

namespace Framework.Common.Interfaces
{
    public interface ICoreDictionary
    {
        string Name { get; set; }

        string this[int index] { get; set; }

        string this[string str] { get; set; }

        Dictionary<string, ICoreDictionary> Dictionary { get; }

        ICoreDictionary[] DictList { get; }

        string[] Keys { get; }

        ICoreDictionary Get(string name);

        ICoreDictionary Get(int index);

        void Set(ICoreDictionary dc);

        List<KeyValue> GetKeyValuelistFrom(string key, string value);

        List<KeyValue> GetKeyValuelistFromDict(string key, string value);
    }
}
