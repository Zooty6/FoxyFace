using MySql.Data.MySqlClient;

namespace DatabaseAccess
{
    public class FoxyFaceDbParameter
    {
        public string Key { get; set; }
        public MySqlDbType Type { get; set; }
        public object Value { get; set; }

        public FoxyFaceDbParameter(string key, MySqlDbType type, object value)
        {
            Key = key;
            Type = type;
            Value = value;
        }

        public FoxyFaceDbParameter()
        {
        }

        public override string ToString()
        {
            return $"{nameof(Key)}: {Key}, {nameof(Type)}: {Type}, {nameof(Value)}: {Value}";
        }
    }
}
