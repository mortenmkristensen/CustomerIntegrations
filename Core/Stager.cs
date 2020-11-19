using Core.Database;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Core {
    class Stager {

        public IDBAccess DBAccess { get; set; }
        public Stager(IDBAccess dBAccess) {
            DBAccess = dBAccess;
        }

        public Script GetScriptById(string id) {
            return DBAccess.GetScriptById(id);
        }

        public IEnumerable<Script> GetScriptsByIds(IEnumerable<string> ids) {
            List<Script> scripts = new List<Script>();
            foreach (var id in ids) {
                scripts.Add(GetScriptById(id));
            }
            return scripts;
        }

        public string GetPath(Script script) {
            return SaveToDisk(script);
        }
        public IEnumerable<string> GetPaths(IEnumerable<Script> scripts) {
            List<string> paths = new List<string>();
            foreach(var script in scripts) {
                paths.Add(GetPath(script));
            }
            return paths;
        }
        private string SaveToDisk(Script script) {
            string path = "";
            switch (script.Language.ToLower()) {
                case "javascript":
                    path = $@"c:\scripts\javascript\{script.Id}.js";
                    break;
                case "python":
                    path = $@"c:\scripts\python\{script.Id}.py";
                    break;
                case "ruby":
                    path = $@"c:\scripts\ruby\{script.Id}.rb";
                    break;
            }
            File.WriteAllText(path, script.Code);
            return path;
        }
    }
}
