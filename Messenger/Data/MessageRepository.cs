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

        public async Task<IEnumerable<Message>> GetMessagesAsync(DateTime from, DateTime to) 
        {
            using (var connection = new NpgsqlConnection(_connectionString)) 
            {
                await connection.OpenAsync();
                var query = @"SELECT id AS Id, message_content AS Text, time_stamp AS TimeStamp, sequence_number AS Order 
                              FROM messages
                              WHERE time_stamp BETWEEN @From AND @To
                              ORDER BY time_stamp ASC";
                return await connection.QueryAsync<Message>(query, new {From = from, To = to });
            }
        }        
        public async Task<IEnumerable<Message>> GetAllMessagesAsync() 
        {
            using (var connection = new NpgsqlConnection(_connectionString)) 
            {
                await connection.OpenAsync();
                var query = @"SELECT id AS Id, message_content AS Text, time_stamp AS TimeStamp, sequence_number AS Order 
                              FROM messages";
                return await connection.QueryAsync<Message>(query);
            }
        }

        public async Task CreateMessage(string content, int sequenceNumber) 
        {
            using (var connection = new NpgsqlConnection(_connectionString)) 
            {
                await connection.OpenAsync();
                var query = @"INSERT INTO messages (message_content, sequence_number) VALUES (@MessageContent, @SequenceNumber)";
                await connection.QueryAsync<Message>(query, new { MessageContent = content, SequenceNumber = sequenceNumber });
            }
        }
    }
}
