using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GameHelperController : ControllerBase
    {
        private readonly HelperConfig _helperConfig;

        public GameHelperController(IOptions<HelperConfig> settings)
        {
            _helperConfig = settings.Value;
        }

        // GET api/values
        [HttpGet]
        public ContentResult New()
        {

            return new ContentResult();
        }

        [HttpPut]
        public async Task<bool> ModifyIntegral()
        {
            return true;
        }
    }
}
