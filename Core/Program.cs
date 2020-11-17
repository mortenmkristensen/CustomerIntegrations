using System;
using Core.Database;
using Core.Models;
using System.IO;

namespace Core {
    class Program {
        static void Main(string[] args) {
            DBAccess db = new DBAccess();
            string code = File.ReadAllText(@"C:\Mapspeople\HelloWorld\main.py");
            Script script = new Script() {
                Id = "2", Customer = "Hans", Language = "python", LanguageVersion = "3.8", Name = "Hello", ScriptVersion = 1.0,
                Code = code
            };
            db.Upsert(script);
            
        }
    }
}
