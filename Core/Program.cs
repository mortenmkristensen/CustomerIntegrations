using System;
using Core.Database;
using Core.Models;
using System.IO;
using MongoDB.Bson;
using System.Threading;

namespace Core {
    class Program {
        static void Main(string[] args) {
            DBAccess db = new DBAccess();
            string code = File.ReadAllText(@"C:\Mapspeople\DataSources\RubySCript.rb");
            for (int i = 43; i < 55; i++) {
                Script script = new Script() {
                    Id = "" + i, Customer = "bent", Language = "ruby", LanguageVersion = "2.7.2", Name = "Test" +1, ScriptVersion = 1.0,
                    Code = code, LastResult = "", HasErrors = false, DateCreated = DateTime.Now, Author = "Emil", LastModified = DateTime.Now
                };
                db.Upsert(script);
                Thread.Sleep(500);
            }
            
        }
    }
}
