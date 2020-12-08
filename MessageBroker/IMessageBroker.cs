using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBroker {
    public interface IMessageBroker {
        void Send(string queueName, List<string> scripts);
        
        string Receive(string queueName);

        void Listen(string queueName);
    }
}
