using MessageBroker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System;

namespace Runner {
    class ContainerBuilder {

        //This method makes a service provider which contains all the dependencies of the runner
        public IServiceProvider Build() {
            var providers = new LoggerProviderCollection();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.MongoDB("mongodb://localhost:27017/MapsPeople", collectionName: "log")
                .WriteTo.Providers(providers)
                .CreateLogger();

            var container = new ServiceCollection();
            container.AddSingleton<IApp, App>();
            container.AddSingleton<IMessageBroker, RabbitBroker>();
            container.AddSingleton<IMessageBrokerConfig, RabbitMQConfig>();
            container.AddSingleton<IDockerService, DockerService>();
            container.AddSingleton(providers);
            container.AddSingleton<ILoggerFactory>(sc => {
                var providerCollection = sc.GetService<LoggerProviderCollection>();
                var factory = new SerilogLoggerFactory(null, true, providerCollection);

                foreach (var provider in sc.GetServices<ILoggerProvider>())
                    factory.AddProvider(provider);

                return factory;
            });
            container.AddLogging();
            return container.BuildServiceProvider();
        }
    }
}
