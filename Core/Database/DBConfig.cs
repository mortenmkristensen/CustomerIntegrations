using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Database {
    class DBConfig {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }

        public DBConfig(string connectionString, string database, string collection) {
            ConnectionString = connectionString;
            Database = database;
            Collection = collection;
        }
    }
}
