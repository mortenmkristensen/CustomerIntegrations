using Models;
using System.Collections.Generic;

namespace Database {
    public interface IDBAccess {
        void Upsert(Script script);
        void Delete(Script script);
        Script GetScriptById(string id);
        IEnumerable<Script> GetByLanguage(string language);
        IEnumerable<Script> GetAll();

    }
}
