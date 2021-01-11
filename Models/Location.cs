using System;
using System.Collections.Generic;
using System.Text;

namespace Models {
    public class Location {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string ExternalId { get; set; }
        public int ConsumerId { get; set; }
        public List<Source> Sources { get; set; }
    }
}
