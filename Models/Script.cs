using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Models {
    public class Script {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Customer { get; set; }
        public string ScriptVersion { get; set; }
        public string Language { get; set; }
        public string LanguageVersion { get; set; }
        public string Code { get; set; }
        public string LastResult { get; set; }
        public bool HasErrors { get; set; }
        public DateTime DateCreated { get; set; }
        public string Author { get; set; }
        public DateTime LastModified { get; set; }

        public override bool Equals(object obj) {
            return obj is Script script &&
                   Id == script.Id;
        }

        public override int GetHashCode() {
            return HashCode.Combine(Id);
        }
    }
}
