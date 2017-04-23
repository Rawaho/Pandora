using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Pandora.Managers;

namespace Pandora.Database
{
    public static class DatabaseManager
    {
        public static DataBase DataBase { get; private set; }

        public static void Initialise()
        {
            DataBase = new DataBase();
        }
    }

    public partial class DataBase
    {
        private readonly string connectionString;
        private readonly Dictionary<WorldPreparedStatement, PreparedStatement> preparedStatements = new Dictionary<WorldPreparedStatement, PreparedStatement>();

        public DataBase()
        {
            var connectionBuilder = new MySqlConnectionStringBuilder
            {
                Server        = ConfigManager.Config.Database.Host,
                Port          = ConfigManager.Config.Database.Port,
                UserID        = ConfigManager.Config.Database.Username,
                Password      = ConfigManager.Config.Database.Password,
                Database      = ConfigManager.Config.Database.Database,
                IgnorePrepare = false,
                Pooling       = true
            };

            connectionString = connectionBuilder.ToString();
            using (var connection = new MySqlConnection(connectionString))
                connection.Open();

            InitialisePreparedStatements();

            LogManager.Write("Database", "Successfully connected to database!");
        }

        private void AddPreparedStatement(WorldPreparedStatement id, string query, params MySqlDbType[] types)
        {
            Debug.Assert(types.Length == query.Count(c => c == '?'));
            Debug.Assert(query.Length != 0);

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        for (uint i = 0; i < types.Length; i++)
                            command.Parameters.Add("", types[i]);
                        command.Prepare();

                        preparedStatements.Add(id, new PreparedStatement(id, query, types));
                    }
                }
            }
            catch (Exception exception)
            {
                LogManager.Write("Database", $"An exception occured while preparing statement {id}!");
                LogManager.Write("Database", $"Exception: {exception.Message}");

                Debug.Assert(false);
            }
        }

        private async Task<int> ExecutePreparedStatementAsync(WorldPreparedStatement id, params object[] parameters)
        {
            PreparedStatement preparedStatement;
            if (!preparedStatements.TryGetValue(id, out preparedStatement))
            {
                Debug.Assert(preparedStatement != null);
                return -1;
            }

            Debug.Assert(preparedStatement.Types.Count == parameters.Length);

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(preparedStatement.Query, connection))
                    {
                        for (int i = 0; i < preparedStatement.Types.Count; i++)
                            command.Parameters.Add("", preparedStatement.Types[i]).Value = parameters[i];

                        await connection.OpenAsync();
                        return await Task.Run(() => command.ExecuteNonQuery()); // by default ExecuteNonQueryAsync is blocking
                    }
                }
            }
            catch (MySqlException exception)
            {
                LogManager.Write("Database", $"An exception occured while executing prepared statement {id}!");
                LogManager.Write("Database", $"Exception: {exception.Message}");
            }

            return -1;
        }

        private MySqlResult SelectPreparedStatement(WorldPreparedStatement id, params object[] parameters)
        {
            PreparedStatement preparedStatement;
            if (!preparedStatements.TryGetValue(id, out preparedStatement))
            {
                Debug.Assert(preparedStatement != null);
                return null;
            }

            Debug.Assert(preparedStatement.Types.Count == parameters.Length);

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(preparedStatement.Query, connection))
                    {
                        for (int i = 0; i < preparedStatement.Types.Count; i++)
                            command.Parameters.Add("", preparedStatement.Types[i]).Value = parameters[i];

                        connection.Open();
                        using (MySqlDataReader commandReader = command.ExecuteReader(CommandBehavior.Default))
                        {
                            using (var result = new MySqlResult())
                            {
                                result.Load(commandReader);
                                result.Count = (uint)result.Rows.Count;
                                return result;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogManager.Write("Database", $"An exception occured while selecting prepared statement {id}!");
                LogManager.Write("Database", $"Exception: {exception.Message}");
            }

            return null;
        }

        private async Task<MySqlResult> SelectPreparedStatementAsync(WorldPreparedStatement id, params object[] parameters)
        {
            PreparedStatement preparedStatement;
            if (!preparedStatements.TryGetValue(id, out preparedStatement))
            {
                Debug.Assert(preparedStatement != null);
                return null;
            }

            Debug.Assert(preparedStatement.Types.Count == parameters.Length);

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(preparedStatement.Query, connection))
                    {
                        for (int i = 0; i < preparedStatement.Types.Count; i++)
                            command.Parameters.Add("", preparedStatement.Types[i]).Value = parameters[i];

                        await connection.OpenAsync();
                        return await Task.Run(() =>
                        {
                            using (MySqlDataReader commandReader = command.ExecuteReader(CommandBehavior.Default))
                            {
                                using (var result = new MySqlResult())
                                {
                                    result.Load(commandReader);
                                    result.Count = (uint)result.Rows.Count;
                                    return result;
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception exception)
            {
                LogManager.Write("Database", $"An exception occured while selecting prepared statement {id}!");
                LogManager.Write("Database", $"Exception: {exception.Message}");
            }

            return null;
        }
    }
}
