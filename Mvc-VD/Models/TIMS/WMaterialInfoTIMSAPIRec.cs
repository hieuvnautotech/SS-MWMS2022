using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.TIMS
{
    public class WMaterialInfoTIMSAPIRec
    {
        public int stt { get; set; }
        public int id_actual { get; set; }
        public int staff_id { get; set; }
        public string staff_name { get; set; }
        public string start_real { get; set; }
        public string end_real { get; set; }
        public DateTime start_dt { get; set; }
        public DateTime end_dt { get; set; }
        public string shift_name { get; set; }
        public int? realQty { get; set; }
        public int? groupQty { get; set; }
        public int? defectQty { get; set; }


        [NotMapped]
        public string shift_dt
        {
            get
            {
                var result = ((shift_name == "Ca dem") && (start_dt.Hour <= 8))
                    ? start_dt.Date.AddDays(-1).ToString("yyyy-MM-dd")
                    : start_dt.Date.ToString("yyyy-MM-dd");

                return result;
            }
        }
    }
}