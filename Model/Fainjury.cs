using System;
using System.Collections.Generic;

namespace RedCrossBackend.Model
{
    public class Fainjury
    {
        public int id { get; set; }
        public int FAId { get; set; }
        public int IId { get; set; }
        public Fainjury(){}
        public Fainjury(int FAId, int IId)
        {
            this.FAId = FAId;
            this.IId = IId;
        }
    }
}
