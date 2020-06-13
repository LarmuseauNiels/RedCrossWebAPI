using System;
using System.Collections.Generic;

namespace RedCrossBackend.DBModel
{
    public partial class FirstAid
    {
        public int Id { get; set; }
        public DateTime? AssignDate { get; set; }
        public string MacAddress { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string Education { get; set; }
        public bool? HadFatraining { get; set; }
        public int? NumberOffAttraining { get; set; }
        public bool? TrainingByRc { get; set; }
        public bool? BlendedTraining { get; set; }
        public string OtjerTrainingProvider { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Setting { get; set; }
        public int? ConfidentApplyingFa { get; set; }
        public bool? PhNeeded { get; set; }
        public int? PhTimeToArriveMs { get; set; }
        public bool? HospitalisationRequired { get; set; }
    }
}
