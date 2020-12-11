using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace MessageBroker {
    public interface IMessageBroker {
        void Send<T>(string queueName, IEnumerable<T> messages);

        List<Script> Receive(string queueName);

        void Listen(string queueName);
    }
}
