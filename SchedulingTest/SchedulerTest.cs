using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Database;
using MessageBroker;
using Microsoft.Extensions.Logging;
using Models;
using Moq;
using Scheduling;
using Xunit;

namespace SchedulingTest {
    public class SchedulerTest {
        private readonly Scheduler scheduler;
        private readonly  Mock<IDBAccess> dBAccessMock = new Mock<IDBAccess>();
        private readonly Mock<IMessageBroker> messageBrokerMock = new Mock<IMessageBroker>();
        private readonly Mock<ILogger<Scheduler>> _log = new Mock<ILogger<Scheduler>>();
        public SchedulerTest() {
            scheduler = new Scheduler(dBAccessMock.Object, messageBrokerMock.Object,_log.Object);
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
            List<Script> listWith3Scripts = new List<Script>();
            listWith3Scripts.Add(script1);
            listWith3Scripts.Add(script2);
            listWith3Scripts.Add(script3);

            List<Script> listWith2Scripts = new List<Script>();
            listWith2Scripts.Add(script1);
            listWith2Scripts.Add(script2);

            List<Script> listWith1Scripts = new List<Script>();
            listWith1Scripts.Add(script1);

            List<Script> listWith0Scripts = new List<Script>();

            List<Script> nullList = null;

            //Act
            var scriptsSeparetedByLangugage1 = scheduler.SeparateByLanguage(listWith3Scripts);
            var scriptLists1 = scriptsSeparetedByLangugage1.Values.ToList();
            var languages1 = scriptsSeparetedByLangugage1.Keys.ToList();

            var scriptsSeparetedByLangugage2 = scheduler.SeparateByLanguage(listWith2Scripts);
            var scriptLists2 = scriptsSeparetedByLangugage2.Values.ToList();
            var languages2 = scriptsSeparetedByLangugage2.Keys.ToList();

            var scriptsSeparetedByLangugage3 = scheduler.SeparateByLanguage(listWith1Scripts);
            var scriptLists3 = scriptsSeparetedByLangugage3.Values.ToList();
            var languages3 = scriptsSeparetedByLangugage3.Keys.ToList();

            var scriptsSeparetedByLangugage4 = scheduler.SeparateByLanguage(listWith0Scripts);

            var scriptsSeparetedByLangugage5 = scheduler.SeparateByLanguage(nullList);

            //Assert
            Assert.True(languages1[0].Equals("python") && scriptLists1[0][0].Language.Equals(languages1[0]));
            Assert.True(languages1[1].Equals("ruby") && scriptLists1[1][0].Language.Equals(languages1[1]));
            Assert.True(languages1[2].Equals("javascript") && scriptLists1[2][0].Language.Equals(languages1[2]));

            Assert.True(languages2[0].Equals("python") && scriptLists2[0][0].Language.Equals(languages2[0]));
            Assert.True(languages2[1].Equals("ruby") && scriptLists2[1][0].Language.Equals(languages2[1]));

            Assert.True(languages3[0].Equals("python") && scriptLists3[0][0].Language.Equals(languages3[0]));

            Assert.True(scriptsSeparetedByLangugage4.Count == 0);

            Assert.True(scriptsSeparetedByLangugage5.Count == 0);
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
        [Fact]
        public void SplitListTest() {
            //Arrange
            List<string> list = new List<string>();
            for (int i = 0; i < 25; i++) {
                list.Add(@"Entry: {i}");
            }
            //Act
            var result1 = scheduler.SplitList<string>(list, 5).ToList();
            var result2 = scheduler.SplitList<string>(list.GetRange(0,10), 7).ToList();
            var result3 = scheduler.SplitList<string>(list, 7).ToList();
            list.Clear();
            var result4 = scheduler.SplitList<string>(list, 10).ToList();
            list = null;

            //Assert
            Assert.Equal(5, result1.Count);
            Assert.Equal(2, result2.Count);
            Assert.Equal(4, result3.Count);
            Assert.Empty(result4);
            Assert.Throws<NullReferenceException>(() => scheduler.SplitList<string>(list, 5).ToList());
        }
    }
}
