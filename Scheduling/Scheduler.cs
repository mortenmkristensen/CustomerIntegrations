using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Database;
using Microsoft.Extensions.Hosting;
using Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Scheduling {
    public class Scheduler : IHostedService {
        private IDBAccess _dbAccess;
        private IDockerService _dockerService;
        private Timer _timer;
        private List<string> rubyIds = new List<string>();
        private List<string> pythonIds = new List<string>();
        private List<string> javaScriptIds = new List<string>();

        public Scheduler(IDBAccess dBAccess, IDockerService dockerService) {
            _dbAccess = dBAccess;
            _dockerService = dockerService;
        }
        public async Task StartAsync(CancellationToken cancellationToken) {
            await PullDockerImage();
            _timer = new Timer(
                Run,
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(5)
                );
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;

        }

        private async void Run(object state) {
            GetIds();
            await SendIdsToRabbitMQ();
            ClearScriptLists();
        }

        private void GetIds() {
            IEnumerable<Script> scripts = _dbAccess.GetAll();
            foreach (var script in scripts) {
                if (script.Language.Equals("ruby")) {
                    rubyIds.Add(script.Id);
                }
                if (script.Language.Equals("python")) {
                    pythonIds.Add(script.Id);
                }
                if (script.Language.Equals("javascript")) {
                    javaScriptIds.Add(script.Id);
                }
            }
        }

        private void SendWithRabbitMQ(string queueName, List<string> ids) {               
            try {
                var factory = new ConnectionFactory() { HostName = "localhost", UserName = "abc", Password = "123" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel()) {
                    channel.QueueDeclare(queue: queueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var message = JsonConvert.SerializeObject(ids);
                    var body = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    channel.BasicPublish(exchange: "",
                                         routingKey: queueName,
                                         basicProperties: properties,
                                         body: body);
                    Console.WriteLine(message);
                    Console.WriteLine();
                    Console.WriteLine();

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

        private IEnumerable<List<T>> SplitList<T>(List<T> ids, int nSize) {
            for (int i = 0; i < ids.Count; i += nSize) {
                yield return ids.GetRange(i, Math.Min(nSize, ids.Count - i));
            }
        }

        private async Task SendIdsToRabbitMQ() {
            var rubyLists = SplitList<string>(rubyIds, 1);
            int i = 0;
            foreach (var list in rubyLists) {
                string queueName = "Ruby_Queue" + i++;
                SendWithRabbitMQ(queueName, list);
                StartDockerContainer("mongodb://192.168.87.107:27017", "Scripts", "MapsPeople", queueName, "ruby", "192.168.87.107", "abc", "123");
            }
            var pythonLists = SplitList<string>(pythonIds, 1);
            i = 0;
            foreach (var list in pythonLists) {
                string queueName = "PythonIds_Queue" + i++;
                SendWithRabbitMQ(queueName, list);
                StartDockerContainer("mongodb://192.168.87.107:27017", "Scripts", "MapsPeople", queueName, "python", "192.168.87.107", "abc", "123");
            }

            var jsLists = SplitList<string>(javaScriptIds, 1);
            i = 0;
            foreach (var list in jsLists) {
                string queueName = "JavaScriptIds_Queue" + i++;
                SendWithRabbitMQ(queueName, list);
                StartDockerContainer("mongodb://192.168.87.107:27017", "Scripts", "MapsPeople", queueName, "node", "192.168.87.107", "abc", "123");
            }
        }

        private void ClearScriptLists() {
            rubyIds.Clear();
            pythonIds.Clear();
            javaScriptIds.Clear();
        }

        private async Task PullDockerImage() {
            await _dockerService.PullImage();
        }
        private async Task StartDockerContainer(string connectionString, string collection, string database, string queuename, string interpreterpath,
                                            string messageBroker, string queueUser, string queuePassword) {
           await _dockerService.StartContainer(connectionString, collection, database, queuename, interpreterpath, messageBroker, queueUser, queuePassword);
        }
    }
}
