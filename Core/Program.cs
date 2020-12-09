using System;
using Core.Database;
using Core.Models;
using System.IO;
using MongoDB.Bson;

namespace Core {
    class Program {
        static void Main(string[] args) {
            DBAccess db = new DBAccess();
            string code = File.ReadAllText(@"C:\Mapspeople\DataSources\Python\pythonDatasource.py");
            Script script = new Script() {
                Id = "5fd0ad42dd2f1986596ec9af", Customer = "Hans", Language = "python", LanguageVersion = "3.8", Name = "Test2", ScriptVersion = 1.0,
                Code = code, LastResult ="", HasErrors = false, DateCreated = DateTime.Now, Author = "Emil", LastModified = DateTime.Now
            };
            db.Upsert(script);

            var script1 = db.GetScriptById("5fd0ad42dd2f1986596ec9af");
            Console.WriteLine(script1.Id);
            
        }
    }
}
