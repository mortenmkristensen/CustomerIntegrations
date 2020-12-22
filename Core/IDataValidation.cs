using System;
using System.Collections.Generic;
using System.Text;

namespace Core {
    public interface IDataValidation {

        bool validateScriptOutput(string jsonScript);
    }
}
