using Framework.Data.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Framework.Data.Sql
{
    public class ConnectionSql : ISvcConnection
    {
        public string ConnType { get { return "SqlServer"; } }

        public string ConnectionString { get; set; } = string.Empty;

        public string Owner
        {
            get
            {
                SqlConnectionStringBuilder dbStr = new SqlConnectionStringBuilder();
                dbStr.ConnectionString = this.ConnectionString;
                return dbStr.UserID;
            }
        }

        public DataTable GetData(string sqlText, CommandType cmdType, Dictionary<string, object> parameters = null)
        {
            if (string.IsNullOrWhiteSpace(sqlText))
                throw new ArgumentNullException("sqlText");

            DataTable result = new DataTable();

            try
            {
                using (SqlConnection pConn = new SqlConnection())
                {
                    pConn.ConnectionString = this.ConnectionString;
                    using (SqlCommand Cmd = pConn.CreateCommand())
                    {
                        Cmd.CommandText = sqlText;
                        Cmd.CommandType = cmdType;

                        if (parameters != null && parameters.Count > 0)
                        {
                            foreach (var item in parameters.Keys)
                            {
                                if (!string.IsNullOrWhiteSpace(item))
                                    Cmd.Parameters.AddWithValue(item, parameters[item] ?? DBNull.Value);
                            }
                        }

                        using (SqlDataAdapter adapter = new SqlDataAdapter())
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
                using (SqlConnection pConn = new SqlConnection())
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
