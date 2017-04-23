using MySql.Data.MySqlClient;

namespace Pandora.Database
{
    public enum WorldPreparedStatement
    {
        AccountInsert,
        AccountMax,
        AccountSelect,
        AccountAuthSelect,
        AccountTokenUpdate
    }

    public partial class DataBase
    {
        private void InitialisePreparedStatements()
        {
            AddPreparedStatement(WorldPreparedStatement.AccountInsert, "INSERT INTO `account` (`id`, `username`, `password`) VALUES (?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.VarChar, MySqlDbType.VarChar);
            AddPreparedStatement(WorldPreparedStatement.AccountMax, "SELECT MAX(`id`) FROM `account`;");
            AddPreparedStatement(WorldPreparedStatement.AccountSelect, "SELECT `id`, `username`, `level`, `xp`, `coins`, `gems`, `flags` FROM `account` WHERE `username` = ? AND `token` = ?;", MySqlDbType.VarChar, MySqlDbType.VarChar);
            AddPreparedStatement(WorldPreparedStatement.AccountAuthSelect, "SELECT `id`, `username` FROM `account` WHERE `username` = ? AND `password` = ?;", MySqlDbType.VarChar, MySqlDbType.VarChar);
            AddPreparedStatement(WorldPreparedStatement.AccountTokenUpdate, "UPDATE `account` SET `token` = ? WHERE `id` = ?;", MySqlDbType.VarChar, MySqlDbType.UInt32);
        }
    }
}
