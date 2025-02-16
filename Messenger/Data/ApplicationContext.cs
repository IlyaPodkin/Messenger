using Dapper;
using Npgsql;

namespace Messenger.Data
{
    public class ApplicationContext
    {
        private readonly string _connectionString;
        public ApplicationContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task CreateMessagesTableAsync()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var createTableQuery =
                    @"CREATE TABLE IF NOT EXISTS messages (
                        id SERIAL PRIMARY KEY,
                        user_name VARCHAR(128) NOT NULL,
                        message_content VARCHAR(128) NOT NULL, 
                        time_stamp TIMESTAMP WITH TIME ZONE                   
                    )";
                await connection.ExecuteAsync(createTableQuery);
                Console.WriteLine("Таблица создана");
            }
        }
    }
}
