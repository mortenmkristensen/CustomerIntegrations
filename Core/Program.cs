using Database;
using System;

namespace Core {
    class Program {
        static void Main(string[] args) {
            IDBAccess dbAccess = new DBAccess(new DBConfig());
            Stager stager = new Stager();
            ScriptRunner scriptRunner = new ScriptRunner();
            string ids = Environment.GetEnvironmentVariable("MP_IDS");//set with env variables
            App app = new App(dbAccess, stager, scriptRunner, ids); 
            string interpreterPath = Environment.GetEnvironmentVariable("MP_INTERPRETERPATH"); //set with env virables
            app.Run(interpreterPath);
        }
    }
}
