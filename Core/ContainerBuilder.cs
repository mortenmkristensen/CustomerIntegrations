using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Database;
using MessageBroker;

namespace Core {
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
