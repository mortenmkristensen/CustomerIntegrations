using Models;
using System.Collections.Generic;

namespace Core {
    interface IApp {
        void Run(string interpreterPath, List<Script> scripts);
        string GetScriptsFromScheduler();
        void Listen(string interpreterPath);
    }
}