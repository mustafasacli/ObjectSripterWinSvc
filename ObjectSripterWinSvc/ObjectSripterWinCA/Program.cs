using Framework.Data.Core;
using Framework.Data.Oracle;
using Framework.IO;
using ObjectSripterWinCA.Source.Manager;
using ObjectSripterWinCA.Source.Values;
using ObjectSripterWinSvc.Source.Queries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ObjectSripterWinCA
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                //string ConnStr = ConfigurationManager.AppSettings["ConnStr"];
                ConnectionOracle pConn = new ConnectionOracle();
                pConn.ConnectionString = AppValues.ConnStr;
                DataManager.Instance.Connection = pConn;
                DataManager.Instance.Connection.Test();
                WriteLine("Connection test success.");
                WriteLine(AppValues.SaveFolder);
                //ConfigurationManager.AppSettings["SaveFolder"]);
                string format = AppValues.DateFormat;//ConfigurationManager.AppSettings["DateFormat"];
                WriteLine(format);
                string s = DateTime.Now.ToString(format);
                WriteLine(s);
                WriteLine();
                WriteLine(AppValues.ConnStr);

                string types = AppValues.Types;
                WriteLine($"{types} Type list will be scripted.");
                string[] typeList = types.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                typeList = typeList ?? new string[] { };
                List<DbObject> objLst = null;
                List<string> scriptLst = null;

                string saveFolder = AppValues.SaveFolder;
                if (saveFolder.StartsWith(".") || saveFolder.StartsWith("\\") || saveFolder.StartsWith("/"))
                {
                    saveFolder = saveFolder.TrimStart('.').TrimStart('\\').TrimStart('/');
                    saveFolder = AssemblyDirectoryV2 + saveFolder;
                }

                if (!saveFolder.EndsWith("\\") && !saveFolder.EndsWith("/"))
                    saveFolder += "/";

                saveFolder += AppValues.SaveFolderName + DateTime.Now.ToString(AppValues.DateFormat) + "/";
                string fullFileName = string.Empty;

                foreach (var item in typeList)
                {
                    try
                    {
                        WriteLine($"----------------{item} TYPE START ----------------------------");
                        //WriteLine($"{item} Type Script is getting.");
                        WriteLine($"Object List with {item} type will be scripted.");
                        objLst = null;
                        objLst = DataManager.Instance.GetObjects(item);

                        if (objLst.Count > 0)
                        {
                            foreach (var obj in objLst)
                            {
                                WriteLine($"------------{obj.TYPENAME} {obj.OWNER}.{obj.NAME} -- START --------------");
                                WriteLine($"{obj.TYPENAME} {obj.OWNER}.{obj.NAME} object is being writed.");
                                //WriteLine(obj.ToString());
                                scriptLst = null;
                                // Getting Metadata Script Text List Of Object.
                                try
                                {
                                    scriptLst = new List<string>();
                                    scriptLst = DataManager.Instance.GetScriptOfObject(obj, new OracleScriptListV2());
                                }
                                catch (Exception ex2)
                                {
                                    LogException(ex2);
                                }
                                //if metadata script of object has not been found, this will work.
                                if (scriptLst.Count == 0)
                                {
                                    // Getting Metadata Script Text List Of Object with another version.
                                    try
                                    {
                                        scriptLst = new List<string>();
                                        scriptLst = DataManager.Instance.GetScriptOfObject(obj, new OracleScriptList());
                                    }
                                    catch (Exception ex3)
                                    {
                                        LogException(ex3);
                                    }
                                }

                                WriteLine("Script has been taken.");
                                WriteLine("Script is being written to file.");

                                //Write Script Content to Console.
                                if (AppValues.WriteScriptToConsole)
                                {
                                    scriptLst.ForEach(st => WriteWithCheck(st));
                                }

                                //File Logging
                                try
                                {
                                    if (!Directory.Exists(saveFolder + obj.TYPENAME + "/"))
                                    { Directory.CreateDirectory(saveFolder + obj.TYPENAME + "/"); }

                                    //fullFileName = $"{saveFolder}{obj.TYPENAME}/{obj.TYPENAME}_{obj.OWNER}.{obj.NAME}_METADATA.sql";
                                    fullFileName = $"{saveFolder}{obj.TYPENAME}/{obj.OWNER}.{obj.NAME}_METADATA.sql";
                                    int cc = 2;
                                    while (File.Exists(fullFileName))
                                    {
                                        fullFileName = $"{saveFolder}{obj.TYPENAME}/{obj.OWNER}.{obj.NAME}_METADATA_{cc}.sql";
                                        //fullFileName = $"{saveFolder}{obj.TYPENAME}/{obj.TYPENAME}_{obj.OWNER}.{obj.NAME}_METADATA_{cc}.sql";
                                        cc++;
                                    }

                                    FileOperator.Instance.Write(fullFileName, scriptLst);
                                    WriteLine("Script has been written to file.");
                                }
                                catch (Exception ex4)
                                {
                                    LogException(ex4);
                                }

                                WriteLine($"------------{obj.TYPENAME} {obj.OWNER}.{obj.NAME} -- END   --------------");
                            }
                        }
                        else
                        {
                            WriteLine($"No objects found with {item} Type.");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogException(ex);
                    }

                    WriteLine($"----------------{item} TYPE END   ----------------------------");
                }

                WriteLine("Script Operation has been finished.");
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
    }
}
