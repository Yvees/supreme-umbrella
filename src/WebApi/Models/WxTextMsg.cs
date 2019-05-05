using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class WxTextMsg
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
        /// 消息类型，文本为text
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 消息类型（文本消息内容）
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 消息类型（消息id，64位整型）
        /// </summary>
        public long MsgId { get; set; }

        public WxTextMsg() { }
        public WxTextMsg(string to, string from, long time, string content)
        {
            ToUserName = to;
            FromUserName = from;
            MsgType = "text";
            CreateTime = time;
            Content = content;
            MsgId = 0;
        }

        public static WxTextMsg FromXml(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return null;
            }

            var ins = new WxTextMsg();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(xml);
            ins.ToUserName = doc.DocumentNode.Descendants("ToUserName").FirstOrDefault().InnerHtml;
            ins.FromUserName = doc.DocumentNode.Descendants("FromUserName").FirstOrDefault().InnerHtml;
            ins.CreateTime = Convert.ToInt64(doc.DocumentNode.Descendants("CreateTime").FirstOrDefault().InnerHtml);
            ins.MsgType = doc.DocumentNode.Descendants("MsgType").FirstOrDefault().InnerHtml;
            ins.Content = doc.DocumentNode.Descendants("Content").FirstOrDefault().InnerHtml;
            ins.MsgId = Convert.ToInt64(doc.DocumentNode.Descendants("MsgId").FirstOrDefault().InnerHtml);

            return ins;
        }

        public string ToXml()
        {
            return string.Format(xmlTpl, ToUserName, FromUserName,
                CreateTime, MsgType, Content);
        }

        private string xmlTpl = @"
            <xml>
              <ToUserName>{0}</ToUserName>
              <FromUserName>{1}</FromUserName>
              <CreateTime>{2}</CreateTime>
              <MsgType>{3}</MsgType>
              <Content>{4}</Content>
            </xml>
        ";
    }
}
