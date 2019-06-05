using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class WxUser
    {
        [Key]
        public int id { get; set; }

        public string openid { get; set; }

        public int sex { get; set; }

        public string nickname { get; set; }

        public string headimgurl { get; set; }

        public string salt { get; set; }

        public static WxUser CreateFromWxUserInfo(WxUserInfo info)
        {
            var user = new WxUser();
            user.openid = info.openid;
            user.sex = info.sex;
            user.nickname = info.nickname;
            user.headimgurl = info.headimgurl;

            var rnd = new Random(DateTime.Now.Millisecond).NextDouble();
            user.salt = rnd.ToString().Substring(2, 8);

            return user;
        }
    }
}
