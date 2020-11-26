using API.Models;
using System.Collections.Generic;

namespace API.Database {
    public interface IDBAccess {
        void Upsert(Script script);
        void Delete(string script);
        Script GetScriptById(string id);
        IEnumerable<Script> GetByLanguage(string language);
        IEnumerable<Script> GetAll();
        IEnumerable<Script> GetByCustomer(string customer);
    }
}
