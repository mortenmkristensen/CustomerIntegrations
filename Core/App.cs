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
            string scriptsJson = GetScriptsFromScheduler();
            if(scriptsJson == null) {
                return;
            }
            List<Script> scripts = DeserializeIds(scriptsJson);
            try {
                Dictionary<string, string> paths = Stager.GetPaths(scripts);
            }catch(Exception e) {
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
    }
}
