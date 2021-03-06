using System;
using System.Collections.Generic;
using Core;
using Core.Exceptions;
using Database;
using MessageBroker;
using Microsoft.Extensions.Logging;
using Models;
using Moq;
using Xunit;

namespace CoreTest {
    public class AppTest {
        private readonly App app;
        private readonly Mock<IMessageBroker> messageBrokerMock = new Mock<IMessageBroker>();
        private readonly Mock<IStager> stagerMock = new Mock<IStager>();
        private readonly Mock<IScriptRunner> scriptRunnerMock = new Mock<IScriptRunner>();
        private readonly Mock<IDataValidation> dataValidationMock = new Mock<IDataValidation>();
        private readonly Mock<IDBAccess> dbAccessMock = new Mock<IDBAccess>();
        private readonly Mock<ILogger<App>>logMock = new Mock<ILogger<App>>();
        public AppTest() {
            app = new App(dbAccessMock.Object, stagerMock.Object, scriptRunnerMock.Object, messageBrokerMock.Object, dataValidationMock.Object, logMock.Object);
        }

        //This test tests a list with a correct script.
        [Fact]
        public void RunTest() {
            //Arrange
            var interpreterPath = "python";
            int count = 15;
            string queueName = "MP_QUEUENAME";
            string consumerQueueName = "MP_CONSUMERQUEUE";

            List<Script> scripts1 = new List<Script>();
            //correct Script
            Script script1 = new Script() {
                Id = "1",
                Name = "Python1",
                Customer = "testCustomer",
                ScriptVersion = "1.11",
                Language = "python",
                LanguageVersion = "1.0.1",
                Code = "#This is the datamodel for Location, Source, and State." + "\n" +
                       "location = { 'Id':" + '\u0022' + "" + '\u0022' + "," + "'ParentId':" + '\u0022' + "" + '\u0022' + "," + "'ExternalId':" + 
                       '\u0022' + "" + '\u0022' + "," + "'ConsumerId': 2, 'Sources': [] }" + "\n" +
                        "source = { 'Type':" + '\u0022' + "" + '\u0022' + "," + "'TimeStamp':" + '\u0022' + "" + '\u0022' + "," + "'State': [] }" + "\n" +
                        "state = { 'Property':" + '\u0022' + "" + '\u0022' + "," + "'Value':" + '\u0022' + "" + '\u0022' + "," + "}",
                DateCreated = DateTime.Now,
                Author = "test1",
                LastModified = DateTime.Now
            };
            scripts1.Add(script1);          
            string path1 = $@"c:\scripts\python\{script1.Id}.py";
            Dictionary<string, string> paths = new Dictionary<string, string>();
            paths.Add(script1.Id, path1);
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
            List<string> messages = new List<string>();
            messages.Add(result);

            messageBrokerMock.Setup(x => x.Receive(queueName)).Returns(scripts1);        
            stagerMock.Setup(x => x.GetPaths(scripts1)).Returns(paths);         
            scriptRunnerMock.Setup(x => x.RunScript(script1.Id, path1, interpreterPath)).Returns(result);          
            dataValidationMock.Setup(x => x.ValidateScriptOutput(result)).Returns(true);
            dbAccessMock.Setup(x => x.Upsert(script1)).Returns(script1);          
            messageBrokerMock.Setup(x => x.Send<string>(consumerQueueName, messages));
                           
            //Act
            int i = app.Run(interpreterPath, count,queueName,consumerQueueName);
           
            //Assert
            Assert.Equal(0, i);
            Assert.Equal(result, script1.LastResult);
        }

        //This test tests a list with a error script.
        [Fact]
        public void Runtest2() {
            //Arrange
            var interpreterPath2 = "node";
            int count = 15;
            string queueName = "Test_MP_QUEUENAME";
            string consumerQueueName = "Test_MP_CONSUMERQUEUE";

            List<Script> scripts2 = new List<Script>();
            //wrong script
            Script script2 = new Script() {
                Id = "2",
                Name = "javascript1",
                Customer = "testCustomer",
                ScriptVersion = "3.11",
                Language = "javascript",
                LanguageVersion = "1.0.1",
                Code = "//This is the datamodel for Location, Source, and State." + "\n" +
                     "let location = {" + "\n" +
                     "   Id:" + '\u0022' + "" + '\u0022' + "," + "\n" +
                     "   ParentId:" + '\u0022' + "" + '\u0022' + "," + "\n" +
                     "wrong" +
                     "};",
                DateCreated = DateTime.Now,
                Author = "test1",
                LastModified = DateTime.Now
            };
            scripts2.Add(script2);
            string path2 = $@"c:\scripts\javascript\{script2.Id}.js";
            Dictionary<string, string> paths2 = new Dictionary<string, string>();
            paths2.Add(script2.Id, path2);
            var result2 = "wrong";
            List<string> messages = new List<string>();

            messageBrokerMock.Setup(x => x.Receive(queueName)).Returns(scripts2);       
            stagerMock.Setup(x => x.GetPaths(scripts2)).Returns(paths2);
            scriptRunnerMock.Setup(x => x.RunScript(script2.Id, path2, interpreterPath2)).Returns(result2);
            dataValidationMock.Setup(x => x.ValidateScriptOutput(result2)).Throws(new ScriptFailedException());
            dbAccessMock.Setup(x => x.Upsert(script2)).Returns(script2);       
            messageBrokerMock.Setup(x => x.Send<string>(consumerQueueName, messages));
           
            //Act
            int i2 = app.Run(interpreterPath2, count,queueName,consumerQueueName);
            
            //Assert        
            Assert.True(script2.HasErrors);
            Assert.Equal(0,i2);
        }

        //This test tests a list which is null.
        [Fact]
        public void RunTest3() {
            //Arrange
            var interpreterPath = "python";
            int count = 15;
            string queueName = "MP_QUEUENAME";
            string consumerQueueName = "MP_CONSUMERQUEUE";

            List<Script> scripts3 = null;
            messageBrokerMock.Setup(x => x.Receive(queueName)).Returns(scripts3);

            //Act
            int i3 = app.Run(interpreterPath, count,queueName,consumerQueueName);

            //Assert
            Assert.Equal(16, i3);
        }
    }

 }