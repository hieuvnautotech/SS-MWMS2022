using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("m_board")]
    public class MBoard
    {
        [Key]
        [Column("mno")]
        public string MNo { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("viewcnt")]
        public int ViewCnt { get; set; }
        [Column("replycnt")]
        public int ReplyCnt { get; set; }
        [Column("div_cd")]
        public string DivCode { get; set; }
        [Column("start_dt")]
        public DateTime StartDate { get; set; }
        [Column("end_dt")]
        public DateTime EndDate { get; set; }
        [Column("widthsize")]
        public int WidthSize { get; set; }
        [Column("heightsize")]
        public int HeightSize { get; set; }
        [Column("back_color")]
        public string BackColor { get; set; }
        [Column("order_no")]
        public int OrderNo { get; set; }
        [Column("del_yn")]
        public string DeleteYn { get; set; }
        [Column("reg_id")]
        public string CreateId { get; set; }
        [Column("reg_dt")]
        public DateTime CreateDate { get; set; }
        [Column("chg_id")]
        public string ChangeId { get; set; }

        [Column("chg_dt")]
        public DateTime ChangeDate { get; set; }
        [Column("active")]
        public bool Active { get; set; }
    }
}