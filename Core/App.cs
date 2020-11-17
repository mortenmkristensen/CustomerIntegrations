using Core.Database;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core {
    class App {

        private Stager Stager { get; set; }
        private ScriptRunner ScriptRunner { get; set; }
        public App(Stager stager, ScriptRunner scriptRunner) {
            Stager = stager;
            ScriptRunner = scriptRunner;
        }

        public void Run(string interpreterPath, IEnumerable<string> ids) {
            List<Script> scripts =(List<Script>) Stager.GetScriptsByIds(ids); //what to send here??
            List<string> paths = (List<string>) Stager.GetPaths(scripts);
            List<string> scriptOutput = ScriptRunner.RunScripts(paths, interpreterPath);
        }
    }
}
