using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace WebIDE.ServiceAccess {
    public interface IAPIAccess {    
        List<Script> GetAllScripts();
        Script GetScriptById(string id);
        Script UploadScript(Script script);
        bool DeleteScript(string id);
    }
}
