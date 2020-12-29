using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Database;
using MessageBroker;

namespace Scheduling {
    class Program {
        static async Task Main(string[] args) {
            await CreateHostBuilder(args).Build().RunAsync();
            //This method creates a hostBuilder that contains all the dependencies for the Scheduler. 
            static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                    .ConfigureServices((_, services) => {
                        services.AddSingleton<IDBAccess, DBAccess>();
                        services.AddSingleton<IDBConfig, DBConfig>();
                        services.AddSingleton<IMessageBroker, RabbitBroker>();
                        services.AddSingleton<IMessageBrokerConfig, RabbitMQConfig>();
                        services.AddHostedService<Scheduler>();
                    });
        }

    }
}
