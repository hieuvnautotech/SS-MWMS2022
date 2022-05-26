using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("w_material_info_tims")]
    public class MaterialInfoTIMS
    {
        [Key]
        public int wmtid { get; set; }

        public int id_actual { get; set; }

        public int id_actual_oqc { get; set; }
        public string  staff_id_oqc { get; set; }
        public string  at_no { get; set; }
        public string product { get; set; }
        public string mt_sts_cd { get; set; }
        public string bbmp_sts_cd { get; set; }
        public string buyer_qr { get; set; }
        public string mt_cd { get; set; }

        public string staff_id { get; set; }

        public string material_code { get; set; }

        public string material_type { get; set; }
        //public string staff_id { get; set; }

        public int gr_qty { get; set; }
        public int alert_NG { get; set; }

        public int real_qty { get; set; }
        public int number_divide { get; set; }

        public string status { get; set; }

        public string sts_update { get; set; }

        public string mt_no { get; set; }

        public string bb_no { get; set; }
        public string box_no { get; set; }
        public string sts_nm { get; set; }

        public string location_code { get; set; }

        public string location_number { get; set; }

        public string from_lct_code { get; set; }

        public string orgin_mt_cd { get; set; }

        public string to_lct_code { get; set; }

        public string receipt_date { get; set; }
        
        public DateTime end_production_dt { get; set; }
        public string input_dt { get; set; }

        public string buyer_code { get; set; }

        public string reg_id { get; set; }

        public DateTime reg_date { get; set; }

        public string shippingDt { get; set; }

        public string chg_id { get; set; }

        public DateTime chg_date { get; set; }

        public bool active { get; set; }

        public string lot_no { get; set; }
    }
}