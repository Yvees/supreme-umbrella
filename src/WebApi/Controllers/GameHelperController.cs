using MagCore.Sdk.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using SystemCommonLibrary.Json;
using WebApi.Components.Manager;
using WebApi.Exceptions;
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
            if (json == null)
                return new ContentResult() { StatusCode = 404 };

            string openid = json.oid.ToString();
            string salt = json.s.ToString();
            int color = Convert.ToInt32(json.color.ToString());
            string map = json.map.ToString();

            if (string.IsNullOrEmpty(openid)
                || string.IsNullOrEmpty(salt)
                || color < 0
                || string.IsNullOrEmpty(map))
                return new ContentResult() { StatusCode = 404 };
            else
            {
                var user = await DbEntityManager.SelectOne<WxUser>("openid", openid);
                if (user.salt != salt)
                    return new ContentResult() { StatusCode = 401 };
                else
                {
                    string name = $"{user.nickname}({HashManager.Md5(openid).Substring(12, 4)})";

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

                    try
                    {
                        //join or creat a game
                        var result = await GameManager.JoinOneGame(
                            userIntegral.mc_integral >= HelperConfig.Current.IntegralToJoin,
                            map, name, color, openid);

                        return new ContentResult() { StatusCode = 200, Content = result.ToString() };
                    }
                    catch (GameJoinedException)
                    {
                        return new ContentResult() { StatusCode = 400, Content = "请等待上局结束" };
                    }
                }
            }
        }

        [HttpPost]
        public async Task<ContentResult> Finish([FromBody]dynamic json)
        {
            Game game = DynamicJson.Parse(json).Deserialize<Game>();
            using (var trans = new TransactionScope())
            {
                var scores = await DbEntityManager.Select<GameScore>("gid", game.Id);
                
                foreach (var player in game.Players)
                {
                    var score = scores.SingleOrDefault(s => s.pidx == player.Index);
                    if (score != null)
                    {
                        //integral 
                        var oid = score.oid;
                        var integral = await DbEntityManager.SelectOne<UserIntegral>("wx_openid", oid);

                        if (player.State == 1)
                        {
                            score.score = 1;
                            integral.mc_integral += 10;
                            integral.total_integral += 10;
                        }
                        else
                        {
                            score.score = -1;
                            integral.mc_integral += 1;
                            integral.total_integral += 1;
                        }

                        //update GameScore & integral
                        await DbEntityManager.Update<GameScore>(score);
                        await DbEntityManager.Update<UserIntegral>(integral);
                    }
                }

                trans.Complete();
            }

            return new ContentResult() { StatusCode = 200};
        }
    }
}
