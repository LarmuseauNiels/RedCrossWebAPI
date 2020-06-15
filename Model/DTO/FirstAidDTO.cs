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
    public class FirstAidRaw : FirstAid
    {
        public string injury { get; set; }
        public string assistance { get; set; }
        public string phType { get; set; }
        public FirstAidRaw(){}
        public FirstAidRaw(FirstAid fa, string inj, string ass, string pht)
        {
            
        }
    }
}
