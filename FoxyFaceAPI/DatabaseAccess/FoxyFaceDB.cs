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
        public string ConnectionString { get; private set; }

        public FoxyFaceDB(string connectionString)
        {
            connection = new MySqlConnection {
                ConnectionString = connectionString
            };
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

        public void Close()
        {
            Dispose();
        }

        public DataTable ExecuteReader(string qry)
        {
            return ExecuteReader(qry, new FoxyFaceDbParameterCollection());
        }

        public DataTable ExecuteReader(string qry, FoxyFaceDbParameterCollection parameterCollection)
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
        /// Executes an SQL statement with the specified binds against the Connection and returns the number of rows affected.
        /// </summary>
        /// <param name="qry">The SQL Statement to be executed.</param>
        public void ExecuteNonQuery(string qry)
        {
            ExecuteNonQuery(qry, new FoxyFaceDbParameterCollection());
        }

        /// <summary>
        /// Executes an SQL statement with the specified binds against the Connection.
        /// </summary>
        /// <param name="qry">The SQL Statement to be executed.</param>
        /// <param name="parameterCollection">The list of bind values to use for execution.</param>
        public void ExecuteNonQuery(string qry, FoxyFaceDbParameterCollection parameterCollection)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = qry;
                //command.BindByName = true;
                AttachParameters(command, parameterCollection);
                command.ExecuteNonQuery();
            }
        }
        
        /// <summary>
        /// Attaches new MySqlParameters to the given Collection using the parameterArrays Values.
        /// </summary>
        /// <param name="mySqlParameters"></param>
        /// <param name="parameterArray"></param>
        private static void AttachParametersNative(MySqlParameterCollection mySqlParameters, object[] parameterArray)
        {
            for (int i = 0; i < parameterArray.Length; i++)
            {
                if (parameterArray[i] == null)
                {
                    parameterArray[i] = string.Empty;
                }
                MySqlParameter param = new MySqlParameter("P_" + i,
                    GetNativeParameterType(parameterArray[i].GetType(), parameterArray[i]))
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = parameterArray[i]
                };
                mySqlParameters.Add(param);
            }
        }

        /// <summary>
        /// Determines the matching MySqlDbType for a given dotnet-type.
        /// </summary>
        /// <param name="dotNetType">The .net type to determine a MySqlDbType for.</param>
        /// <param name="value">The value to decide varchar or clob</param>
        /// <returns>The matching MySqlDbType.</returns>
        private static MySqlDbType GetNativeParameterType(Type dotNetType, object value)
        {
            if (Nullable.GetUnderlyingType(dotNetType) != null)
                dotNetType = Nullable.GetUnderlyingType(dotNetType);
            if (dotNetType == typeof(int))
                return MySqlDbType.Int32;
            if (dotNetType == typeof(string))
                return value.ToString().Length <= 4000 ? MySqlDbType.VarChar : MySqlDbType.Text;
            if (dotNetType == typeof(double))
                return MySqlDbType.Double;
            if (dotNetType == typeof(float))
                return MySqlDbType.Int64;
            if (dotNetType == typeof(DateTime))
                return MySqlDbType.Date;
            if (dotNetType == typeof(DateTimeOffset))
                return MySqlDbType.Timestamp;
            if (dotNetType == typeof(byte[]))
                return MySqlDbType.Blob;
            throw new ApplicationException("Unsupported Datatype! [GetNativeParameterType]");
        }

        /// <summary>
        /// Sets the to-parameters fields using the ParameterCollections values.
        /// </summary>
        /// <param name="from">ParameterCollection containing the values to set.</param>
        /// <param name="to">Array to be set.</param>
        private static void WriteBackParameterValues(MySqlParameterCollection from, object[] to)
        {
            for (int i = 0; i < to.Length; i++)
            {
                if (from[i].Value == DBNull.Value)
                    to[i] = null;
                else
                    to[i] = from[i].Value;
            }
        }

        private static void AttachParameters(MySqlCommand command, FoxyFaceDbParameterCollection parameterCollection)
        {
            if (parameterCollection == null)
                return;

            foreach (FoxyFaceDbParameter parameter in parameterCollection.Parameters)
            {
                command.Parameters.Add(parameter.Key, parameter.Type).Value = parameter.Value;
            }
        }

        public void ExecuteScript(string file)
        {
            string query = File.ReadAllText(file);
            ExecuteNonQuery(query);
        }
    }
}
