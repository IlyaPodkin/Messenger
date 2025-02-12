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
        private readonly MessageHub _messageHub;

        public MessageController(MessageRepository messageRepository, MessageHub messageHub) 
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
            await _messageRepository.CreateMessage(request.Content, request.SequenceNumber);
            await _messageHub.Clients.All.SendAsync("ReseiveMessage", "Server", request.Content);

            return Ok("Сообщение отправлено");
        }

        [HttpGet("message")]
        public async Task<IActionResult> GetMessagesByTimeRange(DateTime from, DateTime to) 
        {
            if (from > to) 
            {
                return BadRequest("Дата начала не может быть больше даты окончания.");
            }
            var messages = await _messageRepository.GetMessagesAsync(from, to);
            return Ok(messages);
        }
        [HttpGet("message/all")]
        public async Task<IActionResult> GetMessagesByTimeRange() => Ok(await _messageRepository.GetAllMessagesAsync());
    }
}
