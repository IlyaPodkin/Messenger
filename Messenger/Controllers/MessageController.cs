using Messenger.Data;
using Messenger.Hubs;
using Messenger.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers
{
    public class MessageController : BaseController
    {
        private readonly MessageRepository _messageRepository;
        private readonly IHubContext<MessageHub> _messageHub;

        public MessageController(MessageRepository messageRepository, IHubContext<MessageHub> messageHub) 
        {
            _messageRepository = messageRepository;
            _messageHub = messageHub;
        }

        [HttpPost("message")]
        public async Task<IActionResult> SendMessages([FromBody] MessageDTO request) 
        {
            if (string.IsNullOrWhiteSpace(request.Content) || request.Content.Length > 128)
            {
                return BadRequest("Сообщение не должно быть пустым и должно содержать максимум 128 символов");
            }
            var newMessage = await _messageRepository.CreateMessage(request.UserName, request.Content);
            await _messageHub.Clients.All.SendAsync("ReceiveMessage", request.UserName, request.Content, newMessage.TimeStamp);
            return Ok(new {userName = request.UserName,
                           content  = request.Content,
                           timeStamp = newMessage.TimeStamp 
            });
        }

        [HttpGet("message")]
        public async Task<IActionResult> GetMessages() => Ok(await _messageRepository.GetAllMessagesAsync());
    }
}
