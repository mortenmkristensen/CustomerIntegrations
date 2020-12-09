using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Models;
using Core.Exceptions;
using Database;
using RabbitMQ.Client.Events;
using System.Text;
using MessageBroker;

namespace Core {
    class App : IApp {

        private IStager Stager { get; set; }
        private IScriptRunner ScriptRunner { get; set; }
        private IDBAccess DBAccess { get; set; }
        private IMessageBroker MessageBroker { get; set; }
        private IMessageBrokerConfig Config { get; set; }
        public App(IDBAccess dbAccess, IStager stager, IScriptRunner scriptRunner, IMessageBroker messageBroker, IMessageBrokerConfig config) {
            Stager = stager;
            ScriptRunner = scriptRunner;
            DBAccess = dbAccess;
            MessageBroker = messageBroker;
            Config = config;
        }

        public void Run(string interpreterPath, List<Script> scripts) {
            Dictionary<string, string> paths = Stager.GetPaths(scripts);
            Dictionary<string, string> scriptOutput = new Dictionary<string, string>();
                foreach (var script in scripts) {
                    foreach (var path in paths) {
                        if (script._id == path.Key) {
                            try {
                                var result = ScriptRunner.RunScript(script._id, path.Value, interpreterPath);
                                script.LastResult = result;
                                script.HasErrors = false;
                                DBAccess.Upsert(script);
                                scriptOutput.Add(script._id, result);
                            } catch (ScriptFailedException sfe) {
                                script.HasErrors = true;
                                script.LastResult = sfe.Message;
                                DBAccess.Upsert(script);
                                scriptOutput.Add(sfe.ScriptId, sfe.Message); //this is to print out errors while developing
                            }
                        }
                    }
                }
            foreach (var script in scriptOutput) {
                Console.WriteLine(script + "\n\n");

            }
        }

        private List<Script> DeserializeIds(string scripts) {
            return JsonConvert.DeserializeObject<List<Script>>(scripts);
        }

        private IEnumerable<Script> GetScriptsByIds(IEnumerable<string> ids) {
            List<Script> scripts = new List<Script>();
            foreach (var id in ids) {
                scripts.Add(DBAccess.GetScriptById(id));
            }
            return scripts;
        }

        public string GetScriptsFromScheduler() {
            var queueName = Environment.GetEnvironmentVariable("MP_QUEUENAME");
            return MessageBroker.Receive(queueName);
        }
        public void Listen(string interpreterPath) {
            var queueName = Environment.GetEnvironmentVariable("MP_QUEUENAME");
                    var consumer = new EventingBasicConsumer(null);
                    consumer.Received += (sender, ea) => {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.Span);
                        if (message != null) {
                            //The message is converted from JSON to IEnumerable<Script>.
                            var deserializedMessage = JsonConvert.DeserializeObject<IEnumerable<Script>>(message);
                            Run(interpreterPath, (List<Script>)deserializedMessage);
                        }
                        consumer.Model.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    };
            MessageBroker.Listen(queueName, consumer);
        }
    }
}
