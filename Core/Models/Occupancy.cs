using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models {
    class Occupancy : Domain {
        public int PersonCount { get; set; }
        public bool Occupied { get; set; }
        public string Timestamp { get; set; }
        public int Capacity { get; set; }
    }
}
