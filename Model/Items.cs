using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedCrossBackend.Model
{
    public class Assistance
    {
        public int id { get; set; }
        public string name { get; set; }
        public Assistance(){}
    }
    public class Injury
    {
        public int id { get; set; }
        public string name { get; set; }
        public Injury(){}
    }
    public class PhType
    {
        public int id { get; set; }
        public string name { get; set; }
        public PhType(){}
    }
}
