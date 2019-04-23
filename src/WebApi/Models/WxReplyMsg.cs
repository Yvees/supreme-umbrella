using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class WxReplyMsg
    {
        /// <summary>
        /// 接收方帐号（收到的OpenID）
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 消息类型，图文为news
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 图文消息个数；当用户发送文本、图片、视频、图文、地理位置这五种消息时，开发者只能回复1条图文消息；其余场景最多可回复8条图文消息
        /// </summary>
        public int ArticleCount { get { return Articles.Count; } }

        /// <summary>
        /// 图文消息信息，注意，如果图文数超过限制，则将只发限制内的条数
        /// </summary>
        public List<WxArticle> Articles { get; set; }

        public WxReplyMsg(string to, string from, long time, string type, WxArticle[] items)
        {
            ToUserName = to;
            FromUserName = from;
            CreateTime = time;
            MsgType = type;
            Articles = new List<WxArticle>(items);
        }

        public string ToXml()
        {
            string articles = string.Empty;
            foreach (var article in Articles)
            {
                articles += article.ToXml();
            }

            return string.Format(xmlTpl, ToUserName, FromUserName,
                CreateTime, MsgType, ArticleCount, articles);
        }

        private string xmlTpl = @"
            <xml>
              <ToUserName>{0}</ToUserName>
              <FromUserName>{1}</FromUserName>
              <CreateTime>{2}</CreateTime>
              <MsgType>{3}</MsgType>
              <ArticleCount>{4}</ArticleCount>
              <Articles>
                {5}
              </Articles>
            </xml>
        ";
    }
}
