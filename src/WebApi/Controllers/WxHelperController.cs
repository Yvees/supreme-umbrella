using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Components.WxApi;

namespace WebApi.Controllers
{
    /// <summary>
    /// 与微信交互的接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WxHelperController : ControllerBase
    {
        [HttpGet]
        [ActionName("Responser")]
        public string Get(string signature, string timestamp, string nonce, string echostr)
        {
            if (WxBase.CheckSignature(signature, timestamp, nonce))
            {
                return echostr; //返回随机字符串则表示验证通过
            }
            else
            {
                Console.WriteLine($"failed check signature:{signature} with {timestamp},{nonce}");
                return string.Empty;
            }
        }

        [HttpPost]
        [ActionName("Responser")]
        public async Task<string> Post([FromBody] dynamic xml)
        {
            Console.WriteLine(xml);
            return "success";
        }
    }
}