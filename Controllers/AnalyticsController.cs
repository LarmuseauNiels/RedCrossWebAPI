using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using RedCrossBackend.Model;

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
        [Route("assistance")]
        public ActionResult<List<string>> GetAssistances()
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
        public ActionResult<List<string>> GetInjuries()
        {
            var inj = _context.Injury.Select(x => x.name).ToList();
            if (inj.Count > 0)
            {
                return inj;
            }
            return NotFound();
        }
        [HttpGet]
        [Route("raw")]
        public ActionResult<List<FirstAidRaw>> GetRawFirstAid(Filter f)
        {
            var rawList = new List<FirstAidRaw>();
            var firsAids = _context.FirstAid.ToList();
            var inj = _context.Injury.ToList();
            var ass = _context.Assistance.ToList();
            var pht = _context.PhType.ToList();

            //Execute filter here

            foreach (var fa in firsAids)
            {
                string injuries = "";
                string assistances = "";
                string phtypes = "";
                foreach (var i in _context.FAInjury.Where(x=>x.FAId == fa.id).ToList())
                    injuries += (injuries.Equals(""))?"": ", " + inj.FirstOrDefault(x => x.id == i.IId).name;
                foreach (var a in _context.FAAssistance.Where(x => x.FAId == fa.id).ToList())
                    assistances += (assistances.Equals("")) ? "" : ", " + inj.FirstOrDefault(x => x.id == a.AId).name;
                foreach (var p in _context.FaphType.Where(x => x.FAId == fa.id).ToList())
                    phtypes += (phtypes.Equals("")) ? "" : ", " + inj.FirstOrDefault(x => x.id == p.PTId).name;
                rawList.Add(new FirstAidRaw(fa,injuries,assistances,phtypes));
            }

            if(rawList.Count == 0)
                return NotFound();

            return rawList;
        }


    }
    public class Filter
    {
        
    }
}
