using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Xunit;

namespace CoreTest {
    public class ScriptRunnerTest {
        private readonly ScriptRunner scriptRunner;
        [Fact]
        public void RunScriptTest() {
            //Arrange
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

            //Act
            string result1 = scriptRunner.RunScript(scriptId, scriptPath, interpreterPath);
            string result2 = scriptRunner.RunScript(scriptId2, scriptPath2, interpreterPath);
            string result3 = scriptRunner.RunScript(scriptId3, scriptPath3, interpreterPath);
            string result4 = scriptRunner.RunScript(scriptId4, scriptPath4, interpreterPath2);
            string result5 = scriptRunner.RunScript(scriptId5, scriptPath5, interpreterPath2);
            string result6 = scriptRunner.RunScript(scriptId6, scriptPath6, interpreterPath2);
            string result7 = scriptRunner.RunScript(scriptId7, scriptPath7, interpreterPath3);
            string result8 = scriptRunner.RunScript(scriptId8, scriptPath7, interpreterPath3);
            string result9 = scriptRunner.RunScript(scriptId9, scriptPath9, interpreterPath3);

            //Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.Null(result3);
            Assert.NotNull(result4);
            Assert.NotNull(result5);
            Assert.Null(result6);
            Assert.NotNull(result7);
            Assert.NotNull(result8);
            Assert.Null(result9);
        }
    
}
