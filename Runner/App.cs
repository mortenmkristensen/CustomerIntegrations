using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MessageBroker;
using Models;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System.Linq;
using Docker.DotNet;
using System.Threading;

namespace Runner {
    class App : IApp {
        private IDockerService _dockerService;
        private IMessageBroker _messsagebroker;
        private List<string> _dockerContainers;
        public App(IDockerService dockerService, IMessageBroker messageBroker) {
            _dockerService = dockerService;
            _messsagebroker = messageBroker;
            _dockerContainers = new List<string>();
        }

        private async Task Run(List<Script> scripts) {
            string interpreter = scripts.FirstOrDefault().Language;
            if (interpreter == "javascript") {
                interpreter = "node";
            }
            var lists = SplitList<Script>(scripts,int.Parse(Environment.GetEnvironmentVariable("MP_CHUNKSIZE")));
            int i = 0;
            string containerName = "";
            foreach (var list in lists) {
                string name = interpreter + i++;
                try {
                    if (!_dockerContainers.Contains(name)) {
                        containerName = StartDockerContainer(Environment.GetEnvironmentVariable("MP_CONNECTIONSTRING"), Environment.GetEnvironmentVariable("MP_COLLECTION"),
                                                                                                Environment.GetEnvironmentVariable("MP_DATABASE"), name, interpreter, 
                                                                                                Environment.GetEnvironmentVariable("MP_MESSAGEBROKER"), Environment.GetEnvironmentVariable("MP_QUEUEUSER"),
                                                                                                Environment.GetEnvironmentVariable("MP_QUEUEPASSWORD"), Environment.GetEnvironmentVariable("MP_CONSUMERQUEUE")).Result;
                    }
                } catch (DockerApiException) {
                    name = interpreter + ++i;
                    containerName = StartDockerContainer(Environment.GetEnvironmentVariable("MP_CONNECTIONSTRING"), Environment.GetEnvironmentVariable("MP_COLLECTION"),
                                                                                               Environment.GetEnvironmentVariable("MP_DATABASE"), name, interpreter,
                                                                                               Environment.GetEnvironmentVariable("MP_MESSAGEBROKER"), Environment.GetEnvironmentVariable("MP_QUEUEUSER"),
                                                                                               Environment.GetEnvironmentVariable("MP_QUEUEPASSWORD"), Environment.GetEnvironmentVariable("MP_CONSUMERQUEUE")).Result;
                }
                _messsagebroker.Send<Script>(name, list);
            }
            await CheckForIdleContainers();
            if (containerName != "") {
                _dockerContainers.Add(containerName);
            }
        }

        private async Task PullDockerImage() {
            await _dockerService.PullImage();
        }
        private async Task<string> StartDockerContainer(string connectionString, string collection, string database, string queuename, string interpreterpath,
                                            string messageBroker, string queueUser, string queuePassword, string consumerQueue) {
            return await _dockerService.StartContainer(connectionString, collection, database, queuename, interpreterpath, messageBroker, queueUser, queuePassword, consumerQueue);

        }

        public async Task start(string queueName) {
            await PullDockerImage();
            await PruneContainers();
            TaskFactory taskFactory = new TaskFactory();
            taskFactory.StartNew(UpdateFromDocker);
            ListenToQueue(queueName);
        }

        private async void UpdateFromDocker() {
            while (true) {
                List<string> temp = new List<string>();
                var containersFromDocker = await _dockerService.GetContainers();
                foreach (var container in containersFromDocker) {
                    temp.Add(container.Names[0].Split("/")[1]);
                }
                _dockerContainers = temp;
                Thread.Sleep(10000);
            }
        }

        public void ListenToQueue(string queueName) {
            EventHandler<BasicDeliverEventArgs> consumer = Handler;
            _messsagebroker.Listen(queueName, consumer);
        }

        private IEnumerable<List<T>> SplitList<T>(List<T> ids, int nSize) {
            for (int i = 0; i < ids.Count; i += nSize) {
                yield return ids.GetRange(i, Math.Min(nSize, ids.Count - i));

            }
        }

        private async Task PruneContainers() {
            await _dockerService.PruneContainers();

        }

        private async Task CheckForIdleContainers() {
            var containers = await _dockerService.GetContainers();
            foreach (var container in containers) {
                if (container.State == "exited") {
                    await PruneContainers();
                }
            }
        }

        private void Handler(object sender, BasicDeliverEventArgs ea) {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body.Span);
            if (message != null) {
                //The message is converted from JSON to IEnumerable<Location>.
                var deserializedMessage = JsonConvert.DeserializeObject<List<Script>>(message);
                Run(deserializedMessage);
            }
        }
    }
}
