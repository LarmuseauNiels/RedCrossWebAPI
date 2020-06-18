using System;
using System.Collections.Generic;

namespace RedCrossBackend.Model
{
    public class FirstAid
    {
        public int? id { get; set; }
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
        public double longitude { get; set; }
        public double latitude { get; set; }
        public string setting { get; set; }
        public int? confidentApplyingFA { get; set; }
        public bool? phNeeded { get; set; }
        public string phTimeToArrive { get; set; }
        public string hospitalisationRequired { get; set; }
        public string country { get; set; }
        public FirstAid(){}
        public FirstAid(FirstAid fa)
        {
            this.id = fa.id;
            this.assignDate = fa.assignDate;
            this.macAddress = fa.macAddress;
            this.gender = fa.gender;
            this.age = fa.age;
            this.education = fa.education;
            this.hadFATraining = fa.hadFATraining;
            this.numberOffATtraining = fa.numberOffATtraining;
            this.trainingByRC = fa.trainingByRC;
            this.blendedTraining = fa.blendedTraining;
            this.otherTrainingProvider = fa.otherTrainingProvider;
            this.longitude = fa.longitude;
            this.latitude = fa.latitude;
            this.setting = fa.setting;
            this.confidentApplyingFA = fa.confidentApplyingFA;
            this.phNeeded = fa.phNeeded;
            this.phTimeToArrive = fa.phTimeToArrive;
            this.hospitalisationRequired = fa.hospitalisationRequired;
            this.country = fa.country;
    }
    }
}
