using System;

namespace MessageBroker {
    //this class provides a hostname and credentials for RabbitMQ
    public class RabbitMQConfig : IMessageBrokerConfig {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        //properties can either be set when creating an istance of this class or be default values
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
