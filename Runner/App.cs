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
        public App(IDockerService dockerService, IMessageBroker messageBroker) {
            _dockerService = dockerService;
            _messsagebroker = messageBroker;
        }

        public void Run(List<Script> scripts) {
            string interpreter = scripts.FirstOrDefault().Language;
            if (interpreter == "javascript") {
                interpreter = "node";
            }
            var lists = SplitList<Script>(scripts, 1);
            int i = 0;
            foreach (var list in lists) {
                string name = interpreter + i++;
                StartDockerContainer("mongodb://192.168.0.117:27017", "Scripts", "MapsPeople", name, interpreter, "192.168.0.117", "abc", "123");
                _messsagebroker.Send<Script>(name, list);
            }
        }

        private async Task PullDockerImage() {
            await _dockerService.PullImage();
        }
        private async Task StartDockerContainer(string connectionString, string collection, string database, string queuename, string interpreterpath,
                                            string messageBroker, string queueUser, string queuePassword) {
            await _dockerService.StartContainer(connectionString, collection, database, queuename, interpreterpath, messageBroker, queueUser, queuePassword);
        }

        public async Task ListenToQueue(string queueName) {
            await PullDockerImage();
            EventHandler<BasicDeliverEventArgs> consumer = Handler;
            _messsagebroker.Listen(queueName, consumer);
        }

        private IEnumerable<List<T>> SplitList<T>(List<T> ids, int nSize) {
            for (int i = 0; i < ids.Count; i += nSize) {
                yield return ids.GetRange(i, Math.Min(nSize, ids.Count - i));
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
