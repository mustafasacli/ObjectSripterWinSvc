using Framework.Data.Core.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Framework.Data.Oracle.Connection
{
    public class ConnectionOracle : ISvcConnection
    {
        public string ConnType { get { return "oracle"; } }

        public string ConnectionString { get; set; } = string.Empty;

        public string Owner
        {
            get
            {
                OracleConnectionStringBuilder dbStr = new OracleConnectionStringBuilder();
                dbStr.ConnectionString = this.ConnectionString;
                return dbStr.UserID.ToUpperInvariant();
            }
        }

        public DataTable GetData(string sqlText, CommandType cmdType, Dictionary<string, object> parameters = null)
        {
            if (string.IsNullOrWhiteSpace(sqlText))
                throw new ArgumentNullException("sqlText");

            DataTable result = new DataTable();

            try
            {
                using (OracleConnection pConn = new OracleConnection())
                {
                    pConn.ConnectionString = this.ConnectionString;
                    using (OracleCommand Cmd = pConn.CreateCommand())
                    {
                        Cmd.CommandText = sqlText;
                        Cmd.CommandType = cmdType;
                        Cmd.BindByName = true;

                        if (parameters != null && parameters.Count > 0)
                        {
                            foreach (var item in parameters.Keys)
                            {
                                if (!string.IsNullOrWhiteSpace(item))
                                    Cmd.Parameters.Add(item, parameters[item] ?? DBNull.Value);
                            }
                        }

                        using (OracleDataAdapter adapter = new OracleDataAdapter())
                        {
                            adapter.SelectCommand = Cmd;
                            adapter.Fill(result);
                        }

                        Cmd.Parameters.Clear();
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return result;
        }

        public void Test()
        {
            try
            {
                using (OracleConnection pConn = new OracleConnection())
                {
                    pConn.ConnectionString = this.ConnectionString;
                    pConn.Open();
                    pConn.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
