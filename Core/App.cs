using System;
using System.Collections.Generic;
using Models;
using Core.Exceptions;
using Database;
using MessageBroker;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Core {
    //This class is the Hub for the system. It runs methods in the correct order.
   public class App : IApp {

        private IStager Stager { get; set; }
        private IScriptRunner ScriptRunner { get; set; }
        private IDBAccess DBAccess { get; set; }
        private IMessageBroker MessageBroker { get; set; }
        private IDataValidation DataValidation { get; set; }
        private ILogger<App> _log;
        public App(IDBAccess dbAccess, IStager stager, IScriptRunner scriptRunner, IMessageBroker messageBroker, IDataValidation dataValidation, ILogger<App> log) {
            Stager = stager;
            ScriptRunner = scriptRunner;
            DBAccess = dbAccess;
            MessageBroker = messageBroker;
            DataValidation = dataValidation;
            _log = log;
        }

        //The first thing this method does is getting a list of scripts from a queue, and if the list is null count is counted up by 1. 
        public int Run(string interpreterPath, int count, string queueName, string consumerQuequeName) {
            var scripts = GetScriptsFromQueue(queueName);
            if (scripts == null) {
                return ++count;
            }
            Dictionary<string, string> paths = Stager.GetPaths(scripts);
            Dictionary<string, string> scriptOutput = new Dictionary<string, string>();
            foreach (var script in scripts) {
                foreach (var path in paths) {
                    // To make sure that it's the correct script we got the script Id is compared to the path key. 
                    if (script.Id == path.Key) {
                        //ScriptRunner.RunScript returns a string that is the result of the executed script (saved in the variable: result) 
                        try {
                            var result = ScriptRunner.RunScript(script.Id, path.Value, interpreterPath);
                            if (DataValidation.ValidateScriptOutput(result)) {
                                script.LastResult = result;
                                script.HasErrors = false;
                                //Script is updated in the database.
                                DBAccess.Upsert(script);
                                scriptOutput.Add(script.Id, result);
                            }
                        } catch (ScriptFailedException sfe) {
                            script.HasErrors = true;
                            script.LastResult = sfe.Message;
                            DBAccess.Upsert(script);
                            _log.LogWarning(sfe, "Exception");
                        }
                    }
                }
            }
            //The result is sent to a messagebroker (the specific queue comes from the environment variablen MP_CONSUMERQUEUE). 
            var messages = scriptOutput.Values.ToList();
            SendData(consumerQuequeName, messages);
            foreach (var id in scriptOutput) {
                Console.WriteLine($"The Script with id: {id.Key} has been run");
            }
            // 0 is returned because that will reset the main method in program.cs.
            return 0;
        }

        //This method gets a queue name from the environment variable MP_QUEUENAME. The queue name is used in MessageBroker.Receive to get the list of Scripts from the queue name.
        private List<Script> GetScriptsFromQueue(string queueName) {
            try {
                return MessageBroker.Receive(queueName);
            }
            catch (Exception e) {
                _log.LogError(e, "Unable to get scripts from queue");
                return null;
            }
        }

        //This method takes a queue name and a message (scripts) and sends it into a queue in the messagebroker. 
        private void SendData(string queueName, IEnumerable<string> messages) {
            try {
                MessageBroker.Send<string>(queueName, messages);
            }
            catch (Exception e) {
                _log.LogError(e, "unable to send results to queue");
            }
        }
    }
}
