using Newtonsoft.Json;
using System;

namespace Models {
    public class Script {
        public string _id { get; set; }
        public string Name { get; set; }
        public string Customer { get; set; }
        public double ScriptVersion { get; set; }
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
                   _id == script._id;
        }

        public override int GetHashCode() {
            return HashCode.Combine(_id);
        }
    }
}
