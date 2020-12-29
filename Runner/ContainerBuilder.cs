﻿using MessageBroker;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Runner {
    class ContainerBuilder {

        //This method makes a service provider which contains all the dependencies of the runner
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
