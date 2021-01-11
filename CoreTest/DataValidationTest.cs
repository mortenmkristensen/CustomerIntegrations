using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Core.Exceptions;
using Moq;
using Xunit;

namespace CoreTest {
    public class DataValidationTest {
  
        [Fact]
        public void ValidateScriptOutputTest() {
            //Arrange
            var dataValidation = new Mock<IDataValidation>();
            string scriptResult = "[{" + '\u0022' + "Id" + '\u0022' + ":" + '\u0022' + "" + '\u0022' +
                "," + '\u0022' + "ParentId" + '\u0022' + ":" + '\u0022' + "" + '\u0022' + "," + '\u0022' + "ExternalId" + '\u0022'
                + ":" + '\u0022' + "9894778d - 7ed6 - 4610 - 987a - 033b8125a24e" + '\u0022' + "," +
                '\u0022' + "ConsumerId" + '\u0022' + ":3," + '\u0022' + "Sources" + '\u0022' +
                ":[{" + '\u0022' + "Type" + '\u0022' + ":" + '\u0022' + "Occupancy" + '\u0022' + "," +
                '\u0022' + "States" + '\u0022' + ":[{" + '\u0022' + "Property" + '\u0022' + ":" + '\u0022' + "MotionDetected" + '\u0022' +
                "," + '\u0022' + "Value" + '\u0022' + ":false},{" + '\u0022' + "Property" + '\u0022' + ":" +
                '\u0022' + "PersonCount" + '\u0022' + "," + '\u0022' + "Value" + '\u0022' + ":3349},{" + '\u0022' + "Property" + '\u0022' +
                ":" + '\u0022' + "SignsOfLife" + '\u0022' + "," + '\u0022' + "Value" + '\u0022' + ":false}]," + '\u0022' + "TimeStamp" + '\u0022' +
                ":" + '\u0022' + "2020 - 12 - 30T13: 47:32.767689Z" + '\u0022' + "}]}]";
            string scriptResult2 = "try test";
            string scriptResult3 = null;

            dataValidation.Setup(x => x.ValidateScriptOutput(scriptResult)).Returns(true);
            dataValidation.Setup(x => x.ValidateScriptOutput(scriptResult2)).Throws(new ScriptFailedException());
            dataValidation.Setup(x => x.ValidateScriptOutput(scriptResult3)).Throws(new ScriptFailedException());

            //Act and Assert
            Assert.True(dataValidation.Object.ValidateScriptOutput(scriptResult));
            Assert.Throws<ScriptFailedException>(() => dataValidation.Object.ValidateScriptOutput(scriptResult2));
            Assert.Throws<ScriptFailedException>(() => dataValidation.Object.ValidateScriptOutput(scriptResult3));
        }
    }
}
