using Framework.Configuration;
using Framework.Configuration.Types;
using Framework.Data.Core.Interfaces;
using Framework.Data.Core.Types;
using Framework.IO;
using ObjectSripterWinCA.Source.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ObjectSripterWinCA
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                string fullFileName = string.Empty;
                List<DbObject> objLst = null;
                List<string> scriptLst = null;

                int typeCounter = 0;
                int typeCount = 0;
                int objCounter = 0;
                int objCount = 0;

                //ConfigurationManager
                IDataManager dataMan;
                ISvcConnection pConn = null;
                string saveFolder;
                //ISvcConnection dictionary
                Dictionary<string, ISvcConnection> dict = new Dictionary<string, ISvcConnection>();

                //IDatamanager dictionary
                Dictionary<string, IDataManager> dictMan = new Dictionary<string, IDataManager>();

                Assembly ass;
                string type_name;
                Type[] types;

                foreach (var item in ConfigurationManager.Assemblies)
                {
                    ass = Assembly.Load(item.Namespace);
                    types = ass.GetExportedTypes();
                    foreach (var typ in types)
                    {
                        type_name = item.TypeName.Trim().ToLowerInvariant();
                        if (typ.IsClass && typ.GetInterfaces().Contains(typeof(ISvcConnection))
                            && typ.IsAbstract == false && typeof(ISvcConnection).IsAssignableFrom(typ))
                        {
                            if (string.IsNullOrWhiteSpace(item.TypeName) == false)
                            {
                                dict.Add(type_name, (ISvcConnection)Activator.CreateInstance(typ));
                            }
                            Console.WriteLine($"{typ.FullName} --> Interface List contains ISvcConnection.");
                            Console.WriteLine($"{typ.FullName} Assembly Name --> {typ.Assembly.FullName}.");
                        }

                        if (typ.IsClass && typ.GetInterfaces().Contains(typeof(IDataManager))
                            && typ.IsAbstract == false && typeof(IDataManager).IsAssignableFrom(typ))
                        {
                            if (string.IsNullOrWhiteSpace(item.TypeName) == false)
                            {
                                dictMan.Add(type_name, (IDataManager)Activator.CreateInstance(typ));
                            }
                            Console.WriteLine($"{typ.FullName} --> Interface List contains IDataManager.");
                            Console.WriteLine($"{typ.FullName} Assembly Name --> {typ.Assembly.FullName}.");
                        }
                    }
                }

                WriteLine("---------------------------------------------------");
                WriteLine("Script Operation has been started.");
                foreach (FConnectionSetting item in ConfigurationManager.ConnectionSettings)
                {
                    dataMan = null;
                    pConn = null;

                    if (dict.ContainsKey(item.ConnectionType.Trim().ToLowerInvariant()))
                        pConn = dict[item.ConnectionType.Trim().ToLowerInvariant()];

                    if (dictMan.ContainsKey(item.ConnectionType.Trim().ToLowerInvariant()))
                        dataMan = dictMan[item.ConnectionType.Trim().ToLowerInvariant()];

                    if (dataMan == null || pConn == null)
                    {
                        WriteLine("Connection Or Data Manager is not valid.");
                        continue;
                    }

                    pConn.ConnectionString =
                        string.IsNullOrWhiteSpace(item.ConnectionString) ? item.GetFormattedConnString() : item.ConnectionString;
                    dataMan.Connection = pConn;

                    if (item.WriteConnectionStringToConsole)
                        WriteLine(pConn.ConnectionString);

                    if (item.WriteEventToConsole)
                    {
                        WriteLine(item.ConnectionName);
                        WriteLine(item.ConnectionStringFormat);
                        WriteLine(item.ConnectionType);
                        WriteLine("FormatKeyValues: ");
                        WriteLine("---------------------------------------------------");
                        foreach (var keyVal in item.FormatKeyValues)
                        {
                            WriteLine(keyVal.Key);
                            WriteLine(keyVal.Value);
                            WriteLine("************************");
                        }

                        WriteLine("Settings: ");
                        WriteLine("---------------------------------------------------");
                        foreach (var keyVal in item.Settings)
                        {
                            WriteLine(keyVal.Key);
                            WriteLine(keyVal.Value);
                            WriteLine("************************");
                        }

                        if (item.WriteEventToConsole)
                            WriteLine("------------------------------------------");

                        WriteLine(item.GetFormattedConnString());
                        dataMan.Connection.Test();

                        if (item.WriteEventToConsole)
                            WriteLine("Connection test success.");
                    }
                    saveFolder = GetSaveFolderPath(item.SaveFolder);
                    foreach (var typ in item.ObjectTypes)
                    {
                        var typName = typ.ToUpperInvariant();
                        try
                        {
                            objCount = 0;
                            objCounter = 0;
                            typeCounter++;
                            if (item.WriteEventToConsole)
                            {
                                WriteLine($"----------------{typName} TYPE START ({typeCounter}/{typeCount})----------------------------");
                                WriteLine($"Object List with {typName} type will be scripted.");
                            }

                            objLst = null;

                            #region [ Getting Object List ]

                            objLst = dataMan.GetObjects(typName);
                            if (dataMan.GetObjectsError != null)
                            {
                                WriteLine($"Object List Error with {typName} type will be writed.");
                                LogException(dataMan.GetObjectsError);
                            }

                            #endregion

                            objCount = objLst.Count;

                            if (objCount > 0)
                            {
                                objCounter = 0;
                                foreach (var obj in objLst)
                                {
                                    objCounter++;

                                    if (item.WriteEventToConsole)
                                    {
                                        WriteLine($"------------{obj.TYPENAME} {obj.OWNER}.{obj.NAME} -- START ({objCounter}/{objCount})--------------");
                                        WriteLine($"{obj.TYPENAME} {obj.OWNER}.{obj.NAME} object is being writed.");
                                    }

                                    #region [ Getting Metadata Script ]

                                    scriptLst = null;

                                    // Getting Metadata Script Text List Of Object.
                                    try
                                    {
                                        scriptLst = new List<string>();
                                        scriptLst = dataMan.GetScriptOfObject(obj);
                                    }
                                    catch (Exception ex2)
                                    {
                                        LogException(ex2);
                                    }

                                    if (dataMan.GetScriptOfObjectError != null)
                                    {
                                        WriteLine($"Object List Error with {obj.ToString()} type will be writed.");
                                        LogException(dataMan.GetScriptOfObjectError);
                                    }

                                    #endregion

                                    if (item.WriteEventToConsole)
                                    {
                                        WriteLine("Script has been taken.");
                                        WriteLine("Script is being written to file.");
                                    }

                                    //Write Script Content to Console.
                                    if (item.WriteScriptToConsole)
                                    {
                                        scriptLst.ForEach(st => WriteWithCheck(st));
                                    }

                                    //File Logging
                                    try
                                    {
                                        if (!Directory.Exists(saveFolder + obj.TYPENAME + "/"))
                                        { Directory.CreateDirectory(saveFolder + obj.TYPENAME + "/"); }

                                        fullFileName = $"{saveFolder}{obj.TYPENAME}/{obj.OWNER}.{obj.NAME}_METADATA.sql";
                                        int cc = 2;
                                        while (File.Exists(fullFileName))
                                        {
                                            fullFileName = $"{saveFolder}{obj.TYPENAME}/{obj.OWNER}.{obj.NAME}_METADATA_{cc}.sql";
                                            cc++;
                                        }

                                        FileOperator.Instance.Write(fullFileName, scriptLst);

                                        if (item.WriteEventToConsole)
                                            WriteLine("Script has been written to file.");
                                    }
                                    catch (Exception ex4)
                                    {
                                        LogException(ex4);
                                    }

                                    if (item.WriteEventToConsole)
                                        WriteLine($"------------{obj.TYPENAME} {obj.OWNER}.{obj.NAME} -- END   ({objCounter}/{objCount})--------------");
                                }
                            }
                            else
                            {
                                if (item.WriteEventToConsole)
                                    WriteLine($"No objects found with {typName} Type.");
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException(ex);
                        }

                        if (item.WriteEventToConsole)
                            WriteLine($"----------------{typName} TYPE END   ({typeCounter}/{typeCount})----------------------------");
                    }

                    WriteLine("Script Operation has been finished.");
                }
            }
            catch (Exception e)
            {
                LogException(e);
            }

            Console.ReadKey();
        }

        private static void LogException(Exception e)
        {
            WriteLine($"Message: {e.Message}");
            WriteLine($"StackTrace: {e.StackTrace}");
        }

        public static string AssemblyDirectoryV2
        {
            get
            {
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                return dir;
            }
        }

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

        private static void WriteWithCheck(string s = null)
        {
            string ss = s ?? string.Empty;

            if (ss.EndsWith("\r\n") || ss.EndsWith("\n"))
            {
                Write(ss);
            }
            else
            {
                WriteLine(ss);
            }
        }

        private static void WriteLine(string s = null)
        { Console.WriteLine(s); }

        private static void Write(string s = null)
        { Console.Write(s); }

        private static string GetSaveFolderPath(string saveFolderPath)
        {
            string saveFolder = saveFolderPath ?? saveFolderPath;
            //saveFolder = AppValues.SaveFolder;
            if (saveFolder.StartsWith(".") || saveFolder.StartsWith("\\") || saveFolder.StartsWith("/"))
            {
                saveFolder = saveFolder.TrimStart('.').TrimStart('\\').TrimStart('/');
                saveFolder = AssemblyDirectoryV2 + saveFolder;
            }

            if (!saveFolder.EndsWith("\\") && !saveFolder.EndsWith("/"))
                saveFolder += "/";

            saveFolder += AppValues.SaveFolderName +
                //$"_{DataManager.Instance.Connection.Owner.ToUpperInvariant()}" +
                DateTime.Now.ToString(AppValues.DateFormat) + "/";

            return saveFolder;
        }
    }
}
