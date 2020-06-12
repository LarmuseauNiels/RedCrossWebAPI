using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedCrossBackend.Model
{
    public class FAAssitance
    {
        public int id { get; set; }
        public int fAId { get; set; }
        public int aId { get; set; }
        public string other { get; set; }
    }
    public class FAInjury
    {
        public int id { get; set; }
        public int fAId { get; set; }
        public int iId { get; set; }
        public string other { get; set; }

    }
    public class FAPhType
    {
        public int id { get; set; }
        public int fAId { get; set; }
        public int pTId { get; set; }
        public string other { get; set; }

    }
    public class FASetting
    {
        public int id { get; set; }
        public int fAId { get; set; }
        public int sId { get; set; }
        public string other { get; set; }

    }
}
