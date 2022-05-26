using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("w_policy_mt")]
    public class PolicyMT
    {
        [Key]
        public int wid { get; set; }

        public string policy_code { get; set; }

        public string policy_name { get; set; }

        public DateTime policy_start_dt { get; set; }

        public DateTime policy_end_dt { get; set; }

        public string work_starttime { get; set; }

        public string work_endtime { get; set; }

        public string lunch_start_time { get; set; }

        public string lunch_end_time { get; set; }

        public string dinner_start_time { get; set; }

        public string dinner_end_time { get; set; }

        public decimal work_hour { get; set; }

        public string use_yn { get; set; }

        public string last_yn { get; set; }

        public string re_mark { get; set; }

        public string reg_id { get; set; }

        public DateTime reg_dt { get; set; }

        public string chg_id { get; set; }

        public DateTime chg_dt { get; set; }

        public bool active { get; set; }
    }
}