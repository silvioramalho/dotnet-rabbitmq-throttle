using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DotnetRabbitmqThrottle.Consumer.ConsoletHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(
                "*** Testando o consumo de mensagens com RabbitMQ + Filas ***");

            /*if (args.Length != 2)
            {
                Console.WriteLine(
                    "Informe 2 parametros: " +
                    "no primeiro a string de conexao com o RabbitMQ, " +
                    "no segundo a Fila/Queue a ser utilizado no consumo das mensagens...");
                return;
            }*/
            string[] conf = new string[2] { "amqp://userpoc:POC2021!@localhost:5672/", "testqueue" };

            CreateHostBuilder(conf).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<WorkerParams>(
                        new WorkerParams()
                        {
                            ConnectionString = args[0],
                            Queue = args[1]
                        });
                    services.AddHostedService<Worker>();
                });
    }
}
