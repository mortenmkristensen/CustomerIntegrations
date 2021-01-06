using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using WebIDE.Controllers;
using WebIDE.ServiceAccess;
using Xunit;

namespace WebIDETest {
    public class IDEControllerTest {
        private readonly IDEController iDEController;
        private readonly Mock<IAPIAccess> aPIAccessMock = new Mock<IAPIAccess>();
        public IDEControllerTest() {
            iDEController = new IDEController(aPIAccessMock.Object);
        }
        [Fact]
        public void ScriptStateTest() {
            //Arrange 
            string scriptId1 = "1";
            string scriptId2 = "cc";
            Script script1 = new Script() {
                Id = "1",
                Name = "Python1",
                Customer = "testCustomer",
                ScriptVersion = "1.11",
                Language = "python",
                LanguageVersion = "1.0.1",
                Code = "#This is the datamodel for Location, Source, and State." + "\n" +
                       "location = { 'Id':" + '\u0022' + "" + '\u0022' + "," + "'ParentId':" + '\u0022' + "" + '\u0022' + "," + "'ExternalId':" + '\u0022' + "" + '\u0022' + "," + "'ConsumerId': 2, 'Sources': [] }" + "\n" +
                        "source = { 'Type':" + '\u0022' + "" + '\u0022' + "," + "'TimeStamp':" + '\u0022' + "" + '\u0022' + "," + "'State': [] }" + "\n" +
                        "state = { 'Property':" + '\u0022' + "" + '\u0022' + "," + "'Value':" + '\u0022' + "" + '\u0022' + "," + "}",
                DateCreated = DateTime.Now,
                Author = "test1",
                LastModified = DateTime.Now
            };
            Script script2 = null;

            aPIAccessMock.Setup(x => x.GetScriptById(scriptId1)).Returns(script1);
            aPIAccessMock.Setup(x => x.GetScriptById(scriptId2)).Returns(script2);

            //Act
            var result1 = iDEController.ScriptState(scriptId1) as ViewResult;
            var script = (Script)result1.ViewData.Model;
            var result2 = iDEController.ScriptState(scriptId2) as ViewResult;

            //Assert
            Assert.Equal("ScriptState", result1.ViewName);
            Assert.Equal("Python1", script.Name);
            Assert.Equal("EditScript", result2.ViewName);
        }

        [Fact]
        public void OpenScriptsTest() {
            //Arrange 
            List<Script> scripts1 = new List<Script>();
            Script script1 = new Script() {
                Id = "1",
                Name = "Python1",
                Customer = "testCustomer",
                ScriptVersion = "1.11",
                Language = "python",
                LanguageVersion = "1.0.1",
                Code = "#This is the datamodel for Location, Source, and State." + "\n" +
                       "location = { 'Id':" + '\u0022' + "" + '\u0022' + "," + "'ParentId':" + '\u0022' + "" + '\u0022' + "," + "'ExternalId':" + '\u0022' + "" + '\u0022' + "," + "'ConsumerId': 2, 'Sources': [] }" + "\n" +
                        "source = { 'Type':" + '\u0022' + "" + '\u0022' + "," + "'TimeStamp':" + '\u0022' + "" + '\u0022' + "," + "'State': [] }" + "\n" +
                        "state = { 'Property':" + '\u0022' + "" + '\u0022' + "," + "'Value':" + '\u0022' + "" + '\u0022' + "," + "}",
                DateCreated = DateTime.Now,
                Author = "test1",
                LastModified = DateTime.Now
            };
            scripts1.Add(script1);

            aPIAccessMock.Setup(x => x.GetAllScripts()).Returns(scripts1);

            //Act 
            var result = iDEController.OpenScripts() as ViewResult;
            var scripts = (List<Script>)result.ViewData.Model;

            //Assert
            Assert.Equal("OpenScripts", result.ViewName);
            Assert.Equal("Python1", scripts[0].Name);
        }

        [Fact]
        public void SearchScriptByIdTest() {
            //Arrange 
            string scriptId1 = "1";
            string scriptId2 = "cc";
            Script script1 = new Script() {
                Id = "1",
                Name = "Python1",
                Customer = "testCustomer",
                ScriptVersion = "1.11",
                Language = "python",
                LanguageVersion = "1.0.1",
                Code = "#This is the datamodel for Location, Source, and State." + "\n" +
                       "location = { 'Id':" + '\u0022' + "" + '\u0022' + "," + "'ParentId':" + '\u0022' + "" + '\u0022' + "," + "'ExternalId':" + '\u0022' + "" + '\u0022' + "," + "'ConsumerId': 2, 'Sources': [] }" + "\n" +
                        "source = { 'Type':" + '\u0022' + "" + '\u0022' + "," + "'TimeStamp':" + '\u0022' + "" + '\u0022' + "," + "'State': [] }" + "\n" +
                        "state = { 'Property':" + '\u0022' + "" + '\u0022' + "," + "'Value':" + '\u0022' + "" + '\u0022' + "," + "}",
                DateCreated = DateTime.Now,
                Author = "test1",
                LastModified = DateTime.Now
            };
            Script script2 = null;

            aPIAccessMock.Setup(x => x.GetScriptById(scriptId1)).Returns(script1);
            aPIAccessMock.Setup(x => x.GetScriptById(scriptId2)).Returns(script2);


            //Act
            var result1 = iDEController.SearchScriptById(scriptId1) as ViewResult;
            var scripts = (List<Script>)result1.ViewData.Model;
            var result2 = iDEController.SearchScriptById(scriptId2) as ViewResult;

            //Assert
            Assert.Equal("OpenScripts", result1.ViewName);
            Assert.Equal("Python1", scripts[0].Name);
            Assert.Equal("EditScript", result2.ViewName);
        }
        //This test tests the SaveScript method, when API run.
        [Fact]
        public void SaveScriptTest1() {
            //Arrange
            string code1 = "#This is the datamodel for Location, Source, and State." + "\n" +
                      "location = { 'Id':" + '\u0022' + "" + '\u0022' + "," + "'ParentId':" + '\u0022' + "" + '\u0022' + "," + "'ExternalId':" + '\u0022' + "" + '\u0022' + "," + "'ConsumerId': 2, 'Sources': [] }" + "\n" +
                       "source = { 'Type':" + '\u0022' + "" + '\u0022' + "," + "'TimeStamp':" + '\u0022' + "" + '\u0022' + "," + "'State': [] }" + "\n" +
                       "state = { 'Property':" + '\u0022' + "" + '\u0022' + "," + "'Value':" + '\u0022' + "" + '\u0022' + "," + "}";
            Script script = new Script() {
                Id = "1",
                Name = "Python1",
                ScriptVersion = "1.11",
                Language = "python",
                Code = code1,
                Author = "test1",
                LastModified = DateTime.Now,
                DateCreated = DateTime.Now
            };
            Mock<IFormCollection> forms = new Mock<IFormCollection>();
            forms.Setup(x => x["id"]).Returns("1");
            forms.Setup(x => x["scriptName"]).Returns("Python1");
            forms.Setup(x => x["language"]).Returns("Python");
            forms.Setup(x => x["version"]).Returns("1.11");
            forms.Setup(x => x["dateCreated"]).Returns(DateTime.Now.ToString());
            forms.Setup(x => x["editorContent"]).Returns(code1);
            forms.Setup(x => x["author"]).Returns("test1");
            forms.Setup(x => x["lastModified"]).Returns(DateTime.Now.ToString());

            aPIAccessMock.Setup(x => x.UploadScript(script)).Returns(script);

            //Act
            var result1 = iDEController.SaveScript(forms.Object) as ViewResult;
            var script2 = (Script)result1.ViewData.Model;

            //Assert
            Assert.Equal("SaveScript", result1.ViewName);
            Assert.Equal("Python1", script2.Name);
        }

        //This test tests the SaveScript method, when API is not run.
        [Fact]
        public void SaveScriptTest2() {
            //Arrange
            string code1 = "#This is the datamodel for Location, Source, and State." + "\n" +
                      "location = { 'Id':" + '\u0022' + "" + '\u0022' + "," + "'ParentId':" + '\u0022' + "" + '\u0022' + "," + "'ExternalId':" + '\u0022' + "" + '\u0022' + "," + "'ConsumerId': 2, 'Sources': [] }" + "\n" +
                       "source = { 'Type':" + '\u0022' + "" + '\u0022' + "," + "'TimeStamp':" + '\u0022' + "" + '\u0022' + "," + "'State': [] }" + "\n" +
                       "state = { 'Property':" + '\u0022' + "" + '\u0022' + "," + "'Value':" + '\u0022' + "" + '\u0022' + "," + "}";
            Script script = new Script() {
                Id = "1",
                Name = "Python1",
                ScriptVersion = "1.11",
                Language = "python",
                Code = code1,
                Author = "test1",
                LastModified = DateTime.Now,
                DateCreated = DateTime.Now
            };
            Mock<IFormCollection> forms = new Mock<IFormCollection>();
            forms.Setup(x => x["id"]).Returns("1");
            forms.Setup(x => x["scriptName"]).Returns("Python1");
            forms.Setup(x => x["language"]).Returns("Python");
            forms.Setup(x => x["version"]).Returns("1.11");
            forms.Setup(x => x["dateCreated"]).Returns(DateTime.Now.ToString());
            forms.Setup(x => x["editorContent"]).Returns(code1);
            forms.Setup(x => x["author"]).Returns("test1");
            forms.Setup(x => x["lastModified"]).Returns(DateTime.Now.ToString());

            //Act
            var result1 = iDEController.SaveScript(forms.Object) as ViewResult;
            var script2 = (Script)result1.ViewData.Model;

            //Assert
            Assert.Equal("SaveScript", result1.ViewName);
            Assert.Null(script2);
        }

        [Fact]
        public void DeleteScriptTest() {
            //Arrange
            string scriptId1 = "2";
            string scriptId2 = "cc";

            aPIAccessMock.Setup(x => x.DeleteScript(scriptId1)).Returns(true);
            aPIAccessMock.Setup(x => x.DeleteScript(scriptId2)).Returns(false);

            //Act
            var result1 = iDEController.DeleteScript(scriptId1) as ViewResult;
            var result2 = iDEController.DeleteScript(scriptId2) as ViewResult;

            //Assert
            Assert.Equal("DeleteScript", result1.ViewName);
            Assert.Equal("DeleteScript", result2.ViewName);
        }

        [Fact]
        public void EditScriptTest() {
            //Arrange 
            string scriptId1 = "1";
            string scriptId2 = "cc";
            Script script1 = new Script() {
                Id = "1",
                Name = "Python1",
                Customer = "testCustomer",
                ScriptVersion = "1.11",
                Language = "python",
                LanguageVersion = "1.0.1",
                Code = "#This is the datamodel for Location, Source, and State." + "\n" +
                       "location = { 'Id':" + '\u0022' + "" + '\u0022' + "," + "'ParentId':" + '\u0022' + "" + '\u0022' + "," + "'ExternalId':" + '\u0022' + "" + '\u0022' + "," + "'ConsumerId': 2, 'Sources': [] }" + "\n" +
                        "source = { 'Type':" + '\u0022' + "" + '\u0022' + "," + "'TimeStamp':" + '\u0022' + "" + '\u0022' + "," + "'State': [] }" + "\n" +
                        "state = { 'Property':" + '\u0022' + "" + '\u0022' + "," + "'Value':" + '\u0022' + "" + '\u0022' + "," + "}",
                DateCreated = DateTime.Now,
                Author = "test1",
                LastModified = DateTime.Now
            };
            Script script2 = null;

            aPIAccessMock.Setup(x => x.GetScriptById(scriptId1)).Returns(script1);
            aPIAccessMock.Setup(x => x.GetScriptById(scriptId2)).Returns(script2);

            //Act
            var result1 = iDEController.EditScript(scriptId1) as ViewResult;
            var script = (Script)result1.ViewData.Model;
            var result2 = iDEController.EditScript(scriptId2) as ViewResult;

            //Assert
            Assert.Equal("EditScript", result1.ViewName);
            Assert.Equal("Python1", script.Name);
            Assert.Equal("EditScript", result2.ViewName);
        }
    }
}
