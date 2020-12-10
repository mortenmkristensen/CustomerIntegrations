using System;
using System.Collections.Generic;
using System.Text;
using Models;
using RabbitMQ.Client.Events;

namespace MessageBroker {
    public interface IMessageBroker {
        void Send<T>(string queueName, IEnumerable<T> messages);
        
        string Receive(string queueName);

        void Listen(string queueName, EventingBasicConsumer consumer);
    }
}
