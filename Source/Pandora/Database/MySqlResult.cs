using System.Data;

namespace Pandora.Database
{
    public class MySqlResult : DataTable
    {
        public uint Count { get; set; }

        public T Read<T>(uint row, string columnName)
        {
            return Rows[(int)row].Read<T>(columnName);
        }
    }
}
