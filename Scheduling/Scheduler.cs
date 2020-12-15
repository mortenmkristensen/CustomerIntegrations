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
        private List<Script> rubyScripts = new List<Script>();
        private List<Script> pythonScripts = new List<Script>();
        private List<Script> jsScripts = new List<Script>();
        private List<Script> scripts = new List<Script>();

        public Scheduler(IDBAccess dBAccess,  IMessageBroker messageBroker) {
            _dbAccess = dBAccess;
            _messageBroker = messageBroker;
        }
        public async Task StartAsync(CancellationToken cancellationToken) {
            _timer = new Timer(
                Run,
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(5)
                );
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;

        }

        private async void Run(object state) {
            GetNewScripts();
            SeparateByLanguage();
            SendToRabbitMQ(rubyScripts);
            SendToRabbitMQ(pythonScripts);
            SendToRabbitMQ(jsScripts);
            ClearLists();
        }

        private void SeparateByLanguage() {
            List<Script> tempRuby = new List<Script>();
            List<Script> tempPython = new List<Script>();
            List<Script> tempJs = new List<Script>();
            foreach (var script in scripts) {
                if (script.Language.Equals("ruby")) {
                    tempRuby.Add(script);
                }
                if (script.Language.Equals("python")) {
                    tempPython.Add(script);
                }
                if (script.Language.Equals("javascript")) {
                    tempJs.Add(script);
                }
            }
            rubyScripts = tempRuby.Distinct().ToList();
            pythonScripts = tempPython.Distinct().ToList();
            jsScripts = tempJs.Distinct().ToList();
        }

        private void SendWithRabbitMQ(string queueName, List<Script> scripts) {
            _messageBroker.Send<Script>(queueName, scripts);
        }

        private IEnumerable<List<T>> SplitList<T>(List<T> ids, int nSize) {
            for (int i = 0; i < ids.Count; i += nSize) {
                yield return ids.GetRange(i, Math.Min(nSize, ids.Count - i));
            }
        }

        private async Task SendToRabbitMQ(List<Script> scripts) {
            var scriptLists = SplitList<Script>(scripts, 100);
            string queueName = "Scheduling_Queue";
            foreach (var list in scriptLists) {
                SendWithRabbitMQ(queueName, list);
            }
        }

        private void GetNewScripts() {
            List<Script> allScripts = _dbAccess.GetAll().ToList();
            scripts = allScripts.Except(scripts).ToList();
        }

        private void ClearLists() {
            rubyScripts.Clear();
            pythonScripts.Clear();
            jsScripts.Clear();
        }
    }
}
