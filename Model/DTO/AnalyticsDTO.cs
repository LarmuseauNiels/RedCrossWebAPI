using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedCrossBackend.Model.DTO
{
    public class AgeRange
    {
        public string range { get; set; }
        public int startyear { get; set; }
        public int endYear { get; set; }
        public AgeRange(string range,int end, int start)
        {
            this.range = range;
            this.startyear = start;
            this.endYear = end;
        }
    }
    public static class AgeRanges
    {
        public static List<AgeRange> GetAgeRanges()
        {
            var list = new List<AgeRange>();
            list.Add(new AgeRange("<15", 0, 15));
            int age = 15;
            for(int i = 0; i < 7; i++)
            {
                list.Add(new AgeRange(age + "-" + (age + 10), age, age + 10));
                age += 10;
            }

            list.Add(new AgeRange(">85", 85, 150));
            return list;
        }



    }
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
        public List<Combination> byProfHelp { get; set; }
    }
}
