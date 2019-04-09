using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    /// <summary>
    /// 与微信交互的接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WxHelperController : ControllerBase
    {
    }
}