using System;
using System.Collections.Generic;

namespace RedCrossBackend.Model
{
    public class FaphType
    {
        public int id { get; set; }
        public int FAId { get; set; }
        public int PTId { get; set; }
        public FaphType() { }
        public FaphType(int FAId, int PTId)
        {
            this.FAId = FAId;
            this.PTId = PTId;
        }
    }
}
