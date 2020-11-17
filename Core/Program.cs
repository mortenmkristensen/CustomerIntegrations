using Core.Database;
using System;
using System.Collections;

namespace Core {
    class Program {
        static void Main(string[] args) {
            IDBAccess dbAccess = new DBAccess();
            Stager stager = new Stager(dbAccess);
            ScriptRunner scriptRunner = new ScriptRunner();
            string ids = "";
            App app = new App(stager, scriptRunner, ids); //set with env variables
            string interpreterPath = ""; //set with env virables
            app.Run(interpreterPath);
        }
    }
}
