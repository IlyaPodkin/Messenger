using Messenger.Data;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics.CodeAnalysis;

namespace Messenger.Hubs
{
    public class MessageHub : Hub
    {
        private readonly MessageRepository _messageRepository;

        public MessageHub(MessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task SendMessage(string user, string message) 
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task GetOldMessages()
        {
            var messages = await _messageRepository.GetAllMessagesAsync();
            await Clients.Caller.SendAsync("ReceiveOldMessages", messages);
        }
    }
}
