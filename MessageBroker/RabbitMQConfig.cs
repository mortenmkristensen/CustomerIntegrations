using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBroker {
    public class RabbitMQConfig : IMessageBrokerConfig {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public RabbitMQConfig() {
            HostName = Environment.GetEnvironmentVariable("MP_MESSAGEBROKER");
            if (HostName == null) {
                HostName = "localhost";
            }
            UserName = Environment.GetEnvironmentVariable("MP_QUEUEUSER");
            if (UserName == null) {
                UserName = "abc";
            }
            Password = Environment.GetEnvironmentVariable("MP_PASSWORD");
            if (Password == null) {
                Password = "123";
            }

        }
    }
}
