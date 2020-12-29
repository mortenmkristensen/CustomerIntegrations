using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Database;
using Microsoft.Extensions.Hosting;
using Models;
using MessageBroker;
using System.Linq;

namespace Scheduling {
    public class Scheduler : IHostedService {
        private IDBAccess _dbAccess;
        private IMessageBroker _messageBroker;
        private Timer _timer;
        private Dictionary<string, List<Script>> scriptsSeperetedByLangugage;
        private List<Script> scripts;

        public Scheduler(IDBAccess dBAccess,  IMessageBroker messageBroker) {
            _dbAccess = dBAccess;
            _messageBroker = messageBroker;
            scripts = new List<Script>();
            scriptsSeperetedByLangugage = new Dictionary<string, List<Script>>();
        }
        //this method is from the IHostedService interface and is called when the program is started.
        //it makes a timer that executes the run method every 5 seconds. 
        public Task StartAsync(CancellationToken cancellationToken) {
            _timer = new Timer(
                Run, //method to be executed
                null, //argument for the method to be executed
                TimeSpan.Zero, //start delay
                TimeSpan.FromSeconds(5) //frequency of executions
                );

            return Task.CompletedTask;
        }

        //this method is from the IHostedService interface and is called when the program is stopped.
        //it stops the timer by setting the start deley of the timer to infinite and the time between executions to 0.
        public Task StopAsync(CancellationToken cancellationToken) {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;

        }
        //Run takes care of calling the other methods in the right order. it has an object as parameter but it is not used. 
        //this is because it is used as the callback for the timer, and the delegate for that specifies that it has to take an object in its signature.
        private void Run(object state) {
            GetNewScripts();
            SeparateByLanguage();
            try {
                //each language specific list is sent one at a time to the messagebroker
                foreach (var scriptList in scriptsSeperetedByLangugage) {
                    SendToRabbitMQ(scriptList.Value);
                }
            } catch (InvalidOperationException) {
                //logging
            } finally {
                ClearLists();
            }
        }

        //this method seperates the list of all scripts into smaller lists, one for each language supported by the system
        private void SeparateByLanguage() {
            foreach (var script in scripts) {
                //a list with scripts written in a specific language already exixts
                if (scriptsSeperetedByLangugage.ContainsKey(script.Language)) {
                    List<Script> scriptList;
                    scriptsSeperetedByLangugage.TryGetValue(script.Language, out scriptList);
                    List<Script> temp = new List<Script>(scriptList);
                    temp.Add(script);
                    scriptsSeperetedByLangugage[script.Language] = temp.Distinct().ToList();
                //a list with scripts in a specific language does not exist
                } else {
                    List<Script> scriptList = new List<Script>();
                    scriptList.Add(script);
                    scriptsSeperetedByLangugage.Add(script.Language, scriptList);
                }
                
            }
        }

        //this method takes a list of objects that should be split into smaller lists and an integer specifying how many elements should be in each list
        //it returns a list containing all the samller lists
        private IEnumerable<List<T>> SplitList<T>(List<T> items, int nSize) {
            for (int i = 0; i < items.Count; i += nSize) {
                yield return items.GetRange(i, Math.Min(nSize, items.Count - i));
            }
        }

        //this method takes a list of scripts and uses split list to split them into smaller lists and send them one at a time to the the queue in the messagebroker specified by the envirnonment variable MP_SCHEDULINGQUEUE
        private void SendToRabbitMQ(List<Script> scripts) {
            var scriptLists = SplitList<Script>(scripts, int.Parse(Environment.GetEnvironmentVariable("MP_CHUNKSIZE")));
            string queueName = Environment.GetEnvironmentVariable("MP_SCHEDULINGQUEUE");
            foreach (var list in scriptLists) {
                _messageBroker.Send<Script>(queueName, scripts);
            }
        }

        //this method gets all the scripts from the database
        private void GetNewScripts() {
            scripts = _dbAccess.GetAll().ToList();
        }

        //this method empties each of language specific lists
        private void ClearLists() {
            foreach (var scriptList in scriptsSeperetedByLangugage) {
                scriptList.Value.Clear();
            }
        }
    }
}
