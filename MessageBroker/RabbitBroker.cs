using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using Models;
using RabbitMQ.Client.Events;

namespace MessageBroker {
    public class RabbitBroker : IMessageBroker {
        private readonly IMessageBrokerConfig _config;
        public RabbitBroker(IMessageBrokerConfig config) {
            _config = config;
        }

        //This method takes a queueName and an Eventhandler that is used when a new message is received
        public void Listen(string queueName, EventHandler<BasicDeliverEventArgs> handler) {
            try {
                //the config class is used to configure the host and credetials for the connection
                var factory = new ConnectionFactory() { HostName = _config.HostName, UserName = _config.UserName, Password = _config.Password };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel()) {
                    //arguments that is used to declare a queue
                    Dictionary<string, object> args = new Dictionary<string, object>();
                    //the queue deletes itself after 30 seconds of idle time
                    args.Add("x-expires", 30000);
                    //a queue is declared that is durable meaning it can survive a broker restart, non-exclusive meaning multiple connections can use it and that does not delete itself when the last subscriber unsubscribes
                    channel.QueueDeclare(queue: queueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: args);

                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    Console.WriteLine(" [*] Waiting for messages.");
                    
                    //a new consumer is made and the eventhandler is added
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += handler;
                    //messages are consumed from the queue
                    channel.BasicConsume(queue: queueName,
                                         autoAck: true,
                                         consumer: consumer);
                    Console.ReadLine();
                }
            } catch (Exception e) {
                if (e is AlreadyClosedException) {
                    Console.WriteLine("The connectionis already closed");
                } else if (e is BrokerUnreachableException) {
                    Console.WriteLine("The broker cannot be reached");
                } else if (e is OperationInterruptedException) {
                    Console.WriteLine("The operation was interupted");
                } else if (e is ConnectFailureException) {
                    Console.WriteLine("Could not connect to the broker broker");
                } else {
                    Console.WriteLine("Something went wrong");
                }
            }
        }

        //This method pulls one message at a time from the queue without creating a consumer that subscribes to the queue
        public List<Script> Receive(string queueName) {
            List<Script> scipts = null;
            try {
                //the config class is used to configure the host and credetials for the connection
                var factory = new ConnectionFactory() { HostName = _config.HostName, UserName = _config.UserName, Password = _config.Password };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel()) {
                    //arguments that is used to declare a queue
                    Dictionary<string, object> args = new Dictionary<string, object>();
                    //the queue deletes itself after 30 seconds of idle time
                    args.Add("x-expires", 30000);
                    //a queueu is declared that is durable meaning it can survive a broker restart, non-exclusive meaning multiple connections can use it and that does not delete itself when the last subscriber unsubscribes
                    channel.QueueDeclare(queue: queueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: args);
                    //a message is pulled from the queue
                    var data = channel.BasicGet(queueName, false);
                    if (data != null) {
                        var message = Encoding.UTF8.GetString(data.Body.Span);
                        //the contents of the message is deserialized
                        scipts = JsonConvert.DeserializeObject<List<Script>>(message);
                        // ack the message, ie. confirm that we have processed it
                        // otherwise it will be requeued a bit later
                        channel.BasicAck(data.DeliveryTag, false);
                    }
                }
            } catch (Exception e) {
                if (e is AlreadyClosedException) {
                    Console.WriteLine("The connectionis already closed");
                } else if (e is BrokerUnreachableException) {
                    Console.WriteLine("The broker cannot be reached");
                } else if (e is OperationInterruptedException) {
                    Console.WriteLine("The operation was interupted");
                } else if (e is ConnectFailureException) {
                    Console.WriteLine("Could not connect to the broker broker");
                } else {
                    Console.WriteLine("Something went wrong");
                }
            }
            return scipts;
        }

        //this method send a message to the queue
        public void Send<T>(string queueName, IEnumerable<T> messages) {
            try {
                //the config class is used to configure the host and credetials for the connection
                var factory = new ConnectionFactory() { HostName = _config.HostName, UserName = _config.UserName, Password = _config.Password };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel()) {
                    //arguments that is used to declare a queue
                    Dictionary<string, object> args = new Dictionary<string, object>();
                    //the queue deletes itself after 30 seconds of idle time
                    args.Add("x-expires", 30000);
                    //a queueu is declared that is durable meaning it can survive a broker restart, non-exclusive meaning multiple connections can use it and that does not delete itself when the last subscriber unsubscribes
                    channel.QueueDeclare(queue: queueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: args);
                    //the message that should be sent is serialized
                    var message = JsonConvert.SerializeObject(messages);
                    var body = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    //the message is published
                    channel.BasicPublish(exchange: "",
                                         routingKey: queueName,
                                         basicProperties: properties,
                                         body: body);
                }
            } catch (Exception e) {
                if (e is AlreadyClosedException) {
                    Console.WriteLine("The connectionis already closed");
                } else if (e is BrokerUnreachableException) {
                    Console.WriteLine("The broker cannot be reached");
                } else if (e is OperationInterruptedException) {
                    Console.WriteLine("The operation was interupted");
                } else if (e is ConnectFailureException) {
                    Console.WriteLine("Could not connect to the broker broker");
                } else {
                    Console.WriteLine("Something went wrong");
                }
            }

        }
    }
}
