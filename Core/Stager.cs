using Models;
using System.Collections.Generic;
using System.IO;

namespace Core {
    //This class prepares the scripts before they are are used by ScriptRunner, by extracting them from the database and then saving them to disk. 
    class Stager : IStager {
        //This method creates a new Dictionary that contains all the paths that SaveToDisk has made, and returns it. 
        public Dictionary<string, string> GetPaths(IEnumerable<Script> scripts) {
            Dictionary<string, string> paths = new Dictionary<string, string>();
            foreach(var script in scripts) {
                paths.Add(script.Id,SaveToDisk(script));
            }
            return paths;
        }

        //This method saves the script in a folder on the disk (different folder depending on the language), and returns the path to the script. 
        private string SaveToDisk(Script script) {
            string path = "";
            switch (script.Language.ToLower()) {
                case "javascript":
                    path = $@"/root/scripts/javascript/{script.Id}.js";
                    break;
                case "python":
                    path = $@"/root/scripts/python/{script.Id}.py";
                    break;
                case "ruby":
                    path = $@"/root/scripts/ruby/{script.Id}.rb";
                    break;
            }
            File.WriteAllText(path, script.Code);
            return path;
        }
    }
}

