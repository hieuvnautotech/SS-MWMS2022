using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("w_box_mapping")]
    public class BoxMapping
    {
        [Key]
        public int bmno { get; set; }

        public string bx_no { get; set; }

        public string buyer_cd { get; set; }

        public string mt_cd { get; set; }

        public int gr_qty { get; set; }

        public string product { get; set; }

        public string type { get; set; }

        public string mapping_dt { get; set; }

        public string status { get; set; }

        public string use_yn { get; set; }

        public string del_yn { get; set; }

        public string reg_id { get; set; }

        public DateTime reg_dt { get; set; }

        public string chg_id { get; set; }

        public DateTime chg_dt { get; set; }

        public bool active { get; set; }


        //ngoại biến
        public int totalQty { get; set; }
        public string statusName { get; set; }
        public string ProductNo { get; set; }
        public int id { get; set; }
    }
}