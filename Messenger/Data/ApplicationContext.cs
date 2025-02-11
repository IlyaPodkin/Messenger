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
        public async Task CreateTable() 
        {
            using(var connection = new NpgsqlConnection(_connectionString)) 
            {
                await connection.OpenAsync();
                var createTableQuery = 
                    @"CREATE TABLE IF NOT EXISTS Messages(
                        id SERIAL PRIMARY KEY,
                        messageContent VARCHAR(128) NOT NULL, 
                        timeStamp TIMESTAMP DEFAULT TIMESTAMP CURRENT_TIMESTAMP,
                        order INT NOT NULL
        `           )";
                await connection.ExecuteAsync(createTableQuery);
                Console.WriteLine("Таблица создана");
            }
        }
    }
}
