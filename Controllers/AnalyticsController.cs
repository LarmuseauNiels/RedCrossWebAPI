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
            var ass = _context.Assistance.Select(x=>x.name).ToList();
            if(ass.Count > 0)
            {
                return ass;
            }
            return NotFound();
        }
       
        [HttpGet]
        [Route("injuries")]
        public ActionResult<IEnumerable<string>> GetInjuries()
        {
            var inj = _context.Injury.Select(x => x.name).ToList();
            if (inj.Count > 0)
            {
                return inj;
            }
            return NotFound();
        }
        
        [HttpGet]
        [Route("countries")]
        public ActionResult<IEnumerable<string>> GetCountries()
        {
            var countries = _context.FirstAid.Select(x => x.country).Distinct().ToList();
            if (countries.Count > 0)
            {
                return countries;
            }
            return NotFound();
        }

        [HttpGet]
        [Route("educations")]
        public ActionResult<IEnumerable<string>> GetEductations()
        {
            var educations = _context.FirstAid.Select(x => x.education).Distinct().ToList();
            if (educations.Count > 0)
            {
                return educations;
            }
            return NotFound();
        }




        [HttpGet]
        [Route("stats")]
        public ActionResult<AnalyticsDTO> GetStatistics([FromQuery]Filter f)
        {
            var firstAidsDB = _context.FirstAid;
            var firstAids = ExecuteFilter(firstAidsDB, f);

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
                mapList.Add(new Coordinates(el.latitude, el.longitude));
            }
            analytics.byMap = mapList;

            //byGender
            analytics.byGender = CalculateCombinationAnalytics(firstAids, 5);

            //byNumberTraining
            analytics.byNumberTraining = CalculateCombinationAnalytics(firstAids, 6);

            //byInjury
            var injuries = _context.Injury.ToList();
            var fainjuries = _context.FAInjury.ToList();
            var injuryList = new List<Combination>();
            foreach (var el in injuries)
                injuryList.Add(new Combination(el.name,fainjuries.Where(x=>x.IId == el.id).Count()));
            analytics.byInjury = injuryList;

            //byAssistance
            var assistances = _context.Assistance.ToList();
            var faassistances = _context.FAAssistance.ToList();
            var assistanceList = new List<Combination>();
            foreach (var el in assistances)
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
            double perc = firstAids.Where(x => (x.phNeeded == null)?false:(bool)x.phNeeded).Count() / firstAids.Count()*100;
            analytics.byPercentProfHelp = perc;

            if (firstAids.Count != 0)
                return analytics;

            return NotFound();
        }
        
        [HttpGet]
        [Route("raw")]
        public ActionResult<IEnumerable<FirstAidRaw>> GetRawFirstAid([FromQuery]Filter f)
        {
            var rawList = CreateRawList(f);

            if (rawList.Count == 0)
                return NotFound();

            return rawList;
        }
       
        [HttpGet]
        [Route("export")]
        public ActionResult ExportCSV([FromQuery]Filter f)
        {
            var rawList = CreateRawList(f);


            if (rawList.Count == 0)
                return NotFound();

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
            return new FileContentResult(bytes, "application/octet-stream")
            {
                FileDownloadName = fileName
            };
        }

        private List<FirstAid> ExecuteFilter(IQueryable<FirstAid> fas,Filter f)
        {
            var query = fas;

            //Execute Filters
            if (!string.IsNullOrEmpty(f.gender))
                query = query.Where(x => x.gender.Equals(f.gender));
            if (!string.IsNullOrEmpty(f.age))
                query = query.Where(x => x.age.Equals(f.age));
            if (!string.IsNullOrEmpty(f.country))
                query = query.Where(x => x.country.Equals(f.country));
            if (f.from.HasValue)
                query = query.Where(x => x.assignDate >= (DateTime)f.from);
            if(f.to.HasValue)
                query = query.Where(x => x.assignDate <= (DateTime)f.to);
            if (!string.IsNullOrEmpty(f.education))
                query = query.Where(x => x.education.Equals(f.education));
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
                    distincts = fas.Select(x => x.age).Distinct().ToList();
                    break;
                //education
                case 2:
                    distincts = fas.Select(x => x.education).Distinct().ToList(); 
                    break;
                //correctSolution
                case 3:
                    distincts = fas.Select(x => x.age).Distinct().ToList(); 
                    break;
                //hosp
                case 4:
                    distincts = fas.Select(x => (x.hospitalisationRequired != null)?x.hospitalisationRequired.ToString():null).Distinct().ToList();
                    break;
                //gender
                case 5:
                    distincts = fas.Select(x => x.gender).Distinct().ToList();
                    break;
                //numbertraining
                case 6:
                    distincts = fas.Select(x => (x.numberOffATtraining != null) ? x.numberOffATtraining.ToString():null).Distinct().ToList();
                    break;
                //blended
                case 7:
                    distincts = fas.Select(x => (x.blendedTraining != null) ? x.blendedTraining.ToString():null).Distinct().ToList();
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
                        count = fas.Where(x => x.numberOffATtraining != null && x.numberOffATtraining.ToString().Equals(el)).Count();
                        break;
                    //blended
                    case 7:
                        count = fas.Where(x => x.blendedTraining != null && x.blendedTraining.ToString().Equals(el)).Count();
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
            var inj = _context.Injury.ToList();
            var ass = _context.Assistance.ToList();
            var pht = _context.PhType.ToList();

            var firstAids = ExecuteFilter(firstAidsDB, f);

            foreach (var fa in firstAids)
            {
                string injuries = "";
                string assistances = "";
                string phtypes = "";
                var t = _context.FAInjury.Where(x => x.FAId == fa.id).ToList();
                var asss = _context.FAAssistance.Where(x => x.FAId == fa.id).ToList();
                var phtdd = _context.FaphType.Where(x => x.FAId == fa.id).ToList();
                foreach (var i in _context.FAInjury.Where(x => x.FAId == fa.id).ToList())
                    injuries += ((injuries.Equals("")) ? "" : ", ") + inj.FirstOrDefault(x => x.id == i.IId).name;
                foreach (var a in _context.FAAssistance.Where(x => x.FAId == fa.id).ToList())
                    assistances += ((assistances.Equals("")) ? "" : ", ") + ass.FirstOrDefault(x => x.id == a.AId).name;
                foreach (var p in _context.FaphType.Where(x => x.FAId == fa.id).ToList())
                    phtypes += ((phtypes.Equals("")) ? "" : ", ") + pht.FirstOrDefault(x => x.id == p.PTId).name;
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
