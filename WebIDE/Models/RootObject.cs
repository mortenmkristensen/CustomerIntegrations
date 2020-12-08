using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebIDE.Models {
    public class RootObject {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Customer { get; set; }
        public double ScriptVersion { get; set; }
        public string Language { get; set; }
        public string LanguageVersion { get; set; }
        public string Code { get; set; }
        public string LastResult { get; set; }
        public bool HasErrors { get; set; }
    }
}
