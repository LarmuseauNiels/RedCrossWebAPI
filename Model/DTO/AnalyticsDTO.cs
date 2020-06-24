using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedCrossBackend.Model.DTO
{
    public class Combination
    {
        public string name { get; set; }
        public int y { get; set; }
        public Combination(string text, int amount)
        {
            this.name = text;
            this.y = amount;
        }
    }
    public class StatsMap
    {
        public double centerLatitude { get; set; }
        public double centerLongitude { get; set; }
        public List<Coordinates> coordinates { get; set; }
        public StatsMap(List<Coordinates> coo)
        {
            this.coordinates = coo;
            if(coo.Count() > 0)
            {
                this.centerLatitude = coo.Average(x => x.latitude);
                this.centerLongitude = coo.Average(x => x.longitude);
            }
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
        public StatsMap byMap { get; set; }
        public List<Combination> byGender { get; set; }
        public List<Combination> byNumberTraining { get; set; }
        public List<Combination> byInjury { get; set; }
        public List<Combination> byAssistance { get; set; }
        public List<Combination> byTraining { get; set; }
        public List<Combination> byBlended { get; set; }
        public double byPercentProfHelp { get; set; }
    }
}
