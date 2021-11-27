using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    public class MessageController : ControllerBase
    {

        private readonly ILogger<MessageController> _logger;
        private readonly IBus _bus;

        public MessageController(ILogger<MessageController> logger,IBus bus) {
            _logger = logger;
            _bus = bus;
        }

        [HttpPost("message/sendasync")]
        public async Task<ActionResult> SendAsync() {

            _logger.LogInformation("Message sending...");
            await _bus.Publish<IMessage>(new {

                Id = new Random().Next(5),
                Text = Guid.NewGuid().ToString()

            });
            _logger.LogInformation("Message sent!");
            return Ok();
        }
    }
}
