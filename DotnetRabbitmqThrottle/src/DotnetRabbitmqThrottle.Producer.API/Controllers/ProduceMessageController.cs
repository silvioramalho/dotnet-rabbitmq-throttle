﻿using System.Collections.Generic;
using DotnetRabbitmqThrottle.Producer.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotnetRabbitmqThrottle.Producer.API.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class ProduceMessageController : ControllerBase
    {

        private static Counter _counter = new Counter();
        private readonly ILogger<ProduceMessageController> _logger;
        private readonly IProducerMessage _producerMessage;
        
        public ProduceMessageController(
            ILogger<ProduceMessageController> logger,
            IProducerMessage producerMessage)
        {
            _logger = logger;
            _producerMessage = producerMessage;

        }        

        [HttpGet]
        public object Get()
        {
            return new
            {
                TotalSendedMessages = _counter.Value
            };
        }

        [HttpPost]
        public object Post([FromBody] IEnumerable<Message> messages)
        {
            lock (_counter)
            {
                _counter.Inc();

                _producerMessage.Send(messages);

                return new
                {
                    Result = "The messages were queued."
                };
            }
        }
    }
}
