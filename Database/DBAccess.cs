using Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database {

    //This class uses MongoDB
    public class DBAccess : IDBAccess {
        MongoClient Client { get; set; }
        IMongoDatabase Database { get; set; }
        IMongoCollection<Script> Collection { get; set; }
        public IDBConfig Config { get; set; }

        //The constructor uses the IDBConfig to connect to the database. 
        public DBAccess(IDBConfig dBConfig) {
            try {
                Config = dBConfig;
                Client = new MongoClient(Config.ConnectionString);
                Database = Client.GetDatabase(Config.Database);
                Collection = Database.GetCollection<Script>(Config.Collection);
            } catch (MongoException me) {
                throw new Exception("Something went wrong when trying to connect to the database", me);
            }
        }

        //This method deletes a script. 
        public void Delete(string id) {
            try {
                if (id != null) {
                    var objectId = new ObjectId(id);
                    var filter = Builders<Script>.Filter.Eq("_id", objectId);
                    Collection.DeleteOne(filter);
                }
            } catch (MongoException me) {
                throw new Exception("Something went wrong when trying to delte a script", me);
            }
        }

        //This method gets all scripts from the database, and returns it in a IEnumerable.
        public IEnumerable<Script> GetAll() {
            Task<List<Script>> scripts = null;
            try {
                scripts = Collection.Find<Script>(f => true).ToListAsync();
            } catch (MongoException me) {
                throw new Exception("Something went wrong when trying to get the scripts", me);
            }
            return scripts.Result;
        }

        //This method gets a specifik script out of the database (based on script Id).
        public Script GetScriptById(string id) {
            Task<Script> script = null;
            try {
                if (id != null) {
                    var objectId = new ObjectId(id);
                    var filter = Builders<Script>.Filter.Eq("_id", objectId);
                    script = Collection.Find(filter).FirstOrDefaultAsync();
                }
            } catch (MongoException me) {
                throw new Exception("Something went wrong when trying to get a script", me);
            }
            return script.Result;
        }

        //This method inserts a script into the database. 
        public Script Upsert(Script script) {
            try {
                //If a new script is created that has no Id, it is inserted (and a new Id i given). 
                if (script.Id == null || script.Id == "") {
                    Collection.InsertOne(script);
                    //If it's a script that already has an Id, the script is replaced. 
                } else {
                    var objectId = new ObjectId(script.Id);
                    var filter = Builders<Script>.Filter.Eq("_id", objectId);
                    Collection.ReplaceOne(filter, script, new ReplaceOptions { IsUpsert = true });
                }
            } catch (MongoException me) {
                throw new Exception("Something went wrong when trying to insert a script", me);
            }
            return script;
        }

        //This method gets scripts from the database by language, and returns it in a list.  
        public IEnumerable<Script> GetByLanguage(string language) {
            Task<List<Script>> scripts = null;
            try {
                var filter = Builders<Script>.Filter.Eq("Language", language);
                scripts = Collection.Find<Script>(filter).ToListAsync();
            } catch (MongoException me) {
                throw new Exception("Something went wrong when trying to get the scripts", me);
            }
            return scripts.Result;
        }

        //This method gets a list of scripts from the database by a customer, and returns it in a list. 
        public IEnumerable<Script> GetByCustomer(string customer) {
            Task<List<Script>> scripts = null;
            try {
                if (customer != null) {
                    var filter = Builders<Script>.Filter.Eq("Customer", customer);
                    scripts = Collection.Find<Script>(filter).ToListAsync();
                }
            } catch (MongoException me) {
                throw new Exception("Something went wrong when trying to get the scripts", me);
            }
            return scripts.Result;
        }
    }
}
