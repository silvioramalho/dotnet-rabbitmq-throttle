using AutoMapper;
using DotnetRabbitmqThrottle.Application.ViewModels;
using DotnetRabbitmqThrottle.Domain.Models;

namespace DotnetRabbitmqThrottle.Application.AutoMapper
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {
            CreateMap<Message, MessageViewModel>();
            CreateMap<MessageViewModel, Message>();
        }
    }
}