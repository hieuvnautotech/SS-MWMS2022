using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    public class MaterialInfoMMS
    {
        public int wmtid { get; set; }

        public int id_actual { get; set; }
        
        public string material_code { get; set; }

        public string material_type { get; set; }

        public int gr_qty { get; set; }

        public int real_qty { get; set; }

        public int number_divide { get; set; }

        public string status { get; set; }

        public string mt_no { get; set; }

        public string bb_no { get; set; }

        public string location_code { get; set; }

        public string location_number { get; set; }

        public string from_lct_code { get; set; }

        public string to_lct_code { get; set; }

        public string receipt_date { get; set; }

        public string reg_id { get; set; }
      
        public DateTime reg_date { get; set; }

        public string chg_id { get; set; }

        public DateTime chg_date { get; set; }

        public string bbmp_sts_cd { get; set; }

        public string orgin_mt_cd { get; set; }

        public int id_actual_oqc { get; set; }

        public string sd_no { get; set; }

        public string sts_update { get; set; }

        public DateTime? ShippingToMachineDatetime { get; set; }

        public bool active { get; set; }

        // ngoại biến
        public string ExportCode { get; set; }

        public string LoctionMachine { get; set; }
    }
}