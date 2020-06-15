using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedCrossBackend.Model.DTO
{
    public class Combination
    {
        public string text { get; set; }
        public int amount { get; set; }
        public Combination(string text, int amount)
        {
            this.text = text;
            this.amount = amount;
        }
    }
    public class Coordinates
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public Coordinates(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }
    }
    public class AnalyticsDTO
    {
        public List<Combination> byAge { get; set; }
        public List<Combination> byEducation { get; set; }
        public List<Combination> byCorrectSolution { get; set; }
        public List<Combination> byHospitalization { get; set; }
        public List<Coordinates> byMap { get; set; }
        public List<Combination> byGender { get; set; }
        public List<Combination> byNumberTraining { get; set; }
        public List<Combination> byInjury { get; set; }
        public List<Combination> byAssistance { get; set; }
        public List<Combination> byTraining { get; set; }
        public List<Combination> byBlended { get; set; }
        public double byPercentProfHelp { get; set; }
    }
}
