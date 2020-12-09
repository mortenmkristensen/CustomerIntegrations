using Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database {
    public class DBAccess : IDBAccess {
            MongoClient Client { get; set; }
            IMongoDatabase Database { get; set; }
            IMongoCollection<Script> Collection { get; set; }
            public IDBConfig Config { get; set; }
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
            public void Delete(string id) {
                try {
                    var filter = Builders<Script>.Filter.Eq("_id", id);
                    Collection.DeleteOne(filter);
                } catch (MongoException me) {
                    throw new Exception("Something went wrong when trying to delte a script", me);
                }
            }

            public IEnumerable<Script> GetAll() {
                Task<List<Script>> scripts = null;
                try {
                    scripts = Collection.Find<Script>(f => true).ToListAsync();
                } catch (MongoException me) {
                    throw new Exception("Something went wrong when trying to get the scripts", me);
                }
                return scripts.Result;
            }

            public Script GetScriptById(string id) {
                Task<Script> script = null;
                try {
                    if (id != null) {
                        var filter = Builders<Script>.Filter.Eq("_id", id);
                        script = Collection.Find(filter).FirstOrDefaultAsync();
                    }
                } catch (MongoException me) {
                    throw new Exception("Something went wrong when trying to get a script", me);
                }
                return script.Result;
            }

            public void Upsert(Script script) {
                try {
                    var filter = Builders<Script>.Filter.Eq("_id", script._id);
                    Collection.ReplaceOne(filter, script, new ReplaceOptions { IsUpsert = true });
                } catch (MongoException me) {
                    throw new Exception("Something went wrong when trying to insert a script", me);
                }
            }

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
