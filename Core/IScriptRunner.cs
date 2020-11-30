using System.Collections.Generic;

namespace Core {
    interface IScriptRunner {
        Dictionary<string,string> RunScripts(Dictionary<string, string> scripts, string interpreterPath);
    }
}