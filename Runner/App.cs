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

        public async Task Run(List<Script> scripts) {
            string interpreter = scripts.FirstOrDefault().Language;
            if (interpreter == "javascript") {
                interpreter = "node";
            }
            var lists = SplitList<Script>(scripts, 1);
            int i = 0;
            string containerName = "";
            foreach (var list in lists) {
                string name = interpreter + i++;
                try {
                    if (!_dockerContainers.Contains(name)) {
                        containerName = StartDockerContainer("mongodb://192.168.87.107:27017", "Scripts", "MapsPeople", name, interpreter, "192.168.87.107", "abc", "123", "Consumer_Queue").Result;
                    }
                } catch (DockerApiException) {
                    name = interpreter + ++i;
                    containerName = StartDockerContainer("mongodb://192.168.87.107:27017", "Scripts", "MapsPeople", name, interpreter, "192.168.87.107", "abc", "123", "Consumer_Queue").Result;
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
            //Timer timer = new Timer(
            //    UpdateFromDocker,
            //    null,
            //    TimeSpan.Zero,
            //    TimeSpan.FromSeconds(10)
            //    );
            TaskFactory taskFactory = new TaskFactory();
            taskFactory.StartNew(UpdateFromDocker);
            UpdateFromDocker();
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
