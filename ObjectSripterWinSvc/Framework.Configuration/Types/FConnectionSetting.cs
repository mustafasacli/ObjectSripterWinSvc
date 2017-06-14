using Framework.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Configuration.Types
{
    public class FConnectionSetting
    {
        public string ConnectionName { get; internal set; }

        public string ConnectionType { get; internal set; }

        public string ConnectionString { get; internal set; }

        public string ConnectionStringFormat { get; internal set; }

        public string FormatKeys { get; internal set; }

        public List<KeyValue> FormatKeyValues { get; internal set; }

        public List<KeyValue> Settings { get; internal set; }

        public bool WriteErrorToLog
        {
            get
            {
                bool rs = false;
                KeyValue kv = Settings.AsQueryable().Where(k => k.Key == ConfigDefaults.WriteErrorToLog).SingleOrDefault();
                if (kv != null)
                {
                    if (ConfStringHelper.IsValid(kv.Key))
                        rs = kv.Value.Trim() == ConfigDefaultValues.WriteErrorToLog;
                }

                return rs;
            }
        }

        public bool WriteConnectionStringToConsole
        {
            get
            {
                bool rs = true;
                KeyValue kv = Settings.AsQueryable().Where(k => k.Key == ConfigDefaults.WriteConnectionStringToConsole).SingleOrDefault();
                if (kv != null)
                {
                    if (ConfStringHelper.IsValid(kv.Key))
                        rs = kv.Value.Trim() == ConfigDefaultValues.WriteConnectionStringToConsole;
                }

                return rs;
            }
        }

        public bool WriteScriptToConsole
        {
            get
            {
                bool rs = false;
                KeyValue kv = Settings.AsQueryable().Where(k => k.Key == ConfigDefaults.WriteScriptToConsole).SingleOrDefault();
                if (kv != null)
                {
                    if (ConfStringHelper.IsValid(kv.Key))
                        rs = kv.Value.Trim() == ConfigDefaultValues.WriteScriptToConsole;
                }

                return rs;
            }
        }

        public bool WriteEventToLog
        {
            get
            {
                bool rs = false;
                KeyValue kv = Settings.AsQueryable().Where(k => k.Key == ConfigDefaults.WriteEventToLog).SingleOrDefault();
                if (kv != null)
                {
                    if (ConfStringHelper.IsValid(kv.Key))
                        rs = kv.Value.Trim() == ConfigDefaultValues.WriteEventToLog;
                }

                return rs;
            }
        }

        public bool WriteEventToConsole
        {
            get
            {
                bool rs = false;
                KeyValue kv = Settings.AsQueryable().Where(k => k.Key == ConfigDefaults.WriteEventToConsole).SingleOrDefault();
                if (kv != null)
                {
                    if (ConfStringHelper.IsValid(kv.Key))
                        rs = kv.Value.Trim() == ConfigDefaultValues.WriteEventToConsole;
                }

                return rs;
            }
        }

        public string ErrorLogFile
        {
            get
            {
                string rs = ConfigDefaultValues.ErrorLogFile;
                KeyValue kv = Settings.AsQueryable().Where(k => k.Key == ConfigDefaults.ErrorLogFile).SingleOrDefault();
                if (kv != null)
                {
                    if (ConfStringHelper.IsValid(kv.Key))
                        rs = kv.Value.Trim();
                }

                return rs;
            }
        }

        public string EventLogFile
        {
            get
            {
                string rs = ConfigDefaultValues.EventLogFile;
                KeyValue kv = Settings.AsQueryable().Where(k => k.Key == ConfigDefaults.EventLogFile).SingleOrDefault();
                if (kv != null)
                {
                    if (ConfStringHelper.IsValid(kv.Key))
                        rs = kv.Value.Trim();
                }

                return rs;
            }
        }

        public string[] ObjectTypes
        {
            get
            {
                string[] arr = null;

                string rs = ConfigDefaultValues.ObjectTypes;
                KeyValue kv = Settings.AsQueryable().Where(k => k.Key == ConfigDefaults.ObjectTypes).SingleOrDefault();
                if (kv != null)
                {
                    if (ConfStringHelper.IsValid(kv.Key))
                        rs = kv.Value.Trim();
                }

                arr = rs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                arr = arr ?? new string[] { };

                return arr;
            }
        }

        public string GetFormattedConnString()
        {
            string rs = string.Empty;

            try
            {
                rs = this.ConnectionStringFormat ?? string.Empty;
                string formatKeys = this.FormatKeys ?? string.Empty;
                string[] formatKeyArr = formatKeys.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                formatKeyArr = formatKeyArr ?? new string[] { };
                List<string> formatKeyList = formatKeyArr.ToList();

                List<KeyValue> keyVals = this.FormatKeyValues;
                keyVals = keyVals.AsQueryable().Where(s => string.IsNullOrWhiteSpace(s.Key) == false).ToList();
                string st;
                keyVals.ForEach(kv =>
                {
                    if (formatKeyList.IndexOf(kv.Key) > -1 && !string.IsNullOrWhiteSpace(kv.Key))
                    {
                        st = kv.Key;
                        if (!st.EndsWith("#"))
                        { st += "#"; }

                        if (!st.StartsWith("#"))
                        { st = "#" + st; }

                        rs = rs.Replace(st, kv.Value);
                    }
                });

            }
            catch (Exception e)
            { throw; }

            return rs;
        }

        public string SaveFolder
        {
            get
            {
                string rs = string.Empty;
                KeyValue kv = Settings.AsQueryable().Where(k => k.Key == ConfigDefaults.SaveFolder).SingleOrDefault();
                if (kv != null)
                {
                    if (ConfStringHelper.IsValid(kv.Key))
                        rs = kv.Value.Trim();
                }

                return rs;
            }
        }
    }
}
