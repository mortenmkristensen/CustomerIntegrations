using Database;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
namespace Core {
    class Stager : IStager {

        public string GetPath(Script script) {
            return SaveToDisk(script);
        }
        public Dictionary<string, string> GetPaths(IEnumerable<Script> scripts) {
            Dictionary<string, string> paths = new Dictionary<string, string>();
            foreach(var script in scripts) {
                paths.Add(script._id,GetPath(script));
            }
            return paths;
        }
        private string SaveToDisk(Script script) {
            string path = "";
            switch (script.Language.ToLower()) {
                case "javascript":
                    path = $@"c:\scripts\javascript\{script._id}.js";
                    break;
                case "python":
                    path = $@"c:\scripts\python\{script._id}.py";
                    break;
                case "ruby":
                    path = $@"c:\scripts\ruby\{script._id}.rb";
                    break;
            }
            File.WriteAllText(path, script.Code);
            return path;
        }
    }
}
