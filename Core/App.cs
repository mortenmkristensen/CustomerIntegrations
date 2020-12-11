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
using MessageBroker;
using System.Linq;

namespace Core {
    class App : IApp {

        private IStager Stager { get; set; }
        private IScriptRunner ScriptRunner { get; set; }
        private IDBAccess DBAccess { get; set; }
        private IMessageBroker MessageBroker { get; set; }
        public App(IDBAccess dbAccess, IStager stager, IScriptRunner scriptRunner, IMessageBroker messageBroker) {
            Stager = stager;
            ScriptRunner = scriptRunner;
            DBAccess = dbAccess;
            MessageBroker = messageBroker;
        }

        public void Run(string interpreterPath) {
            var scripts = GetScriptsFromScheduler();
            if(scripts == null) {
                return;
            }
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

            var messages = scriptOutput.Values.ToList();
            SendData(Environment.GetEnvironmentVariable("MP_CONSUMERQUEUE"), messages);
            foreach (var id in scriptOutput) {
                Console.WriteLine($"The Script with id: {id.Key} has been run");
            }
        }

        private IEnumerable<Script> GetScriptsByIds(IEnumerable<string> ids) {
            List<Script> scripts = new List<Script>();
            foreach (var id in ids) {
                scripts.Add(DBAccess.GetScriptById(id));
            }
            return scripts;
        }

        public List<Script> GetScriptsFromScheduler() {
            var queueName = Environment.GetEnvironmentVariable("MP_QUEUENAME");
            return MessageBroker.Receive(queueName);
        }

        private void SendData(string queueName, IEnumerable<string> messages) {
            MessageBroker.Send<string>(queueName, messages);
        }
    }
}
