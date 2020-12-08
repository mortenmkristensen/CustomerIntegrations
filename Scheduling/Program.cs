using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Database;
using MessageBroker;

namespace Scheduling {
    class Program {
        static async Task Main(string[] args) {
            await CreateHostBuilder(args).Build().RunAsync();

            static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                    .ConfigureServices((_, services) => {
                        services.AddSingleton<IDBAccess, DBAccess>();
                        services.AddSingleton<IDBConfig, DBConfig>();
                        services.AddSingleton<IDockerService, DockerService>();
                        services.AddSingleton<IMessageBroker, RabbitBroker>();
                        services.AddSingleton<IMessageBrokerConfig, RabbitMQConfig>();
                        services.AddHostedService<Scheduler>();
                    });
        }
        
    }
}
