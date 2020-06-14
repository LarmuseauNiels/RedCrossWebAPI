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
    public class AppController : ControllerBase
    {

        private readonly ILogger<TestController> _logger;
        private readonly DB_RedCrossContext context;

        public AppController(ILogger<TestController> logger, DB_RedCrossContext dbContext)
        {
            _logger = logger;
            context = dbContext;
        }

        [HttpPost]
        public bool SaveForm(FirstAid fa)
        {
            return true;
        }
    }
}
