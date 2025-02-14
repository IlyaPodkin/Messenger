using Messenger.Models;
using Dapper;
using Npgsql;

namespace Messenger.Data
{
    public class MessageRepository
    {
        private readonly string _connectionString;

        public MessageRepository(string connectionString) 
        {
            _connectionString = connectionString;
        }
     
        public async Task<IEnumerable<Message>> GetAllMessagesAsync() 
        {
            using (var connection = new NpgsqlConnection(_connectionString)) 
            {
                await connection.OpenAsync();
                var query = @"SELECT id AS Id, user_name AS UserName, message_content AS Text, time_stamp AS TimeStamp
                              FROM messages";
                return await connection.QueryAsync<Message>(query);
            }
        }

        public async Task<Message> CreateMessage(string userName, string content)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var timeStamp = DateTime.Now;
                var query = @"INSERT INTO messages (user_name, message_content, time_stamp) 
                      VALUES (@UserName, @MessageContent, CURRENT_TIMESTAMP)
                      RETURNING id, user_name AS UserName, message_content AS Text, time_stamp AS TimeStamp";

                var newMessage = await connection.QuerySingleAsync<Message>(query, new { UserName = userName, MessageContent = content, TimeStamp = timeStamp });
                return newMessage;
            }
        }
    }
}
