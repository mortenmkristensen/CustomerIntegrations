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
        IDBAccess dbAccess = new DBAccess(new DBConfig());
        Timer _timer;
        List<string> rubyIds = new List<string>();
        List<string> pythonIds = new List<string>();
        List<string> javaScriptIds = new List<string>();
        public DockerService DockerService { get; set; }

        public Scheduler() {
            DockerService = new DockerService();
        }
        public Task StartAsync(CancellationToken cancellationToken) {
            _timer = new Timer(
                Run,
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(5)
                );
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;

        }

        private async void Run(Object state) {
            GetIds();
            await PullDockerImage();
            await SendIdsToRabbitMQ();
            ClearScriptLists();
        }

        private void GetIds() {
            IEnumerable<Script> scripts = dbAccess.GetAll();
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
        /*
        private List<List<string>> BreakdownList(List<string> ids) {
            List<List<string>> superIdsLists = new List<List<string>>();
            while (ids.Count > 30) {
                List<string> idsList1 = new List<string>();
                for (int i =0; i<30; i++) {
                    idsList1.Add(ids[i]);
                }
                superIdsLists.Add(idsList1);
                foreach (var id in idsList1) {
                    ids.Remove(id);
                }
            }
            superIdsLists.Add(ids);
            return superIdsLists;
        }

        private async Task SendIdsToRabbitMQ() {
            List<List<string>> rubySuperLists = BreakdownList(rubyIds);
            for (int i=0; i < rubySuperLists.Count; i++) {
                string queueName = "RubyIds_Queue" + (i).ToString();
                SendWithRabbitMQ(queueName, rubySuperLists[i]);
                await StartDockerContainer("mongodb://192.168.87.107:27017", "Scripts", "MapsPeople", queueName, "ruby", "192.168.87.107", "abc", "123");
            }
            List<List<string>> pythonSuperLists = BreakdownList(pythonIds);
            for (int i = 0; i < pythonSuperLists.Count; i++) {
                string queueName = "PythonIds_Queue" + (i).ToString();
                SendWithRabbitMQ(queueName, pythonSuperLists[i]);
                await StartDockerContainer("mongodb://192.168.87.107:27017", "Scripts", "MapsPeople", queueName, "python", "192.168.87.107", "abc", "123");
            }
            List<List<string>> javaScriptSuperLists = BreakdownList(javaScriptIds);
            for (int i = 0; i < javaScriptSuperLists.Count; i++) {
                string queueName = "JavaScriptIds_Queue" + (i).ToString();
                SendWithRabbitMQ(queueName, javaScriptSuperLists[i]);
                await StartDockerContainer("mongodb://192.168.87.107:27017", "Scripts", "MapsPeople", queueName, "node", "192.168.87.107", "abc", "123");
            }
        }
        */

        private async Task SendIdsToRabbitMQ() {
            string queueName = "RubyIds_Queue";
            SendWithRabbitMQ(queueName, rubyIds);
            await StartDockerContainer("mongodb://192.168.87.107:27017", "Scripts", "MapsPeople", queueName, "ruby", "192.168.87.107", "abc", "123");

            queueName = "PythonIds_Queue";
            SendWithRabbitMQ(queueName, pythonIds);
            await StartDockerContainer("mongodb://192.168.87.107:27017", "Scripts", "MapsPeople", queueName, "python", "192.168.87.107", "abc", "123");

            queueName = "JavaScriptIds_Queue";
            SendWithRabbitMQ(queueName,javaScriptIds);
            await StartDockerContainer("mongodb://192.168.87.107:27017", "Scripts", "MapsPeople", queueName, "node", "192.168.87.107", "abc", "123");
        }

        private void ClearScriptLists() {
            rubyIds.Clear();
            pythonIds.Clear();
            javaScriptIds.Clear();
        }

        private async Task PullDockerImage() {
            await DockerService.PullImage();
        }
        private async Task<string> StartDockerContainer(string connectionString, string collection, string database, string queuename, string interpreterpath,
                                            string messageBroker, string queueUser, string queuePassword) {
           return await DockerService.StartContainer(connectionString, collection, database, queuename, interpreterpath, messageBroker, queueUser, queuePassword);
        }
    }
}
