using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models {
    class Domain {
        public string Id { get; set; }
        public Dictionary<string, string> CustomProperties { get; set; }
    }
}
