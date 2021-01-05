using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Models;
using Xunit;

namespace CoreTest {
    public class StagerTest {
        private readonly Stager stager;
        public StagerTest() {
            stager = new Stager();
        }

        [Fact]
        public void GetPathsTest() {
            // Arrange
            //python
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
            scripts1.Add(script1);
            scripts1.Add(script2);

            //ruby
            List<Script> scripts2 = new List<Script>();
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
            scripts2.Add(script3);
            scripts2.Add(script4);

            //javascript
            List<Script> scripts3 = new List<Script>();
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
            scripts3.Add(script5);
            scripts3.Add(script6);

            //Act
            var paths1 = stager.GetPaths(scripts1);
            var paths2 = stager.GetPaths(scripts2);
            var paths3 = stager.GetPaths(scripts3);

            //Assert
            Assert.True(paths1.Count > 0);
            Assert.True(paths2.Count > 0);
            Assert.True(paths3.Count > 0);
        }
    }
}
