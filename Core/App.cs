using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Models;

namespace Core {
    class App {

        private Stager Stager { get; set; }
        private ScriptRunner ScriptRunner { get; set; }

        private string Ids { get; set; } 
        public App(Stager stager, ScriptRunner scriptRunner, string ids) {
            Stager = stager;
            ScriptRunner = scriptRunner;
            Ids = ids;
        }

        public void Run(string interpreterPath) {
            List<Script> scripts =(List<Script>) Stager.GetScriptsByIds(DeserializeIds());
            Dictionary<string, string> paths = Stager.GetPaths(scripts);
            Dictionary<string, string> scriptOutput = ScriptRunner.RunScripts(paths, interpreterPath);
            foreach (var script in scriptOutput) {
                Console.WriteLine(script + "\n\n");
                
            }
        }

        private List<string> DeserializeIds() {
            return JsonConvert.DeserializeObject<List<string>>(Ids);
        }
    }
}
