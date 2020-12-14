using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MessageBroker;
using Models;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System.Linq;

namespace Runner {
    class App : IApp {
        private IDockerService _dockerService;
        private IMessageBroker _messsagebroker;
        private Dictionary<string, int> dockerContainers;
        public App(IDockerService dockerService, IMessageBroker messageBroker) {
            _dockerService = dockerService;
            _messsagebroker = messageBroker;
            dockerContainers = new Dictionary<string, int>();
        }

        public async void Run(List<Script> scripts) {
            string interpreter = scripts.FirstOrDefault().Language;
            if (interpreter == "javascript") {
                interpreter = "node";
            }
            var lists = SplitList<Script>(scripts, 1);
            int i = 0;
            foreach (var list in lists) {
                string name = interpreter + i++;
                await StartDockerContainer("mongodb://192.168.0.117:27017", "Scripts", "MapsPeople", name, interpreter, "192.168.0.117", "abc", "123", "Consumer_Queue");
                _messsagebroker.Send<Script>(name, list);
            }
            await CheckForIdleContainers();
        }

        private async Task PullDockerImage() {
            await _dockerService.PullImage();
        }
        private async Task StartDockerContainer(string connectionString, string collection, string database, string queuename, string interpreterpath,
                                            string messageBroker, string queueUser, string queuePassword, string consumerQueue) {
             await _dockerService.StartContainer(connectionString, collection, database, queuename, interpreterpath, messageBroker, queueUser, queuePassword, consumerQueue);

        }

        public async Task start(string queueName) {
           await PullDockerImage();
            ListenToQueue(queueName);
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
          var containers =  await _dockerService.GetContainers();
            foreach (var container in containers) {
                if(container.State == "exited") {
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
