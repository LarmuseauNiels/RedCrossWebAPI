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
        public int? numberOffFATtraining { get; set; }
        public bool? trainingByRC { get; set; }
        public bool? blendedTraining { get; set; }
        public string otherTrainingProvider { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public string setting { get; set; }
        public int? confidentApplyingFA { get; set; }
        public string phNeeded { get; set; }
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
            this.numberOffFATtraining = fa.numberOffFATtraining;
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
        public void NullToDefault()
        {
            if (this.hadFATraining == null)
                this.hadFATraining = false;
            if (this.trainingByRC == null)
                this.trainingByRC = false;
            if (this.blendedTraining == null)
                this.blendedTraining = false;
            if (this.phNeeded == null)
                this.phNeeded = "Unknown";
            if (this.assignDate == null)
                this.assignDate = DateTime.Now;
            if (this.hospitalisationRequired == null)
                this.hospitalisationRequired = "Unknown";
            if (this.macAddress == null)
                this.macAddress = GenerateRandomMacAddress();
        }
        private string GenerateRandomMacAddress()
        {
            Random random = new Random();
            string maca = "";
            for (int i = 0; i < 12; i++)
            {
                if (i % 2 == 0)
                {
                    if (i != 0)
                        maca += "-";
                }
                int rand = random.Next(0, 16);
                switch (rand)
                {
                    case 10: maca += "A"; break;
                    case 11: maca += "B"; break;
                    case 12: maca += "C"; break;
                    case 13: maca += "D"; break;
                    case 14: maca += "E"; break;
                    case 15: maca += "F"; break;
                    default:
                        if (rand < 10)
                            maca += rand.ToString();
                        break;
                }
            }
            return maca;
        }
    }
}
