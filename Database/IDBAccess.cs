using Models;
using System.Collections.Generic;

namespace Database {
    public interface IDBAccess {
        Script Upsert(Script script);
        void Delete(string script);
        Script GetScriptById(string id);
        IEnumerable<Script> GetByLanguage(string language);
        IEnumerable<Script> GetAll();
        IEnumerable<Script> GetByCustomer(string customer);
    }
}
