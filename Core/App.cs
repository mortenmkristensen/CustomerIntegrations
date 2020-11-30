using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Models;
using Core.Exceptions;
using Database;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using RabbitMQ.Client.Exceptions;

namespace Core {
    class App : IApp {

        private IStager Stager { get; set; }
        private IScriptRunner ScriptRunner { get; set; }
        private string Ids { get; set; }
        IDBAccess DBAccess { get; set; }
        public App(IDBAccess dbAccess, IStager stager, IScriptRunner scriptRunner) {
            Stager = stager;
            ScriptRunner = scriptRunner;
          //Ids = Environment.GetEnvironmentVariable("MP_IDS");
            DBAccess = dbAccess;
        }

        public void Run(string interpreterPath) {
            List<Script> scripts =(List<Script>) GetScriptsByIds(DeserializeIds());
            Dictionary<string, string> paths = Stager.GetPaths(scripts);
            Dictionary<string, string> scriptOutput = new Dictionary<string, string>();
            try {
                scriptOutput = ScriptRunner.RunScripts(paths, interpreterPath);
                foreach (var script in scripts) {
                    foreach (var output in scriptOutput) {
                        if(script.Id == output.Key) {
                            script.LastResult = output.Value;
                            script.HasErrors = false;
                            DBAccess.Upsert(script);
                        }
                    }
                }
            }catch(ScriptFailedException sfe) {
                Script script = DBAccess.GetScriptById(sfe.ScriptId);
                script.HasErrors = true;
                script.LastResult = sfe.Message;
                DBAccess.Upsert(script);
                scriptOutput.Add(sfe.ScriptId, sfe.Message); //this is to print out errors while developing
            }
            foreach (var script in scriptOutput) {
                Console.WriteLine(script + "\n\n");

            }
        }

        private List<string> DeserializeIds() {
            return JsonConvert.DeserializeObject<List<string>>(Ids);
        }

        private IEnumerable<Script> GetScriptsByIds(IEnumerable<string> ids) {
            List<Script> scripts = new List<Script>();
            foreach (var id in ids) {
                scripts.Add(DBAccess.GetScriptById(id));
            }
            return scripts;
        }

        private void GetIdsFromScheduler() {
            var queueName = Environment.GetEnvironmentVariable("MP_QUEUENAME");
                try {
                    var factory = new ConnectionFactory() { HostName = "localhost" };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel()) {
                        channel.QueueDeclare(queue: queueName,
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                        channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                        Console.WriteLine(" [*] Waiting for messages.");

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (sender, ea) => {
                            var body = ea.Body.Span;
                            var message = Encoding.UTF8.GetString(body);
                            if (message != null) {
                                Ids = message;
                            }
                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        };
                        channel.BasicConsume(queue: queueName,
                                             autoAck: false,
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
    }
}
