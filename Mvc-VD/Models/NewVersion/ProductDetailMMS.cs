using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("product_detail_mms")]
    public class ProductDetailMMS
    {
        [Key]
        [Column("id_product_lot")]
        public int ProductLotId { get; set; }
        [Column("po_no")]
        public string ProductNo { get; set; }
        [Column("product_code")]
        public string ProductCode { get; set; }
        [Column("id_actual")]
        public int ActualId { get; set; }
        [Column("actual_name")]
        public string ActualName { get; set; }
        [Column("staff_hist")]
        public string StaffId { get; set; }
        [Column("start_staff")]
        public DateTime StaffStart { get; set; }
        [Column("end_staff")]
        public DateTime StaffEnd { get; set; }
        [Column("start_machine")]
        public DateTime MachineStart { get; set; }
        [Column("end_machine")]
        public DateTime MachineEnd { get; set; }
        [Column("machine_hist")]
        public string MachineCode { get; set; }
        [Column("material_name")]
        public string MaterialName { get; set; }
        [Column("use_material_spec")]
        public string UseMaterialSpec { get; set; }
        [Column("material_size")]
        public string MaterialSize { get; set; }
        [Column("lot_no")]
        public string LotNo { get; set; }
        [Column("recei_date")]
        public DateTime ReceiDate { get; set; }
        [Column("expiry_date")]
        public DateTime ExpiryDate { get; set; }
        [Column("gr_qty")]
        public int GroupQuanity { get; set; }
        [Column("real_qty")]
        public int RealQuanity { get; set; }
        [Column("reg_date")]
        public DateTime CreateDate { get; set; }
        [Column("reg_id")]
        public string CreateId { get; set; }
        [Column("chg_date")]
        public DateTime ChangeDate { get; set; }
        [Column("chg_id")]
        public string ChangeId { get; set; }
        [Column("active")]
        public bool Active { get; set; }
    }
}