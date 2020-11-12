using System;
using System.Collections.Generic;
using System.Text;

namespace Core {
    interface IStorageAccess<T> {
        void Insert();
        void Delete(T t);
        void Update(T t);
        IEnumerable<string> GetScriptPaths(string path);
    }
}
