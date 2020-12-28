using System;
using System.Collections.Generic;
using System.Text;
using Models;
using Newtonsoft.Json;
using Core.Exceptions;

namespace Core {
    public class DataValidation : IDataValidation{

        public bool ValidateScriptOutput(string scriptResult) {
            bool isLocation = false;
            try {
                JsonConvert.DeserializeObject<List<Location>>(scriptResult);
                isLocation = true;
            } catch (JsonException) {
                throw new ScriptFailedException("Cannot deserialize result: " + scriptResult);
            }
            return isLocation;
        }
    }
}
