using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Core.Exceptions;
using Moq;
using Xunit;

namespace CoreTest {
    public class ScriptRunnerTest {
      
        [Fact]
        public void RunScriptTest() {
            //Arrange
            var scriptRunner = new Mock<IScriptRunner>();
            //python
            string scriptId = "1";
            string scriptPath = @"C:\scripts\\python\1.py";
            string interpreterPath = "python";
            //Script with wrong dataformat.
            string scriptId2 = "2";
            string scriptPath2 = @"C:\scripts\python\2.py";
            //script with error
            string scriptId3 = "3";
            string scriptPath3 = @"C:\scripts\python\3.py";

            //ruby
            string scriptId4 = "4";
            string scriptPath4 = @"C:\scripts\ruby\4.rb";
            string interpreterPath2 = "ruby";
            //Script with wrong dataformat.
            string scriptId5 = "5";
            string scriptPath5 = @"C:\scripts\ruby\5.rb";
            //script with error
            string scriptId6 = "6";
            string scriptPath6 = @"C:\scripts\ruby\6.rb";

            //javascript
            string scriptId7 = "7";
            string scriptPath7 = @"C:\scripts\javascript\7.js";
            string interpreterPath3 = "node";
            //Script with wrong dataformat.
            string scriptId8 = "8";
            string scriptPath8 = @"C:\scripts\javascript\8.js";
            //script with error
            string scriptId9 = "9";
            string scriptPath9 = @"C:\scripts\javascript\9.js";
            var result = "[{" + '\u0022' + "Id" + '\u0022' + ":" + '\u0022' + "" + '\u0022' +
               "," + '\u0022' + "ParentId" + '\u0022' + ":" + '\u0022' + "" + '\u0022' + "," + '\u0022' + "ExternalId" + '\u0022'
               + ":" + '\u0022' + "9894778d - 7ed6 - 4610 - 987a - 033b8125a24e" + '\u0022' + "," +
               '\u0022' + "ConsumerId" + '\u0022' + ":3," + '\u0022' + "Sources" + '\u0022' +
               ":[{" + '\u0022' + "Type" + '\u0022' + ":" + '\u0022' + "Occupancy" + '\u0022' + "," +
               '\u0022' + "States" + '\u0022' + ":[{" + '\u0022' + "Property" + '\u0022' + ":" + '\u0022' + "MotionDetected" + '\u0022' +
               "," + '\u0022' + "Value" + '\u0022' + ":false},{" + '\u0022' + "Property" + '\u0022' + ":" +
               '\u0022' + "PersonCount" + '\u0022' + "," + '\u0022' + "Value" + '\u0022' + ":3349},{" + '\u0022' + "Property" + '\u0022' +
               ":" + '\u0022' + "SignsOfLife" + '\u0022' + "," + '\u0022' + "Value" + '\u0022' + ":false}]," + '\u0022' + "TimeStamp" + '\u0022' +
               ":" + '\u0022' + "2020 - 12 - 30T13: 47:32.767689Z" + '\u0022' + "}]}]";

            //Act
            scriptRunner.Setup(x => x.RunScript(scriptId, scriptPath, interpreterPath)).Returns(result);
            scriptRunner.Setup(x => x.RunScript(scriptId2, scriptPath2, interpreterPath)).Returns(result);
            scriptRunner.Setup(x => x.RunScript(scriptId3, scriptPath3, interpreterPath)).Throws(new ScriptFailedException());
            scriptRunner.Setup(x => x.RunScript(scriptId4, scriptPath4, interpreterPath2)).Returns(result);
            scriptRunner.Setup(x => x.RunScript(scriptId5, scriptPath5, interpreterPath2)).Returns(result);
            scriptRunner.Setup(x => x.RunScript(scriptId6, scriptPath6, interpreterPath2)).Throws(new ScriptFailedException());
            scriptRunner.Setup(x => x.RunScript(scriptId7, scriptPath7, interpreterPath3)).Returns(result);
            scriptRunner.Setup(x => x.RunScript(scriptId8, scriptPath8, interpreterPath3)).Returns(result);
            scriptRunner.Setup(x => x.RunScript(scriptId9, scriptPath9, interpreterPath3)).Throws(new ScriptFailedException());


            //Assert
            Assert.NotNull(scriptRunner.Object.RunScript(scriptId, scriptPath, interpreterPath));
            Assert.NotNull(scriptRunner.Object.RunScript(scriptId2, scriptPath2, interpreterPath));
            Assert.Throws<ScriptFailedException>(() => scriptRunner.Object.RunScript(scriptId3, scriptPath3, interpreterPath));
            Assert.NotNull(scriptRunner.Object.RunScript(scriptId4, scriptPath4, interpreterPath2));
            Assert.NotNull(scriptRunner.Object.RunScript(scriptId5, scriptPath5, interpreterPath2));
            Assert.Throws<ScriptFailedException>(() => scriptRunner.Object.RunScript(scriptId6, scriptPath6, interpreterPath2));
            Assert.NotNull(scriptRunner.Object.RunScript(scriptId7, scriptPath7, interpreterPath3));
            Assert.NotNull(scriptRunner.Object.RunScript(scriptId8, scriptPath8, interpreterPath3));
            Assert.Throws<ScriptFailedException>(() => scriptRunner.Object.RunScript(scriptId9, scriptPath9, interpreterPath3));
        }
    }
}
