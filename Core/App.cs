using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Models;
using Core.Exceptions;
using Database;

namespace Core {
    class App : IApp {

        private Stager Stager { get; set; }
        private ScriptRunner ScriptRunner { get; set; }
        private string Ids { get; set; }
        IDBAccess DBAccess { get; set; }
        public App(IDBAccess dbAccess, IStager stager, IScriptRunner scriptRunner) {
            Stager = stager;
            ScriptRunner = scriptRunner;
            Ids = Environment.GetEnvironmentVariable("MP_IDS");
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
    }
}
