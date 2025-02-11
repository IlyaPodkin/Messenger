using Messenger.Data;
using Messenger.Models;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers
{
    public class MessageController : BaseController
    {
        private readonly MessageRepository _messageRepository;
        public MessageController(MessageRepository messageRepository) 
        {
            _messageRepository = messageRepository;
        }

        [HttpPost("message")]
        public async Task<IActionResult> SendMessages([FromBody] MessageDTO request) 
        {
            if (string.IsNullOrWhiteSpace(request.Content) || request.Content.Length > 128)
            {
                return BadRequest("Сообщение не должно быть пустым и должно содержать максимум 128 символов");
            }
            await _messageRepository.CreateMessage(request.Content, request.SequenceNumber);
            return Ok("Сообщение отправлено");
        }

        [HttpGet("messages")]
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
