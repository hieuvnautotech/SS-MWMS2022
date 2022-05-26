using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("d_material_info")]
    public class MaterialInfo
    {
        [Key]
        public int mtid { get; set; }

        public string mt_type { get; set; }
      
        public string mt_no { get; set; }
       
        public string mt_cd { get; set; }
      
        public string mt_no_origin { get; set; }
     
        public string mt_nm { get; set; }
     
        public string mf_cd { get; set; }
      
        public int gr_qty { get; set; }
        
        public string unit_cd { get; set; }
       
        public int bundle_qty { get; set; }
      
        public string bundle_unit { get; set; }
     
        public string sp_cd { get; set; }
       
        public string s_lot_no { get; set; }

        public string item_vcd { get; set; }
     
        public string qc_range_cd { get; set; }
     
        public string width { get; set; }

        public string del_yn { get; set; }

        public string width_unit { get; set; }
       
        public string spec { get; set; }
     
        public string spec_unit { get; set; }
       
        public string area { get; set; }
     
        public string area_unit { get; set; }
     
        public string thick { get; set; }
        
        public string thick_unit { get; set; }
      
        public string stick { get; set; }
       
        public string stick_unit { get; set; }

        public string consum_yn { get; set; }

        public string price { get; set; }
     
        public string tot_price { get; set; }
       
        public string price_unit { get; set; }
       
        public string price_least_unit { get; set; }
      
        public string photo_file { get; set; }
      
        public string re_mark { get; set; }
       
        public string use_yn { get; set; }
       
        public string barcode { get; set; }
    
        public string reg_id { get; set; }
      
        public DateTime reg_dt { get; set; }
        
        public string chg_id { get; set; }
      
        public DateTime chg_dt { get; set; }

        public string ExportCode { get; set; }

        

        /// <summary>
        /// Biến ngoại lai
        /// </summary>
        
        public int RowNum { get; set; }

        public string mt_type_nm { get; set; }

        public string new_price { get; set; }

        public string new_spec { get; set; }

        public string new_width { get; set; }

        public string area_all { get; set; }

        public string tot_price_new { get; set; }

        public string stick_new { get; set; }

        public string thick_new { get; set; }

        public string item_nm { get; set; }

        public string qc_range_cd_nm { get; set; }

        public string bundle_unit_nm { get; set; }

        public string consumable { get; set; }


      
    }
}