﻿using System;
using Core.Database;
using Core.Models;
using System.IO;

namespace Core {
    class Program {
        static void Main(string[] args) {
            DBAccess db = new DBAccess();
            string code = File.ReadAllText(@"C:\Mapspeople\DataSources\Python\pythonDatasource.py");
            Script script = new Script() {
                Id = "7", Customer = "Hans", Language = "python", LanguageVersion = "3.8", Name = "Test", ScriptVersion = 1.0,
                Code = code, LastResult ="", HasErrors = false, DateCreated = DateTime.Now, Author = "Emil", LastModified = DateTime.Now
            };
            db.Upsert(script);
            
        }
    }
}
