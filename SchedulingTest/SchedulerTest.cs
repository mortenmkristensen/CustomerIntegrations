using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Database;
using MessageBroker;
using Models;
using Moq;
using Scheduling;
using Xunit;

namespace SchedulingTest {
    public class SchedulerTest {
        private readonly Scheduler scheduler;
        private readonly  Mock<IDBAccess> dBAccessMock = new Mock<IDBAccess>();
        private readonly Mock<IMessageBroker> messageBrokerMock = new Mock<IMessageBroker>();
        public SchedulerTest() {
            scheduler = new Scheduler(dBAccessMock.Object, messageBrokerMock.Object);
        }
        //This test tests the private method SeperateByLanguage.
        [Fact]
        public void SeperateByLanguageTest() {
            //Arrange
            //python
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
            //ruby
            Script script2 = new Script() {
                Id = "2",
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
            //javascript
            Script script3 = new Script() {
                Id = "3",
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
            List<Script> scripts = new List<Script>();
            scripts.Add(script1);
            scripts.Add(script2);
            scripts.Add(script3);

            //Act
            MethodInfo methodInfo = typeof(Scheduler).GetMethod("SeparateByLanguage", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = {scripts};
            var scriptsSeparetedByLangugage = (Dictionary<string, List<Script>>) methodInfo.Invoke(scheduler, parameters);
            var scriptLists = scriptsSeparetedByLangugage.Values.ToList();
            var languages = scriptsSeparetedByLangugage.Keys.ToList();
           
            //Assert
            Assert.True(languages[0].Equals("python") && scriptLists[0][0].Language.Equals(languages[0]));
            Assert.True(languages[1].Equals("ruby") && scriptLists[1][0].Language.Equals(languages[1]));
            Assert.True(languages[2].Equals("javascript") && scriptLists[2][0].Language.Equals(languages[2]));
        }

        //This test tests the private method GetNewScripts.
        [Fact]
        public void GetNewScriptsTest() {
            //Arrange
            //python
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
       
            List<Script> scripts1 = new List<Script>();
            scripts1.Add(script1);
            dBAccessMock.Setup(x => x.GetAll()).Returns(scripts1);

            //Act
            var scripts2 = scheduler.GetNewScripts();

            //Assert
            Assert.Equal(scripts1, scripts2);
        }
    }
}
