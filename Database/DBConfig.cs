using System;

namespace Database {
    public class DBConfig : IDBConfig {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }

        public DBConfig() {
            ConnectionString = Environment.GetEnvironmentVariable("MP_CONNECTIONSTRING");
            Database = Environment.GetEnvironmentVariable("MP_DATABASE");
            Collection = Environment.GetEnvironmentVariable("MP_COLLECTION");
        }
    }
}
