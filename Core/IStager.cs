using Models;
using System.Collections.Generic;

namespace Core {
    interface IStager {
        Dictionary<string, string> GetPaths(IEnumerable<Script> scripts);
    }
}