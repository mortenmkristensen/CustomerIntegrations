using Microsoft.Extensions.DependencyInjection;
using System;
using Database;
using MessageBroker;

namespace Core {
    //This class is used for the pattern Dependency Injection 
    class ContainerBuilder {
        public IServiceProvider Build() {
            var container = new ServiceCollection();
            container.AddSingleton<IDBConfig, DBConfig>();
            container.AddSingleton<IDBAccess, DBAccess>();
            container.AddSingleton<IApp, App>();
            container.AddTransient<IStager, Stager>();
            container.AddTransient<IScriptRunner, ScriptRunner>();
            container.AddSingleton<IMessageBroker, RabbitBroker>();
            container.AddSingleton<IMessageBrokerConfig, RabbitMQConfig>();
            container.AddTransient<IDataValidation, DataValidation>();
            return container.BuildServiceProvider();
        }
    }
}
