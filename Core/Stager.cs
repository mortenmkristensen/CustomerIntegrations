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

        public Script getScriptById(string id) {
            return DBAccess.GetById(id);
        }

        public IEnumerable<Script> GetScriptsByIds(IEnumerable<string> ids) {
            List<Script> scripts = new List<Script>();
            foreach (var id in ids) {
                scripts.Add(getScriptById(id));
            }
            return scripts;
        }

        public string GetPath(Script script) {
            return SaveToDisk(script);
        }

        /*
        public IEnumerable<string> GetPaths(IEnumerable<Script> scripts) {
            List<string> paths = new List<string>();
            foreach(var script in scripts) {
                paths.Add(GetPath(script));
            }
            return paths;
        }
        */
         public IEnumerable<KeyValuePair<string,string>> GetPaths(IEnumerable<Script> scripts) {
            Dictionary<string, string> paths = new Dictionary<string, string>();
            foreach(var script in scripts) {
                paths.Add(GetPath(script), script.Language);
            }
            return paths;
        }

        //this might not be the correct way to make the files as i dont know if File.writeAllText can make anything else than txt files
        private string SaveToDisk(Script script) {
            string path = "";
            switch (script.Language) {
                case "javascript":
                    path = $@"c:\scripts\javascript\{script.Name}.js";
                    break;
                case "python":
                    path = $@"c:\scripts\python\{script.Name}.PY";
                    break;
                case "ruby":
                    path = $@"c:\scripts\ruby\{script.Name}.rb";
                    break;
            }
            File.WriteAllText(path, script.Code);
            return path;
        }
    }
}
