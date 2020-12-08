using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBroker {
    public class RabbitMQConfig :IMessageBrokerConfig{
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public RabbitMQConfig() {
            HostName = "localhost";
            UserName = "abc";
            Password = "123";
        }
    }
}
