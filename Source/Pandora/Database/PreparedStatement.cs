using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Pandora.Database
{
    public class PreparedStatement
    {
        public WorldPreparedStatement Id { get; }
        public string Query { get; }
        public List<MySqlDbType> Types { get; } = new List<MySqlDbType>();

        public PreparedStatement(WorldPreparedStatement id, string query, params MySqlDbType[] types)
        {
            Id    = id;
            Query = query;
            Types.AddRange(types);
        }
    }
}
