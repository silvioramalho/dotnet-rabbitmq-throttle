using DotnetRabbitmqThrottle.Domain.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DotnetRabbitmqThrottle.Application.Extensions
{
    public static class MessageExtension
    {
        private static readonly DefaultContractResolver contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        public static string SerializeMessage(this Message message)
        {
            return JsonConvert.SerializeObject(message, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public static Message DeserializeMessage(this string message)
        {
            return JsonConvert.DeserializeObject<Message>(message);
        }
    }
}