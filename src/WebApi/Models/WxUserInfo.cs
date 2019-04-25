using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class WxUserInfo
    {
        public int subscribe { get; set; }
        public string openid { get; set; }
        public string nickname { get; set; }
        public int sex { get; set; }
        public string language { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        public long subscribe_time { get; set; }
        public string remark { get; set; }
        public object[] tagid_list { get; set; }
        public string subscribe_scene { get; set; }
        public int qr_scene { get; set; }
        public string qr_scene_str { get; set; }
    }
}
