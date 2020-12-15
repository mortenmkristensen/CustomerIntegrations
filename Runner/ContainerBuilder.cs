using MessageBroker;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Runner {
    class ContainerBuilder {
        public IServiceProvider Build() {
            var container = new ServiceCollection();
            container.AddSingleton<IApp, App>();
            container.AddSingleton<IMessageBroker, RabbitBroker>();
            container.AddSingleton<IMessageBrokerConfig, RabbitMQConfig>();
            container.AddSingleton<IDockerService, DockerService>();
            return container.BuildServiceProvider();
        }
    }
}
