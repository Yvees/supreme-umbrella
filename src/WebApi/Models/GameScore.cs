using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class GameScore
    {
        [Key]
        public int id { get; set; }

        public string oid { get; set; }

        public string pid { get; set; }

        public string gid { get; set; }

        public int score { get; set; }

        public int time { get; set; }
    }
}
