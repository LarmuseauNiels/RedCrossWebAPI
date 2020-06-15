using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;

namespace RedCrossBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {

        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        //[HttpGet]
        //public Test Get()
        //{
        //    var rng = new Random();
        //    return new Test { example = "Basic Test" };
        //}

        [HttpGet]
        public GeocodeResponse ReverseGeoCode(double lat, double longitu )
        {
            ReverseGeocoder y = new ReverseGeocoder();

            Task<GeocodeResponse> r2 = y.ReverseGeocode(new ReverseGeocodeRequest
            {
                Latitude = lat,
                Longitude = longitu,
                

            });
            GeocodeResponse resp = r2.Result;
            //string Countrycode = resp.Address.CountryCode;
            return resp;
        }
    }
}
