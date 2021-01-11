using Models;
using System.Collections.Generic;

namespace Core {
    public interface IStager {
        Dictionary<string, string> GetPaths(IEnumerable<Script> scripts);
    }
}