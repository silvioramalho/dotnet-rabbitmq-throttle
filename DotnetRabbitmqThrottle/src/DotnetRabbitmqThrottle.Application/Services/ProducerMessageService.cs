using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using DotnetRabbitmqThrottle.Application.Extensions;
using DotnetRabbitmqThrottle.Application.ViewModels;
using DotnetRabbitmqThrottle.Domain.Models;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace DotnetRabbitmqThrottle.Application.Services
{
    public class ProducerMessageService : IProducerMessage
    {
        private readonly ILogger<ProducerMessageService> _logger;
        private readonly IMapper _mapper;
        private readonly IChannelSetup _channelSetup;

        public ProducerMessageService(
            IMapper mapper,
            IChannelSetup channelSetup,
            ILogger<ProducerMessageService> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _channelSetup = channelSetup;
        }

        public void Send(IEnumerable<MessageViewModel> messages)
        {
            try
            {
                string queueName = messages.Select(p => p.From)
                    .FirstOrDefault(p => !string.IsNullOrWhiteSpace(p));

                var channel = _channelSetup.CreateChannel(queueName);

                foreach (var item in messages)
                {
                    Message message = _mapper.Map<Message>(item);
                    IBasicProperties props = channel.CreateBasicProperties();
                    props.Priority = (byte)(item.Priority ?? 0);
                    props.Persistent = true;

                    channel.BasicPublish(exchange: "",
                                         routingKey: queueName,
                                         basicProperties: props,
                                         body: Encoding.UTF8.GetBytes(message.SerializeMessage()));

                    _logger.LogInformation(
                        $"[Message sended] {message}");
                }

                _logger.LogInformation("Sending messages completed !!!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.GetType().FullName} | " +
                             $"Message: {ex.Message}");
            }
        }
    }
}