using System.Collections.Generic;

namespace Core {
    interface IScriptRunner {
        List<string> RunScripts(List<string> scriptPaths, string interpreterPath);
    }
}