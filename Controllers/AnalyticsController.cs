using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;
using RedCrossBackend.Model;
using RedCrossBackend.Model.DTO;

namespace RedCrossBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnalyticsController : ControllerBase
    {

        private readonly ILogger<AnalyticsController> _logger;
        private readonly DB_RedCrossContext _context;

        public AnalyticsController(ILogger<AnalyticsController> logger, DB_RedCrossContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        [HttpGet]
        [Route("assistances")]
        public ActionResult<IEnumerable<string>> GetAssistances()
        {
            var ass = _context.Assistance.Where(x => x.name != null).Select(x=>x.name).OrderBy(x=>x).ToList();
            if(ass.Count > 0)
            {
                return ass;
            }
            return BadRequest("No Assistances Found");
        }
       
        [HttpGet]
        [Route("injuries")]
        public ActionResult<IEnumerable<string>> GetInjuries()
        {
            var inj = _context.Injury.Where(x => x.name != null).Select(x => x.name).OrderBy(x => x).ToList();
            if (inj.Count > 0)
            {
                return inj;
            }
            return BadRequest("No Injuries Found");
        }
        
        [HttpGet]
        [Route("countries")]
        public ActionResult<IEnumerable<string>> GetCountries()
        {
            var countries = _context.FirstAid.Where(x => x.country != null).Select(x => x.country).OrderBy(x => x).Distinct().ToList();
            if (countries.Count > 0)
            {
                return countries;
            }
            return BadRequest("No Countries Found");
        }

        [HttpGet]
        [Route("educations")]
        public ActionResult<IEnumerable<string>> GetEductations()
        {
            var educations = _context.FirstAid.Where(x => x.education != null).Select(x => x.education).OrderBy(x => x).Distinct().ToList();
            if (educations.Count > 0)
            {
                return educations;
            }
            return BadRequest("No Educations Found");
        }
        [HttpGet]
        [Route("ages")]
        public ActionResult<IEnumerable<string>> GetAges()
        {
            var ages = _context.FirstAid.Where(x => x.age != null).Select(x => x.age).OrderBy(x => x).Distinct().ToList();
            if (ages.Count > 0)
            {
                return ages;
            }
            return BadRequest("No Ages Found");
        }




        [HttpGet]
        [Route("stats")]
        public ActionResult<AnalyticsDTO> GetStatistics([FromQuery]Filter f)
        {
            var firstAidsDB = _context.FirstAid;
            var firstAids = ExecuteFilter(firstAidsDB, f);
            var ids = firstAids.Select(x => x.id).ToList();

            var analytics = new AnalyticsDTO();

            //byAge
            analytics.byAge = CalculateCombinationAnalytics(firstAids,1);

            //byEducation
            analytics.byEducation = CalculateCombinationAnalytics(firstAids, 2);

            //byCorrectSolution
            analytics.byCorrectSolution = CalculateCombinationAnalytics(firstAids, 3);

            //byHospitalization
            analytics.byHospitalization = CalculateCombinationAnalytics(firstAids, 4);

            //byMap
            var mapList = new List<Coordinates>();
            foreach(var el in firstAids)
            {
                if(el.latitude != 0 && el.longitude != 0)
                    mapList.Add(new Coordinates(el.latitude, el.longitude));
            }
            analytics.byMap = new StatsMap(mapList);

            //byGender
            analytics.byGender = CalculateCombinationAnalytics(firstAids, 5);

            //byNumberTraining
            analytics.byNumberTraining = CalculateCombinationAnalytics(firstAids, 6);

            //byInjury
            var injuries = _context.Injury.ToList();
            var fainjuries = _context.FAInjury.ToList();
            fainjuries = fainjuries.Where(x => ids.Contains(x.FAId)).ToList();
            var injuryList = new List<Combination>();
            foreach (var el in injuries)
                if(fainjuries.Where(x => x.IId == el.id).Count()>0)
                injuryList.Add(new Combination(el.name,fainjuries.Where(x=>x.IId == el.id).Count()));
            analytics.byInjury = injuryList;

            //byAssistance
            var assistances = _context.Assistance.ToList();
            var faassistances = _context.FAAssistance.ToList();
            faassistances = faassistances.Where(x => ids.Contains(x.FAId)).ToList();
            var assistanceList = new List<Combination>();
            foreach (var el in assistances)
                if(faassistances.Where(x => x.AId == el.id).Count()>0)
                assistanceList.Add(new Combination(el.name, faassistances.Where(x => x.AId == el.id).Count()));
            analytics.byAssistance = assistanceList;

            //byTraining
            var trainingList = new List<Combination>();
            var trained = firstAids.Where(x => (x.hadFATraining == null) ? false : (bool)x.hadFATraining).ToList();
            var rcTraining = trained.Where(x => (x.trainingByRC == null) ? false : (bool)x.trainingByRC).Count();
            var otherTraining = trained.Where(x => (x.otherTrainingProvider == null) ? false : true).Count();
            var bothTraining = firstAids.Where(x => (x.otherTrainingProvider == null || x.trainingByRC == null) ? false : (bool)x.hadFATraining).Count();
            var noTraining = firstAids.Where(x => (x.hadFATraining == null) ? true : !(bool)x.hadFATraining).Count();
            if (rcTraining > 0)
                trainingList.Add(new Combination("Red Cross FA Training", rcTraining));
            if (otherTraining > 0)
                trainingList.Add(new Combination("Other FA Training", otherTraining));
            if (bothTraining > 0)
                trainingList.Add(new Combination("Red Cross & Other FA Training", bothTraining));
            if (noTraining > 0)
                trainingList.Add(new Combination("No FA Training", noTraining));

            //byBlended
            analytics.byBlended = CalculateCombinationAnalytics(firstAids, 7);

            //byPercentProfHelp
            var byPhNeeded = CalculateCombinationAnalytics(firstAids, 8);
            analytics.byProfHelp = byPhNeeded;

            return analytics;
        }
        
        [HttpGet]
        [Route("raw")]
        public ActionResult<IEnumerable<FirstAidRaw>> GetRawFirstAid([FromQuery]Filter f)
        {
            var rawList = CreateRawList(f);

            return rawList;
        }
       
        [HttpGet]
        [Route("export")]
        public ActionResult ExportCSV([FromQuery]Filter f)
        {
            var rawList = CreateRawList(f);


            if (rawList.Count == 0)
                return BadRequest("No Elements in CSV for the applied Filter");

            var sw = new StringWriter();
            var config = new CsvHelper.Configuration.Configuration()
            {
                Delimiter = ";",
                HasHeaderRecord = false,
                TrimOptions = CsvHelper.Configuration.TrimOptions.Trim
            };
            var csv = new CsvWriter(sw, config);

            /// Add headers
            foreach (var e in typeof(FirstAidRaw).GetProperties().Select(x=>x.Name))
            {
                csv.WriteField(e);
            }
            csv.NextRecord();

            var dateNow = DateTime.Now;
            string fileName = "RedCross_FA_Export_" + dateNow.Day + dateNow.Month + dateNow.Year + dateNow.Hour + dateNow.Minute;

            fileName += ".csv";

            csv.WriteRecords(rawList);
            csv.Flush();

            var bytes = Encoding.Unicode.GetBytes(sw.ToString());
            return new FileContentResult(bytes, "text/csv")
            {
                FileDownloadName = fileName
            };
        }

        private List<FirstAid> ExecuteFilter(IQueryable<FirstAid> fas,Filter f)
        {
            var query = fas;

            //Execute Filters
            if (!string.IsNullOrEmpty(f.gender))
                query = query.Where(x => x.gender != null && x.gender.Equals(f.gender));
            if (!string.IsNullOrEmpty(f.age))
                query = query.Where(x => x.age != null && x.age.Equals(f.age));
            if (!string.IsNullOrEmpty(f.country))
                query = query.Where(x => x.country != null && x.country.Equals(f.country));
            if (f.from.HasValue)
                query = query.Where(x => x.assignDate != null && x.assignDate >= (DateTime)f.from);
            if(f.to.HasValue)
                query = query.Where(x => x.assignDate != null && x.assignDate <= (DateTime)f.to);
            if (!string.IsNullOrEmpty(f.education))
                query = query.Where(x => x.education != null && x.education.Equals(f.education));
            if (!string.IsNullOrEmpty(f.assistance))
            {
                var assistanceId = _context.Assistance.FirstOrDefault(x => x.name.Equals(f.assistance)).id;
                var ids = _context.FAAssistance.Where(x => x.AId == assistanceId).Select(x => x.FAId).ToList();
                query = query.Where(x => x.id != null && ids.Contains((int)x.id));
            }
            if (!string.IsNullOrEmpty(f.injury))
            {
                var injuryId = _context.Injury.FirstOrDefault(x => x.name.Equals(f.injury)).id;
                var ids = _context.FAInjury.Where(x => x.IId == injuryId).Select(x => x.FAId).ToList();
                query = query.Where(x => x.id != null && ids.Contains((int)x.id));
            }
            if (!string.IsNullOrEmpty(f.typeOfFA))
            {
                switch (f.typeOfFA)
                {
                    case "Red Cross FA Training(s)": 
                        query = query.Where(x => (x.trainingByRC == null) ? false : (bool)x.trainingByRC);
                        break;
                    case "Other FA Training(s)":
                        query = query.Where(x => (x.hadFATraining == null) ? false : ((bool)x.hadFATraining && x.otherTrainingProvider != null));
                        break;
                    case "Red Cross & Other FA Trainings": 
                        query = query.Where(x => (x.hadFATraining == null || x.trainingByRC == null) ? false : ((bool)x.trainingByRC && x.otherTrainingProvider != null));
                        break;
                    case "None":
                        query = query.Where(x => (x.hadFATraining == null) ? true : !(bool)x.hadFATraining);
                        break;
                    default:break;
                }
            }

            return query.ToList();
        }
        private List<Combination> CalculateCombinationAnalytics(List<FirstAid> fas, int param)
        {
            var distincts = fas.Select(x => x.age).Distinct().ToList();
            switch (param)
            {
                //age
                case 1:
                    distincts = fas.Where(x => x.age != null).Select(x => x.age).Distinct().ToList();
                    break;
                //education
                case 2:
                    distincts = fas.Where(x => x.education != null).Select(x => x.education).Distinct().ToList(); 
                    break;
                //correctSolution
                case 3:
                    distincts = fas.Where(x => x.age != null).Select(x => x.age).Distinct().ToList(); 
                    break;
                //hosp
                case 4:
                    distincts = fas.Where(x => x.hospitalisationRequired != null).Select(x => (x.hospitalisationRequired != null)?x.hospitalisationRequired.ToString():null).Distinct().ToList();
                    break;
                //gender
                case 5:
                    distincts = fas.Where(x => x.gender != null).Select(x => x.gender).Distinct().ToList();
                    break;
                //numbertraining
                case 6:
                    distincts = fas.Where(x => x.numberOffFATtraining != null).Select(x => (x.numberOffFATtraining != null) ? x.numberOffFATtraining.ToString():null).Distinct().ToList();
                    break;
                //blended
                case 7:
                    distincts = fas.Where(x => x.blendedTraining != null).Select(x => (x.blendedTraining != null) ? x.blendedTraining.ToString():null).Distinct().ToList();
                    break;
                case 8:
                    distincts = fas.Where(x => x.phNeeded != null).Select(x => x.phNeeded).Distinct().ToList();
                    break;
            }
            var list = new List<Combination>();
            foreach (var el in distincts)
            {
                int count = 0;
                switch (param)
                {
                    //age
                    case 1: count = fas.Where(x => x.age != null && x.age.Equals(el)).Count();
                        break;
                    //education
                    case 2:
                        count = fas.Where(x => x.education != null && x.education.Equals(el)).Count();
                        break;
                    //correctSolution
                    case 3:
                        count = fas.Where(x => x.age != null && x.age.Equals(el)).Count();
                        break;
                    //hosp
                    case 4:
                        count = fas.Where(x => x.hospitalisationRequired != null && x.hospitalisationRequired.ToString().Equals(el)).Count();
                        break;
                    //gender
                    case 5:
                        count = fas.Where(x => x.gender != null && x.gender.Equals(el)).Count();
                        break;
                    //numbertraining
                    case 6:
                        count = fas.Where(x => x.numberOffFATtraining != null && x.numberOffFATtraining.ToString().Equals(el)).Count();
                        break;
                    //blended
                    case 7:
                        count = fas.Where(x => x.blendedTraining != null && x.blendedTraining.ToString().Equals(el)).Count();
                        break;
                    //PhNeeded
                    case 8:
                        count = fas.Where(x => x.phNeeded != null && x.phNeeded.Equals(el)).Count();
                        break;
                }
                list.Add(new Combination(el,count));
            }
                
            return list;
        }
        private List<FirstAidRaw> CreateRawList(Filter f)
        {
            var rawList = new List<FirstAidRaw>();
            var firstAidsDB = _context.FirstAid;

            var firstAids = ExecuteFilter(firstAidsDB, f);


            var injJoin = _context.Injury.Join(_context.FAInjury,
                inj => inj.id, 
                fa => fa.IId, 
                (inj, fa) => new { Inj = inj, Fa = fa }).ToList();
            var assJoin = _context.Assistance.Join(_context.FAAssistance, ass => ass.id, fa => fa.AId, (ass, fa) => new { Ass = ass, Fa = fa }).ToList();
            var phtJoin = _context.PhType.Join(_context.FaphType, ph => ph.id, fa => fa.PTId, (ph, fa) => new { Ph = ph, Fa = fa }).ToList();

            foreach (var fa in firstAids)
            {


                string injuries = string.Join(", ", injJoin.Where(x => x.Fa.FAId == fa.id).Select(x => x.Inj.name));
                string assistances = string.Join(", ", assJoin.Where(x => x.Fa.FAId == fa.id).Select(x => x.Ass.name));
                string phtypes = string.Join(", ", phtJoin.Where(x => x.Fa.FAId == fa.id).Select(x => x.Ph.name));
                rawList.Add(new FirstAidRaw(fa, injuries, assistances, phtypes));
            }

            return rawList;
        }
    }
    public class Filter
    {
        public string gender { get; set; }
        public string typeOfFA { get; set; }
        public string injury { get; set; }
        public string assistance { get; set; }
        public string age { get; set; }
        public string country { get; set; }
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
        public string education { get; set; }
    }
}
