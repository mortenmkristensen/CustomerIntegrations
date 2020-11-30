using Models;
using System.Collections.Generic;

namespace Core {
    interface IStager {
        string GetPath(Script script);
        Dictionary<string, string> GetPaths(IEnumerable<Script> scripts);
    }
}