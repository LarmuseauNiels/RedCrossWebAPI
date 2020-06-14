using System;
using System.Collections.Generic;

namespace RedCrossBackend.Model
{
    public partial class FirstAid
    {
        public int id { get; set; }
        public DateTime? assignDate { get; set; }
        public string macAddress { get; set; }
        public string gender { get; set; }
        public string age { get; set; }
        public string education { get; set; }
        public bool? hadFATraining { get; set; }
        public int? numberOffATtraining { get; set; }
        public bool? trainingByRC { get; set; }
        public bool? blendedTraining { get; set; }
        public string otherTrainingProvider { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string setting { get; set; }
        public int? confidentApplyingFA { get; set; }
        public bool? phNeeded { get; set; }
        public int? phTimeToArriveMs { get; set; }
        public bool? hospitalisationRequired { get; set; }

    }
}
