using Framework.Common.Types;
using Framework.Configuration.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Framework.Configuration
{
    public static class ConfigurationManager
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string AssemblyDirectoryV2
        {
            get
            {
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                return dir;
            }
        }

        //AppDomain.CurrentDomain.BaseDirectory
        static ConfigurationManager()
        {
            try
            {
                string configFileName = AssemblyDirectoryV2;

                if (configFileName.StartsWith("\\"))
                    configFileName.TrimStart('\\');

                if (configFileName.StartsWith("/"))
                    configFileName.TrimStart('/');

                configFileName = $"{configFileName}/{ConfigurationConstants.confFileName}";

                if (!File.Exists(configFileName))
                    throw new ArgumentException($"{ConfigurationConstants.confFileName} file has not found in directory.");

                XmlNodeList nodeLst = null;

                XmlDocument confDoc = new XmlDocument();
                confDoc.Load(configFileName);

                #region [ Connections ]

                List<FConnectionSetting> connSets = new List<FConnectionSetting>();

                try
                {
                    nodeLst = null;
                    nodeLst = confDoc.SelectNodes(ConfigurationConstants.ConnectionNode);

                    if (ConfXmlHelper.IsValid(nodeLst))
                    {
                        XmlNode subNd;
                        XmlNodeList subNdLst;

                        FConnectionSetting setting;
                        KeyValue keyVal;
                        List<KeyValue> keyVals;// = new List<KeyValue>();
                        List<KeyValue> settings;// = new List<KeyValue>();

                        foreach (XmlNode item in nodeLst)
                        {
                            keyVals = new List<KeyValue>();
                            settings = new List<KeyValue>(); ;
                            if (item == null)
                                continue;

                            setting = new FConnectionSetting();
                            setting.ConnectionName = ConfXmlHelper.TryGetValue(item.Attributes, ConfigurationConstants.name);
                            setting.ConnectionType = ConfXmlHelper.TryGetValue(item.Attributes, ConfigurationConstants.typename);

                            if (!ConfStringHelper.IsValid(setting.ConnectionName) || !ConfStringHelper.IsValid(setting.ConnectionType))
                                continue;

                            setting.ConnectionString = ConfXmlHelper.TryGetSubNodeValue(item, ConfigurationConstants.ConnectionString);
                            setting.ConnectionStringFormat = ConfXmlHelper.TryGetSubNodeValue(item, ConfigurationConstants.ConnectionStringFormat);

                            subNd = item.SelectSingleNode(ConfigurationConstants.FormatKeysNode);

                            setting.FormatKeys = ConfXmlHelper.TryGetValue(subNd.Attributes, ConfigurationConstants.Keys);
                            subNdLst = subNd.SelectNodes(ConfigurationConstants.FormatKey);
                            if (ConfXmlHelper.IsValid(subNdLst))
                            {
                                foreach (XmlNode ndd in subNdLst)
                                {
                                    keyVal = new KeyValue
                                    {
                                        Key = ConfXmlHelper.TryGetValue(ndd.Attributes, ConfigurationConstants.Key),
                                        Value = ConfXmlHelper.TryGetValue(ndd.Attributes, ConfigurationConstants.Value)
                                    };

                                    if (ConfStringHelper.IsValid(keyVal.Key))
                                    {
                                        keyVals.Add(keyVal);
                                    }
                                }
                            }
                            setting.FormatKeyValues = keyVals;

                            subNdLst = null;
                            subNdLst = item.SelectNodes(ConfigurationConstants.ConnSettings);
                            if (ConfXmlHelper.IsValid(subNdLst))
                            {
                                foreach (XmlNode ndd in subNdLst)
                                {
                                    keyVal = new KeyValue
                                    {
                                        Key = ConfXmlHelper.TryGetValue(ndd.Attributes, ConfigurationConstants.Key),
                                        Value = ConfXmlHelper.TryGetValue(ndd.Attributes, ConfigurationConstants.Value)
                                    };

                                    if (ConfStringHelper.IsValid(keyVal.Key))
                                    {
                                        settings.Add(keyVal);
                                    }
                                }
                            }
                            setting.Settings = settings;

                            connSets.Add(setting);
                        }
                    }
                }
                finally
                {
                    ConnectionSettings = connSets;
                }

                #endregion [ Connections ]

                #region [ Assemblies ]

                List<FrameAssembly> assemblies = new List<FrameAssembly>();

                try
                {
                    nodeLst = null;
                    nodeLst = confDoc.SelectNodes(ConfigurationConstants.AssembliesNode);

                    if (ConfXmlHelper.IsValid(nodeLst))
                    {
                        string type_name;
                        string name_space;
                        string class_name;
                        foreach (XmlNode item in nodeLst)
                        {
                            if (item == null)
                                continue;

                            type_name = ConfXmlHelper.TryGetValue(item.Attributes, ConfigurationConstants.NdType);
                            name_space = ConfXmlHelper.TryGetValue(item.Attributes, ConfigurationConstants.NdNamespace);
                            class_name = ConfXmlHelper.TryGetValue(item.Attributes, ConfigurationConstants.NdClass);

                            if (ConfStringHelper.IsValid(type_name) && ConfStringHelper.IsValid(name_space) && ConfStringHelper.IsValid(class_name))
                            {
                                assemblies.Add(new FrameAssembly(type_name, name_space, class_name));
                            }
                        }
                    }
                }
                finally
                {
                    Assemblies = assemblies;
                }

                #endregion [ Assemblies ]
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<FrameAssembly> Assemblies { get; private set; }

        public static List<FConnectionSetting> ConnectionSettings { get; private set; }
    }
}
