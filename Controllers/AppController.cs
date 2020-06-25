using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using RedCrossBackend.Model;

namespace RedCrossBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppController : ControllerBase
    {

        private readonly ILogger<AppController> _logger;
        private readonly DB_RedCrossContext _context;

        public AppController(ILogger<AppController> logger, DB_RedCrossContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpPost]
        public ActionResult<FirstAid> SaveForm(FirstAidDTO dto)
        {
            FirstAid firstAid = dto;
            ReverseGeocoder y = new ReverseGeocoder();

            var t = new ReverseGeocodeRequest
            {
                Latitude = firstAid.latitude,
                Longitude = firstAid.longitude,
                PreferredLanguages = "en",

            };
            Task<GeocodeResponse> r2 = y.ReverseGeocode(t);
            
            GeocodeResponse resp = r2.Result;
            firstAid.country = resp.Address.Country;
            firstAid.NullToDefault();
            _context.FirstAid.Add(firstAid);
            _context.SaveChanges();
            var id = _context.FirstAid.OrderByDescending(x => x.id).FirstOrDefault().id;
            if (id == null)
            {
                return NotFound();
            }
            if (dto.injury != null && dto.injury.Count() > 0)
            {
                var injuries = _context.Injury.ToList();
                foreach (var injury in dto.injury)
                {
                    var inj = injuries.FirstOrDefault(x => x.name.Equals(injury));
                    if (inj != null)
                        _context.Add(new Fainjury((int)id, inj.id));
                }
            }
            if (dto.assistance != null && dto.assistance.Count() > 0) { 
                var assistances = _context.Assistance.ToList();
                foreach (var assistance in dto.assistance)
                {
                    var ass = assistances.FirstOrDefault(x => x.name.Equals(assistance));
                    if (ass != null)
                        _context.Add(new Faassistance((int)id, ass.id));
                }
            }
            if (dto.phType != null && dto.phType.Count() > 0)
            {
                var phTypes = _context.PhType.ToList();
                foreach(var phType in dto.phType)
                {
                    var ph = phTypes.FirstOrDefault(x => x.name.Equals(phType));
                    if (ph != null)
                        _context.Add(new FaphType((int)id, ph.id));
                }
            }
            _context.SaveChanges();
            return firstAid;
        }
    }
}
