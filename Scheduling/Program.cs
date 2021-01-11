using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Database;
using MessageBroker;
using Serilog;
using Serilog.Extensions.Logging;
using Microsoft.Extensions.Logging;
using System;

namespace Scheduling {
    class Program {
        static async Task Main(string[] args) {
            //Adding logger with MongoDB Sink and a minimum level of Warning.
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.MongoDB(Environment.GetEnvironmentVariable("MP_CONNECTIONSTRING") + "/" + Environment.GetEnvironmentVariable("MP_DATABASE"),
                                collectionName: Environment.GetEnvironmentVariable("MP_LOGCOLLECTION"))
                .CreateLogger();

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
                        services.AddSingleton(new LoggerProviderCollection());
                        services.AddSingleton<ILoggerFactory>(sc => {
                            var providerCollection = sc.GetService<LoggerProviderCollection>();
                            var factory = new SerilogLoggerFactory(null, true, providerCollection);

                            foreach (var provider in sc.GetServices<ILoggerProvider>())
                                factory.AddProvider(provider);

                            return factory;
                        });
                        services.AddLogging();
                    });
        }

    }
}
