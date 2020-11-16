using Core.Database;
using System;

namespace Core {
    class Program {
        static void Main(string[] args) {
            IDBAccess dbAccess = new DBAccess();
            Stager stager = new Stager(dbAccess);
            ScriptRunner scriptRunner = new ScriptRunner();
            App app = new App(stager, scriptRunner);
            app.Run();
        }
    }
}
