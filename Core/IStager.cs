using Models;
using System.Collections.Generic;

namespace Core {
    interface IStager {
        string GetPath(Script script);
        IEnumerable<string> GetPaths(IEnumerable<Script> scripts);
        Script GetScriptById(string id);
        IEnumerable<Script> GetScriptsByIds(IEnumerable<string> ids);
    }
}