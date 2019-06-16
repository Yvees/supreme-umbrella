using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class UserIntegral
    {
        [Key]
        public int id { get; set; }

        public string wx_openid { get; set; }

        public int mc_integral { get; set; }

        public int total_integral { get; set; }


    }
}
