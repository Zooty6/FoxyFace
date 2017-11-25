using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DatabaseAccess
{

    /// <summary>
    /// Example:
    /// const string qry = "select distinct useraccount_id, useralias, " +
    ///                                      "firstname || ' ' || lastname name, foregroundcolor, backgroundcolor " +
    ///                                 "from shiftschedule_v s where daydate between :bFrom and :bTo";
    ///
    ///            ScoteDbParameterCollection parameters = new ScoteDbParameterCollection();
    ///            parameters.Add(":bFrom", OracleDbType.Date, from);
    ///            parameters.Add(":bTo", OracleDbType.Date, to);
    ///
    ///            DataTable table = database.ExecuteReader(qry, parameters);
    /// </summary>
    public class FoxyFaceDbParameterCollection
    {
        public List<FoxyFaceDbParameter> Parameters { get; set; }

        public FoxyFaceDbParameterCollection()
        {
            Parameters = new List<FoxyFaceDbParameter>();
        }

        public void Add(string key, MySqlDbType type, object value)
        {
            Parameters.Add(new FoxyFaceDbParameter { Key = key, Type = type, Value = value });
        }

        public void Add(FoxyFaceDbParameter partColumnParameter)
        {
            Parameters.Add(partColumnParameter);
        }

        public override string ToString()
        {
            return $"{nameof(Parameters)}: {string.Join(", ", Parameters)}";
        }
    }
}
