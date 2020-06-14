using System;
using System.Collections.Generic;

namespace RedCrossBackend.Model
{
    public class FirstAidDTO : FirstAid
    {
        public string[] injury { get; set; }
        public string[] assistance { get; set; }
        public string[] phType { get; set; }
    }
}
