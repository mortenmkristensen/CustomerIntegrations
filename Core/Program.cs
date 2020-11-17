using Core.Database;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core {
    class Program {
        static void Main(string[] args) {
            IDBAccess dbAccess = new DBAccess(new DBConfig());
            Stager stager = new Stager(dbAccess);
            ScriptRunner scriptRunner = new ScriptRunner();
            string ids = Environment.GetEnvironmentVariable("MP_IDS");//set with env variables
            App app = new App(stager, scriptRunner, ids); 
            string interpreterPath = Environment.GetEnvironmentVariable("MP_INTERPRETERPATH"); //set with env virables
            app.Run(interpreterPath);
        }
    }
}
