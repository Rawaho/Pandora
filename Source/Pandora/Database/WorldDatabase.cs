using System.Data;
using System.Threading.Tasks;
using Pandora.Game;
using Pandora.Managers;

namespace Pandora.Database
{
    public partial class DataBase
    {
        public async Task<int> CreateAccount(string username, string password)
        {
            return await ExecutePreparedStatementAsync(WorldPreparedStatement.AccountInsert, AssetManager.GetNewAccountId(), username, password);
        }

        public async Task<DataRow> GetAccountAuth(string username, string password)
        {
            MySqlResult result = await SelectPreparedStatementAsync(WorldPreparedStatement.AccountAuthSelect, username, password);
            return result.Count == 0u ? null : result.Rows[0];
        }

        public async Task<Account> GetAccount(string username, string authToken)
        {
            MySqlResult result = await SelectPreparedStatementAsync(WorldPreparedStatement.AccountSelect, username, authToken);
            return result.Count == 0u ? null : new Account(result.Rows[0]);
        }

        public async Task AccountTokenUpdate(string token, uint id)
        {
            await ExecutePreparedStatementAsync(WorldPreparedStatement.AccountTokenUpdate, token, id);
        }

        public uint AccountMax()
        {
            MySqlResult result = SelectPreparedStatement(WorldPreparedStatement.AccountMax);
            return result.Read<uint>(0u, "MAX(`id`)") + 1u;
        }
    }
}
