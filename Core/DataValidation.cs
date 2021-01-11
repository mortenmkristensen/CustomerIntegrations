using System.Collections.Generic;
using Models;
using Newtonsoft.Json;
using Core.Exceptions;

namespace Core {

    //This class is used to check if scripts are in the correct format.
    public class DataValidation : IDataValidation{

        //This method deserializes the script, and if it succeeds the bool isLocation is true and returned. If it fails an exception it thrown. 
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
