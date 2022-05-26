using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    public class InventoryProduct
    {
        public int materialid { get; set; }
        public int id_actual { get; set; }
        public string material_code { get; set; }
        public DateTime? recei_wip_date { get; set; }
        public DateTime? picking_date { get; set; }
        public string sd_no { get; set; }
        public string ex_no { get; set; }
        public string lct_sts_cd { get; set; }
        public string mt_no { get; set; }
        public string mt_type { get; set; }
        public double gr_qty { get; set; }
        public double real_qty { get; set; }
        public string bb_no { get; set; }
        public string orgin_mt_cd { get; set; }
        public DateTime? recei_date { get; set; }
        public DateTime? expiry_date { get; set; }
        public DateTime? export_date { get; set; }
        public DateTime? date_of_receipt { get; set; }
        public string lot_no { get; set; }
        public string location_code { get; set; }
        public string from_lct_cd { get; set; }
        public string location_number { get; set; }
        public string status { get; set; }
        public string create_id { get; set; }
        public DateTime create_date { get; set; }
        public string change_id { get; set; }
        public DateTime change_date { get; set; }
        public string ExportCode { get; set; }
        public string LoctionMachine { get; set; }
        public DateTime? ShippingToMachineDatetime { get; set; }
        public bool active { get; set; }

        //Ngoại Biến 
        public string locationName { get; set; }
        public string mt_cd { get; set; }
    }
}