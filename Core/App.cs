using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Models;
using Database;

namespace Core {
    class App : IApp {

        private IStager Stager { get; set; }
        private IScriptRunner ScriptRunner { get; set; }

        private string Ids { get; set; }
        public App(IStager stager, IScriptRunner scriptRunner) {
            Stager = stager;
            ScriptRunner = scriptRunner;
            Ids = Environment.GetEnvironmentVariable("MP_IDS");
        }

        public void Run(string interpreterPath) {
            List<Script> scripts = (List<Script>)Stager.GetScriptsByIds(DeserializeIds());
            List<string> paths = (List<string>)Stager.GetPaths(scripts);
            List<string> scriptOutput = ScriptRunner.RunScripts(paths, interpreterPath);
            foreach (var script in scriptOutput) {
                Console.WriteLine(script + "\n\n");

            }
        }

        private List<string> DeserializeIds() {
            return JsonConvert.DeserializeObject<List<string>>(Ids);
        }
    }
}
