using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class HelperConfig
    {
        public string MagcoreApiMatch { get; set; }
        public string MagcoreApiTrain { get; set; }
        public string InternalDb { get; set; }
        public string WxAppid { get; set; }
        public string WxSecret { get; set; }
        public string WxInterfaceHost { get; set; }

        public string WxMenu { get; set; }
        public static HelperConfig Current { get;set;}
    }
}
