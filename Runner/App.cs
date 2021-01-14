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
using Microsoft.Extensions.Logging;

namespace Runner {
    class App : IApp {
        private IDockerService _dockerService;
        private IMessageBroker _messsagebroker;
        private List<string> _dockerContainers;
        private ILogger<App> _log;
        public App(IDockerService dockerService, IMessageBroker messageBroker, ILogger<App> log) {
            _dockerService = dockerService;
            _messsagebroker = messageBroker;
            _dockerContainers = new List<string>();
            _log = log;
        }

        //This method takes a list of scripts that are in the same langauge and sets the interpreter for that language.
        //The list is the split into smaller lists and a DockerContainer is started for each of those lists
        private async Task Run(List<Script> scripts) {
            string interpreter = scripts.FirstOrDefault().Language;
            if (interpreter == "javascript") {
                interpreter = "node";
            }
            //the list is split into smaller lists of a size that is reasonable for a docker container to handle 
            IEnumerable<List<Script>> lists = null;
            try {
                lists = SplitList<Script>(scripts, int.Parse(Environment.GetEnvironmentVariable("MP_CHUNKSIZE")));
            }
            catch (NullReferenceException nre) {
                _log.LogError(nre, "List cannot be null, when splitting");
                return;
            }
            int i = 0;
            string containerName = "";
            foreach (var list in lists) {
                string name = interpreter + i++;
                try {
                    //if a container does not already exist a new one is started
                    if (!_dockerContainers.Contains(name)) {
                        containerName = _dockerService. StartContainer(Environment.GetEnvironmentVariable("MP_CONNECTIONSTRING"),
                                                             Environment.GetEnvironmentVariable("MP_COLLECTION"),
                                                             Environment.GetEnvironmentVariable("MP_DATABASE"), name, interpreter,
                                                             Environment.GetEnvironmentVariable("MP_MESSAGEBROKER"),
                                                             Environment.GetEnvironmentVariable("MP_QUEUEUSER"),
                                                             Environment.GetEnvironmentVariable("MP_QUEUEPASSWORD"),
                                                             Environment.GetEnvironmentVariable("MP_CONSUMERQUEUE"),
                                                             Environment.GetEnvironmentVariable("MP_LOGCOLLECTION")).Result;
                    }
                }
                catch (DockerApiException dae) {
                    _log.LogError(dae, "Unable to start dockercontainer");
                }
                catch (AggregateException ae) {
                    _log.LogError(ae, "unable to start docker container: " + name);
                    _dockerContainers.Add(containerName);
                }
                catch (Exception e) {
                    _log.LogError(e, "Problem starting container: " + name);
                }
                try {
                    _messsagebroker.Send<Script>(name, list);
                }
                catch (Exception e) {
                    _log.LogError(e, "Unable to send to queue");
                }

            }
            //idle containers are removed
            await CheckForIdleContainers();
            if (containerName != "") {
                _dockerContainers.Add(containerName);
            }
        }

        //This method is called when the program is started and makes sure to pull the image and prune unwanted containers
        //it then starts aTaskFactory to make a new thread which UpdateFromDocker runs on independently from the main thread
        //lastly it starts to listen for incoming messages in queue in the messagebroker specified by the queueName parameter
        public async Task Start(string queueName) {
            await _dockerService.PullImage();
            await _dockerService.PruneContainers();
            TaskFactory taskFactory = new TaskFactory();
            taskFactory.StartNew(UpdateFromDocker);
            ListenToQueue(queueName);
        }

        //This method updates the list of container names every 10 seconds
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

        //This method intantiates an EventHandler which is sent to the Listen method in the messagebroker class together with the queueName
        public void ListenToQueue(string queueName) {
            EventHandler<BasicDeliverEventArgs> consumer = MessageReceivedHandler;
            try {
                _messsagebroker.Listen(queueName, consumer);
            }
            catch (Exception e) {
                _log.LogCritical(e, "Cannot connect to queue: " + queueName);
            }
        }


        //This method takes a list of objects that should be split into smaller lists and an integer specifying how many elements should be in each list
        //it returns a list containing all the smaller lists
        private IEnumerable<List<T>> SplitList<T>(List<T> items, int nSize) {
            for (int i = 0; i < items.Count; i += nSize) {
                yield return items.GetRange(i, Math.Min(nSize, items.Count - i));

            }
        }

        //This method checks if any containers are idle and then removes them if they are
        private async Task CheckForIdleContainers() {
            var containers = await _dockerService.GetContainers();
            foreach (var container in containers) {
                if (container.State == "exited") {
                    await _dockerService.PruneContainers();
                }
            }
        }

        //This is the eventhandler that is executed everytime a new message is recieved from the messagebroker
        private void MessageReceivedHandler(object sender, BasicDeliverEventArgs ea) {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body.Span);
            if (message != null) {
                //The message is converted from JSON to List<Script>.
                var deserializedMessage = JsonConvert.DeserializeObject<List<Script>>(message);
                Run(deserializedMessage);
            }
        }
    }
}
