using System;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;

namespace DatabaseAccess
{
    /// <summary>
    /// Represents the logic hot to access MySql data
    /// </summary>
    public class FoxyFaceDB : IDisposable
    {
        private readonly MySqlConnection connection;
        
        public FoxyFaceDB(string connectionString)
        {
            connection = new MySqlConnection(connectionString);
        }

        public void Open()
        {
            connection.Open();
        }

        public bool IsOpen()
        {
            return connection.State == ConnectionState.Open;
        }

        public IDbTransaction BeginTransaction()
        {
            return connection.BeginTransaction();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            connection.Close();
        }

        public DataTable ExecuteReader(string qry, params MySqlParameter[] parameterCollection)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = qry;
                //command.BindByName = true;
                
                AttachParameters(command, parameterCollection);
                MySqlDataReader reader = command.ExecuteReader();

                DataTable ret = new DataTable();
                if (reader.HasRows) {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        ret.Columns.Add(reader.GetName(i));
                    }
                    while(reader.Read()) {
                        DataRow row = ret.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader.GetValue(i);
                        }
                        ret.Rows.Add(row);
                    }
                }
                return ret;
            }
        }
        
        /// <summary>
        /// Executes an SQL statement with the specified binds against the Connection.
        /// </summary>
        /// <param name="qry">The SQL Statement to be executed.</param>
        /// <param name="parameterCollection">The list of bind values to use for execution.</param>
        public long ExecuteNonQuery(string qry, params MySqlParameter[] parameterCollection)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = qry;
                //command.BindByName = true;
                AttachParameters(command, parameterCollection);
                command.ExecuteNonQuery();
                return command.LastInsertedId;
            }
        }

        private static void AttachParameters(MySqlCommand command, MySqlParameter[] parameterCollection)
        {
            if (parameterCollection == null)
                return;

            foreach (var parameter in parameterCollection)
            {
                command.Parameters.Add(parameter);
            }
        }

        public void ExecuteScript(string file)
        {
            string query = File.ReadAllText(file);
            ExecuteNonQuery(query);
        }
    }
}
