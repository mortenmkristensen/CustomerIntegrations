using System;
using System.Collections.Generic;
using System.Text;
using Models;
using Newtonsoft.Json;

namespace Core {
    public class DataValidation : IDataValidation{

        public bool validateScriptOutput(string jsonScript) {
            bool isLocation = false;
            try {
                JsonConvert.DeserializeObject<Location>(jsonScript);
                isLocation = true;
                return isLocation;
            } catch (JsonException) {
                isLocation = false;
                return isLocation;
            }
        }
    }
}
