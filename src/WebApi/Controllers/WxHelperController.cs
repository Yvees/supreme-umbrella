using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Components.WxApi;
using WebApi.Models;

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
            if (string.IsNullOrEmpty(signature) 
                || string.IsNullOrEmpty(timestamp)
                || string.IsNullOrEmpty(nonce))
            {
                return string.Empty;
            }

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
        public async Task<ContentResult> Post(string signature, string timestamp, string nonce, string openid)
        {
            using (Stream stream = Request.Body)
            {
                byte[] buffer = new byte[Request.ContentLength.Value];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                string xml = Encoding.UTF8.GetString(buffer);
                Console.WriteLine($"收到：{xml}");
                var msg = WxTextMsg.FromXml(xml);

                if (msg.MsgType.Contains("text")
                    && msg.Content.Contains("磁芯大战"))
                {
                    //
                    var info = await WxBase.GetUserInfo(openid);
                    Console.WriteLine($"获取：{info.nickname} 信息");


                    //
                    var article = new WxArticle("MagCore - 磁芯大战", "进入房间创建游戏",
                        HelperConfig.Current.WxInterfaceHost + "assets/images/icon.png",
                        HelperConfig.Current.WxInterfaceHost + "pages/creator.html"
                            + "?t=" + DateTime.Now.Ticks.ToString()
                            + "&oid=" + openid + "&name=" + info.nickname );
                    var reply = new WxArticleMsg(msg.FromUserName, msg.ToUserName, 
                        msg.CreateTime, new WxArticle[] { article });
                    string text = reply.ToXml();
                    Console.WriteLine($"回复：{text}");
                    
                    return new ContentResult() { StatusCode = 200, Content = text };
                }
                else
                {
                    //
                    var defaultReply = new WxTextMsg(msg.FromUserName, msg.ToUserName, msg.CreateTime,
                               "未正确识别，磁芯大战比赛期间，其他回复暂时停止服务");

                    return new ContentResult() { StatusCode = 200, Content = defaultReply.ToXml() };
                }
            }
        }        
    }
}
