using Microsoft.Extensions.DependencyInjection;
using System;
using Database;
using MessageBroker;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

namespace Core {
    //This class is used for the pattern Dependency Injection 
    class ContainerBuilder {
        public IServiceProvider Build() {
            var providers = new LoggerProviderCollection();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.MongoDB("mongodb://localhost:27017/MapsPeople", collectionName: "log")
                .WriteTo.Providers(providers)
                .CreateLogger();

            var container = new ServiceCollection();
            container.AddSingleton<IDBConfig, DBConfig>();
            container.AddSingleton<IDBAccess, DBAccess>();
            container.AddSingleton<IApp, App>();
            container.AddTransient<IStager, Stager>();
            container.AddTransient<IScriptRunner, ScriptRunner>();
            container.AddSingleton<IMessageBroker, RabbitBroker>();
            container.AddSingleton<IMessageBrokerConfig, RabbitMQConfig>();
            container.AddTransient<IDataValidation, DataValidation>();
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
