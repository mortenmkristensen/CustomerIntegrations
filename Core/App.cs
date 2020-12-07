﻿using Newtonsoft.Json;
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
        IDBAccess DBAccess { get; set; }
        public App(IDBAccess dbAccess, IStager stager, IScriptRunner scriptRunner) {
            Stager = stager;
            ScriptRunner = scriptRunner;
            DBAccess = dbAccess;
        }

        public void Run(string interpreterPath) {
            string ids = GetIdsFromScheduler();
            Dictionary<string, string> paths = new Dictionary<string, string>();
            if (ids == null) {
                return;
            }
            List<Script> scripts = (List<Script>)GetScriptsByIds(DeserializeIds(ids));
            try {
                paths = Stager.GetPaths(scripts);
            }
            catch (Exception e) {
                Console.WriteLine("Error occured while getting paths, Error:", e.Message);
            }
            Dictionary<string, string> scriptOutput = new Dictionary<string, string>();
                foreach (var script in scripts) {
                    foreach (var path in paths) {
                        if (script.Id == path.Key) {
                            try {
                                var result = ScriptRunner.RunScript(script.Id, path.Value, interpreterPath);
                                script.LastResult = result;
                                script.HasErrors = false;
                                DBAccess.Upsert(script);
                                scriptOutput.Add(script.Id, result);
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

        private List<string> DeserializeIds(string ids) {
            return JsonConvert.DeserializeObject<List<string>>(ids);
        }

        private IEnumerable<Script> GetScriptsByIds(IEnumerable<string> ids) {
            List<Script> scripts = new List<Script>();
            foreach (var id in ids) {
                scripts.Add(DBAccess.GetScriptById(id));
            }
            return scripts;
        }

        public string GetIdsFromScheduler() {
            var queueName = Environment.GetEnvironmentVariable("MP_QUEUENAME");
            string message = null;
            try {
                var factory = new ConnectionFactory() { HostName = Environment.GetEnvironmentVariable("MP_MESSAGEBROKER"), Password= Environment.GetEnvironmentVariable("MP_QUEUEPASSWORD"), UserName= Environment.GetEnvironmentVariable("MP_QUEUEUSER") };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel()) {
                    channel.QueueDeclare(queue: queueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
                    var data = channel.BasicGet(queueName, false);
                    if (data != null) {
                        message = Encoding.UTF8.GetString(data.Body.Span);
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
            return message;
        }
    }
}
