using System;
using System.Collections.Generic;

namespace Models {
    public class Source {
        public string Type { get; set; }
        public List<State> State { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}