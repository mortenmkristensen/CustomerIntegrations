using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Database;
using Microsoft.Extensions.Hosting;
using Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
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
        public Task StartAsync(CancellationToken cancellationToken) {
            _timer = new Timer(
                Run,
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(5)
                );

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;

        }

        private void Run(object state) {
            GetNewScripts();
            SeparateByLanguage();
            foreach (var scriptList in scriptsSeperetedByLangugage) {
                SendToRabbitMQ(scriptList.Value);
            }
            ClearLists();
        }

        private void SeparateByLanguage() {
            foreach (var script in scripts) {
                if (!scriptsSeperetedByLangugage.ContainsKey(script.Language)) {
                    List<Script> scriptList = new List<Script>();
                    scriptList.Add(script);
                    scriptsSeperetedByLangugage.Add(script.Language, scriptList);
                } else {
                    List<Script> scriptList;
                    scriptsSeperetedByLangugage.TryGetValue(script.Language, out scriptList);
                    scriptList.Add(script);
                    scriptsSeperetedByLangugage[script.Language] = scriptList;
                }
            }
        }

        //private void SeparateByLanguage() {
        //    List<List<Temp>
        //    List<Script> tempRuby = new List<Script>();
        //    List<Script> tempPython = new List<Script>();
        //    List<Script> tempJs = new List<Script>();

        //    foreach (var script in scripts) {
        //        if (script.Language.Equals("ruby")) {
        //            tempRuby.Add(script);
        //        }
        //        if (script.Language.Equals("python")) {
        //            tempPython.Add(script);
        //        }
        //        if (script.Language.Equals("javascript")) {
        //            tempJs.Add(script);
        //        }
        //    }
        //    someScripts.Add(new List<Script> { tempRuby });
        //    someScripts.Add(new List<Script> { tempPython });
        //    someScripts.Add(new List<Script> { tempJs });
        //}


        private IEnumerable<List<T>> SplitList<T>(List<T> itmes, int nSize) {
            for (int i = 0; i < itmes.Count; i += nSize) {
                yield return itmes.GetRange(i, Math.Min(nSize, itmes.Count - i));
            }
        }

        private void SendToRabbitMQ(List<Script> scripts) {
            var scriptLists = SplitList<Script>(scripts, int.Parse(Environment.GetEnvironmentVariable("MP_CHUNKSIZE")));
            string queueName = "Scheduling_Queue";
            foreach (var list in scriptLists) {
                _messageBroker.Send<Script>(queueName, list);
            }
        }

        private void GetNewScripts() {
            scripts = _dbAccess.GetAll().ToList();
        }

        private void ClearLists() {
            foreach (var scriptList in scriptsSeperetedByLangugage) {
                scriptList.Value.Clear();
            }
        }
    }
}
