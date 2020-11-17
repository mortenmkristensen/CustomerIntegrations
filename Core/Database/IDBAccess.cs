using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Database {
    interface IDBAccess {
        void Upsert(Script script);
        void Delete(Script script);
        Script GetScriptById(string id);
        IEnumerable<Script> GetByLanguage(string language);
        IEnumerable<Script> GetAll();

    }
}
