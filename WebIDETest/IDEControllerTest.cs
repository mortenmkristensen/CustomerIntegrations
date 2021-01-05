using System;
using Microsoft.AspNetCore.Mvc;
using WebIDE.Controllers;
using Xunit;

namespace WebIDETest {
    public class IDEControllerTest {
        private readonly IDEController iDEController;
        public IDEControllerTest() {
            iDEController = new IDEController();
        }
        [Fact]
        public void ScriptStateTest() {
            //Arrange 
            string scriptId1 = "5ff437f0fd1dc0267d7eac57";
            string scriptId2 = "cc";

            //Act
            var result1 = iDEController.ScriptState(scriptId1) as ViewResult;
            var result2 = iDEController.ScriptState(scriptId2) as ViewResult;

            //Assert
            Assert.Equal("ScriptSate", result1.ViewName);
            //Assert.Equal("EditScript", result2.ViewName);
        }
    }
}
