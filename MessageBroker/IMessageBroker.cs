using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace MessageBroker {
    public interface IMessageBroker {
        void Send(string queueName, List<Script> scripts);
        
        string Receive(string queueName);

        void Listen(string queueName);
    }
}
