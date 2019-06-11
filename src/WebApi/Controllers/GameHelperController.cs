using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SystemCommonLibrary.Json;
using WebApi.Components.Extension;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GameHelperController : ControllerBase
    {
        [HttpPost]
        public ContentResult New(string oid, string s, int color, string map)
        {
            

            return new ContentResult();
        }

        [HttpGet]
        public async Task<bool> Finish()
        {
            return true;
        }
    }
}
