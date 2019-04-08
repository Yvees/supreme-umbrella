using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ContentRuslt NewGame()
        {

            return new ContentRuslt();
        }

        [HttpPut]
        public async Task<bool> ModifyIntegral()
        {
            return true;
        }
    }
}
