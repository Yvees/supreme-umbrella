using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    /// <summary>
    /// 图文消息信息
    /// </summary>
    public class WxArticle
    {
        /// <summary>
        /// 图文消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图文消息描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图片链接，支持JPG、PNG格式，较好的效果为大图360*200，小图200*200
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 点击图文消息跳转链接
        /// </summary>
        public string Url { get; set; }

        public WxArticle(string title, string desc, string coverUrl, string url)
        {
            Title = title;
            Description = desc;
            PicUrl = coverUrl;
            Url = url;
        }

        public string ToXml()
        {
            return string.Format(xmlTpl, Title, Description, PicUrl, Url);
        }

        private string xmlTpl = @"
            <item>
                <Title>{0}</Title>
                <Description>{1}</Description>
                <PicUrl>{2}</PicUrl>
                <Url>{3}</Url>
            </item>
        ";
    }
}
