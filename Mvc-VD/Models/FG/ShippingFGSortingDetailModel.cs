using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.FG
{
    public class ShippingFGSortingDetailModel
    {
        public int id { get; set; }
        public string ShippingCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string buyer_qr { get; set; }
        public string location { get; set; }
        public string locationname { get; set; }
        public string countBuyer { get; set; }
        public string sumQuantity { get; set; }
        public string lot_no { get; set; }
        public string Quantity { get; set; }
        public string CreateId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string reg_date_convert { get { return this.CreateDate.ToString("yyyy-MM-dd HH:mm:dd"); } }
        public string ChangeId { get; set; }
        public string Model { get; set; }
        public System.DateTime ChangeDate { get; set; }
    }
}