using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("w_dl_info")]
    public class DeliveryInfo
    {
       
        [Key]
        public int dlid { get; set; }

        public string dl_no { get; set; }

        public string dl_nm { get; set; }

        public string status { get; set; }

        public string work_dt { get; set; }

        //public DateTime work_dt { get; set; }

        public string lct_cd { get; set; }

        public string remark { get; set; }

        public string use_yn { get; set; }

        public string reg_id { get; set; }

        public DateTime reg_dt { get; set; }

        public string chg_id { get; set; }

        public DateTime chg_dt { get; set; }

        public bool active { get; set; }


        // Ngoại biến
        public string dl_no1 { get; set; }
    }
}