using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SystemCommonLibrary.Json;
using WebApi.Components.Extension;
using WebApi.Components.Manager;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GameHelperController : ControllerBase
    {
        [HttpPost]
        public async Task<ContentResult> New([FromBody]dynamic json)
        {
            if (json == null
                || json.oid == null || string.IsNullOrEmpty(json.oid)
                || json.s == null || string.IsNullOrEmpty(json.s)
                || json.color == null || string.IsNullOrEmpty(json.color.ToString())
                || json.map == null || string.IsNullOrEmpty(json.map))
                return new ContentResult() { StatusCode = 404 };
            else
            {
                var user = await DbEntityManager.SelectOne<WxUser>("openid", json.oid);
                if (user.salt != json.s)
                    return new ContentResult() { StatusCode = 401 };
                else
                {
                    string openid = json.oid;
                    string map = json.map;
                    int color = json.color;
                    string name = user.name;

                    //init integral
                    UserIntegral userIntegral = null;
                    if (await DbEntityManager.Exist<UserIntegral>("wx_openid", openid))
                    {
                        userIntegral = await DbEntityManager.SelectOne<UserIntegral>("wx_openid", openid);
                    }
                    else
                    {
                        userIntegral = new UserIntegral()
                        {
                            wx_openid = openid,
                            mc_integral = 0,
                            total_integral = 0
                        };

                        await DbEntityManager.Insert(userIntegral);
                    }

                    //join or creat a game
                    string gid = await GameManager.JoinOneGame(
                        userIntegral.mc_integral < HelperConfig.Current.IntegralToJoin,
                        map, name, color, openid);

                    return new ContentResult() { StatusCode = 200, Content = gid };
                }
            }
        }

        [HttpGet]
        public async Task<bool> Finish()
        {
            return true;
        }
    }
}
