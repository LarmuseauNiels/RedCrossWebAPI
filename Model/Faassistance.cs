using System;
using System.Collections.Generic;

namespace RedCrossBackend.Model
{
    public class Faassistance
    {
        public int id { get; set; }
        public int FAId { get; set; }
        public int AId { get; set; }
        public Faassistance() { }
        public Faassistance(int FAId, int AId)
        {
            this.FAId = FAId;
            this.AId = AId;
        }
    }
}
