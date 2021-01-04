using Models;
using System.Collections.Generic;

namespace Core {
    public interface IStager {
        string GetPath(Script script);
        Dictionary<string, string> GetPaths(IEnumerable<Script> scripts);
    }
}