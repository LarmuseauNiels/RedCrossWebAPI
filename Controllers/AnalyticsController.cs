using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RedCrossBackend.Model;

namespace RedCrossBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnalyticsController : ControllerBase
    {

        private readonly ILogger<TestController> _logger;
        private readonly DB_RedCrossContext context;

        public AnalyticsController(ILogger<TestController> logger, DB_RedCrossContext dbContext)
        {
            _logger = logger;
            context = dbContext;
        }

        [HttpGet]
        public Test Get()
        {
            var rng = new Random();
            return new Test { example = "Basic Test" };
        }
    }
}
