using Framework.Common.Interfaces;
using Framework.Common.Types;
using System;
using System.Collections.Generic;
using Framework.Common.Extensions;

namespace Framework.Common.Factory
{
    internal class CoreDictionary : ICoreDictionary
    {
        private Dictionary<string, ICoreDictionary> dicts = null;
        private Dictionary<string, string> keyVals = null;

        public CoreDictionary()
        {
            dicts = new Dictionary<string, ICoreDictionary>();
            keyVals = new Dictionary<string, string>();
        }

        public string Name { get; set; }

        public string this[int index]
        {
            get
            {
                string[] ks = Keys;
                string ss = string.Empty;
                if (string.IsNullOrWhiteSpace(ks[index]))
                    return ss;

                ss = keyVals[ks[index]] ?? string.Empty;

                return ss;
            }
            set
            {
                string[] ks = Keys;
                if (string.IsNullOrWhiteSpace(ks[index]))
                    return;

                keyVals[ks[index]] = value ?? string.Empty;
            }
        }

        public string this[string str]
        {
            get
            {
                string ss = string.Empty;
                if (string.IsNullOrWhiteSpace(str))
                    return ss;

                ss = keyVals[str] ?? string.Empty;

                return ss;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(str))
                    return;

                keyVals[str] = value ?? string.Empty;
            }
        }

        public Dictionary<string, ICoreDictionary> Dictionary { get { return dicts; } }

        public ICoreDictionary[] DictList
        {
            get
            {
                ICoreDictionary[] dictlst = new ICoreDictionary[Dictionary.Keys.Count];
                int ind = 0;
                foreach (var key in Dictionary.Keys)
                {
                    dictlst[ind++] = Dictionary[key];
                }

                return dictlst;
            }
        }

        public string[] Keys
        {
            get
            {
                string[] _keys = new string[keyVals.Count];
                keyVals.Keys.CopyTo(_keys, 0);
                return _keys;
            }
        }

        public ICoreDictionary Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return dicts[name];
        }

        public ICoreDictionary Get(int index)
        {
            return DictList[index];
        }

        public void Set(ICoreDictionary dc)
        {
            if (dc == null)
                return;

            if (string.IsNullOrWhiteSpace(dc.Name))
                return;

            dicts[dc.Name] = dc;
        }

        public List<KeyValue> GetKeyValuelistFrom(string key, string value)
        {
            if (key.IsNotValid() || value.IsNotValid())
                throw new ArgumentException("Check arguments. All arguments are not valid.");
            /*
            if (this.Keys.AsQueryable().Where(k => k == key).ToList().Count == 0
                || this.Keys.AsQueryable().Where(k => k == value).ToList().Count == 0)
                throw new ArgumentException("Check dictionary. dictionary does not have all arguments.");
            */
            List<KeyValue> lst = new List<KeyValue>();

            foreach (var k in DictList)
            {
                lst.Add(new KeyValue { Key = k[key], Value = k[value] ?? string.Empty });
            }

            return lst;
        }

        public List<KeyValue> GetKeyValuelistFromDict(string key, string value)
        {
            if (key.IsNotValid() || value.IsNotValid())
                throw new ArgumentException("Check arguments. All arguments are not valid.");

            List<KeyValue> lst = new List<KeyValue>();
            List<KeyValue> tmpLst = new List<KeyValue>();

            foreach (var k in DictList)
            {
                tmpLst = k.GetKeyValuelistFrom(key, value);

                lst.AddRange(tmpLst.ToArray());
            }

            return lst;
        }
    }
}
