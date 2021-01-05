using System;
using System.Collections.Generic;
using Core;
using Core.Exceptions;
using Database;
using MessageBroker;
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
        
        public AppTest() {
            app = new App(dbAccessMock.Object, stagerMock.Object, scriptRunnerMock.Object, messageBrokerMock.Object, dataValidationMock.Object);
        }

        [Fact]
        public void RunTest() {
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
            Script script2 = new Script() {
                Id = "2",
                Name = "Python2",
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
            Script script3 = new Script() {
                Id = "3",
                Name = "ruby1",
                Customer = "testCustomer",
                ScriptVersion = "2.11",
                Language = "ruby",
                LanguageVersion = "2.0.1",
                Code = "#This is the datamodel for Location, Source, and State." + "\n" + "class Location" + "\n" +
                        " attr_writer :Id,:ParentId,:ExternalId,:ConsumerId,:Sources" + "\n" +
                        " attr_reader :Id,:ParentId,:ExternalId,:ConsumerId,:Sources" + "\n" +
                        " def to_json(*a)" + "\n" +
                        "  { 'Id' => @Id, 'ParentId' => @ParentId, 'ExternalId' => @ExternalId, 'ConsumerId'=> @ConsumerId, 'Sources' => @Sources}.to_json" + "\n" +
                         " end" + "\n" +
                        " def self.from_json string" + "\n" +
                        "  data = JSON.load string" + "\n" +
                        "  self.new data['Id'], data['ParentId'], data['ExternalId'], data['ConsumerId'], data['Sources']" + "\n" +
                        " end",
                DateCreated = DateTime.Now,
                Author = "test1",
                LastModified = DateTime.Now
            };
            Script script4 = new Script() {
                Id = "4",
                Name = "ruby1",
                Customer = "testCustomer",
                ScriptVersion = "2.11",
                Language = "ruby",
                LanguageVersion = "2.0.1",
                Code = "#This is the datamodel for Location, Source, and State." + "\n" + "class Location" + "\n" +
                       " attr_writer :Id,:ParentId,:ExternalId,:ConsumerId,:Sources" + "\n" +
                       " attr_reader :Id,:ParentId,:ExternalId,:ConsumerId,:Sources" + "\n" +
                       " def to_json(*a)" + "\n" +
                       "  { 'Id' => @Id, 'ParentId' => @ParentId, 'ExternalId' => @ExternalId, 'ConsumerId'=> @ConsumerId, 'Sources' => @Sources}.to_json" + "\n" +
                        " end",
                DateCreated = DateTime.Now,
                Author = "test1",
                LastModified = DateTime.Now
            };
            Script script5 = new Script() {
                Id = "5",
                Name = "javascript1",
                Customer = "testCustomer",
                ScriptVersion = "3.11",
                Language = "javascript",
                LanguageVersion = "1.0.1",
                Code = "//This is the datamodel for Location, Source, and State." + "\n" +
                      "let location = {" + "\n" +
                      "   Id:" + '\u0022' + "" + '\u0022' + "," + "\n" +
                      "   ParentId:" + '\u0022' + "" + '\u0022' + "," + "\n" +
                      "   ExternalId:" + '\u0022' + "" + '\u0022' + "," + "\n" +
                      "   customerId: 1," + "\n" +
                      "   Sources: []" + "\n" +
                      "};",
                DateCreated = DateTime.Now,
                Author = "test1",
                LastModified = DateTime.Now
            };
            Script script6 = new Script() {
                Id = "6",
                Name = "javascript1",
                Customer = "testCustomer",
                ScriptVersion = "3.11",
                Language = "javascript",
                LanguageVersion = "1.0.1",
                Code = "//This is the datamodel for Location, Source, and State." + "\n" +
                      "let location = {" + "\n" +
                      "   Id:" + '\u0022' + "" + '\u0022' + "," + "\n" +
                      "   ParentId:" + '\u0022' + "" + '\u0022' + "," + "\n" +
                      "   ExternalId:" + '\u0022' + "" + '\u0022' + "," + "\n" +
                      "   customerId: 1," + "\n" +
                      "   Sources: []" + "\n" +
                      "};",
                DateCreated = DateTime.Now,
                Author = "test1",
                LastModified = DateTime.Now
            };
            //wrong script
            Script script7 = new Script() {
                Id = "7",
                Name = "javascript1",
                Customer = "testCustomer",
                ScriptVersion = "3.11",
                Language = "javascript",
                LanguageVersion = "1.0.1",
                Code = "//This is the datamodel for Location, Source, and State." + "\n" +
                     "let location = {" + "\n" +
                     "   Id:" + '\u0022' + "" + '\u0022' + "," + "\n" +
                     "   ParentId:" + '\u0022' + "" + '\u0022' + "," + "\n" +
                     "wrong"+
                     "};",
                DateCreated = DateTime.Now,
                Author = "test1",
                LastModified = DateTime.Now
            };

            scripts1.Add(script1);
            scripts1.Add(script2);
            scripts1.Add(script3);
            scripts1.Add(script4);
            scripts1.Add(script5);
            scripts1.Add(script6);
            scripts1.Add(script7);
            string queueName = "MP_QUEUENAME";
            messageBrokerMock.Setup(x => x.Receive(queueName)).Returns(scripts1);
            string path1 = $@"c:\scripts\python\{script1.Id}.py";
            string path2 = $@"c:\scripts\python\{script2.Id}.py";
            string path3 = $@"c:\scripts\ruby\{script3.Id}.rb";
            string path4 = $@"c:\scripts\ruby\{script4.Id}.rb";
            string path5 = $@"c:\scripts\javascript\{script5.Id}.js";
            string path6 = $@"c:\scripts\javascript\{script6.Id}.js";
            string path7 = $@"c:\scripts\javascript\{script7.Id}.js";

            Dictionary<string, string> paths = new Dictionary<string, string>();
            paths.Add(script1.Id, path1);
            paths.Add(script2.Id, path2);
            paths.Add(script3.Id, path3);
            paths.Add(script4.Id, path4);
            paths.Add(script5.Id, path5);
            paths.Add(script6.Id, path6);
            paths.Add(script7.Id, path7);
            stagerMock.Setup(x => x.GetPaths(scripts1)).Returns(paths);
            var result2 = "wrong";
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
            var interpreterPath = "python";
            var interpreterPath2 = "ruby";
            var interpreterPath3 = "node";
            scriptRunnerMock.Setup(x => x.RunScript(script1.Id, path1, interpreterPath)).Returns(result);
            scriptRunnerMock.Setup(x => x.RunScript(script2.Id, path2, interpreterPath)).Returns(result);
            scriptRunnerMock.Setup(x => x.RunScript(script3.Id, path3, interpreterPath2)).Returns(result);
            scriptRunnerMock.Setup(x => x.RunScript(script4.Id, path4, interpreterPath2)).Returns(result);
            scriptRunnerMock.Setup(x => x.RunScript(script5.Id, path5, interpreterPath3)).Returns(result);
            scriptRunnerMock.Setup(x => x.RunScript(script6.Id, path6, interpreterPath3)).Returns(result);
            scriptRunnerMock.Setup(x => x.RunScript(script7.Id, path7, interpreterPath3)).Returns(result2);
            dataValidationMock.Setup(x => x.ValidateScriptOutput(result)).Returns(true);
            dataValidationMock.Setup(x => x.ValidateScriptOutput(result2)).Throws(new ScriptFailedException());


            dbAccessMock.Setup(x => x.Upsert(script1)).Returns(script1);
            dbAccessMock.Setup(x => x.Upsert(script2)).Returns(script2);
            dbAccessMock.Setup(x => x.Upsert(script3)).Returns(script3);
            dbAccessMock.Setup(x => x.Upsert(script4)).Returns(script4);
            dbAccessMock.Setup(x => x.Upsert(script5)).Returns(script5);
            dbAccessMock.Setup(x => x.Upsert(script6)).Returns(script6);
            dbAccessMock.Setup(x => x.Upsert(script7)).Returns(script7);
            List<string> messages = new List<string>();
            messages.Add(result);
            messages.Add(result);
            messageBrokerMock.Setup(x => x.Send<string>(Environment.GetEnvironmentVariable("MP_CONSUMERQUEUE"), messages));

            int count = 15;
            //Act
            int i = app.Run(interpreterPath, count);
            int i2 = app.Run(interpreterPath2, count);
            int i3 = app.Run(interpreterPath3, count);
            //Assert
            Assert.True(i>=0);
            Assert.True(i2>= 0);
            Assert.True(i3>= 0);
            //Assert.True(script7.HasErrors);
        }
    }

 }