﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Framework.IO
{
    public class FileOperator
    {
        private static Lazy<FileOperator> lazyOp =
            new Lazy<FileOperator>(() => new FileOperator());

        private FileOperator()
        {
        }

        public static FileOperator Instance
        { get { return lazyOp.Value; } }

        public void Write(string filePath, List<string> rows, bool writeLine = false)
        {
            try
            {
                string str;
                FileMode fMode = File.Exists(filePath) ? FileMode.Append : FileMode.OpenOrCreate;
                if (!writeLine)
                {
                    using (StreamWriter writer = new StreamWriter(
                               new FileStream(filePath, fMode))
                    { AutoFlush = true })
                    {
                        foreach (string s in rows)
                        {
                            str = s ?? string.Empty;
                            if (str.EndsWith("\r\n") || str.EndsWith("\n"))
                            {
                                writer.Write(str);
                            }
                            else
                            {
                                writer.WriteLine(str);
                            }
                        }

                        writer.Flush();
                    }
                }
                else
                {
                    using (StreamWriter writer = new StreamWriter(
                new FileStream(filePath, fMode))
                    { AutoFlush = true })
                    {
                        foreach (string s in rows)
                        {
                            writer.WriteLine(s ?? string.Empty);
                        }
                        writer.Flush();
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
