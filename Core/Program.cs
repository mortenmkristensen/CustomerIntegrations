using Core.Database;
using System;
using System.Collections;

namespace Core {
    class Program {
        static void Main(string[] args) { //dont know how to send a list from outside
            IDBAccess dbAccess = new DBAccess();
            Stager stager = new Stager(dbAccess);
            ScriptRunner scriptRunner = new ScriptRunner();
            App app = new App(stager, scriptRunner);
            string interpreterPath = ""; //set denne via env variabel
            app.Run(interpreterPath);
        }
    }
}
