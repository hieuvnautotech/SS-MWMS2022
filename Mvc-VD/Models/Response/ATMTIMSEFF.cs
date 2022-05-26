using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class ATMTIMSEFF
    {
        public string remark { get; set; }
        public string at_no { get; set; }
        public string product { get; set; }
        public string product_nm { get; set; }
        public int SanLuongLyThuyet { get; set; }
        public string SoMetLyThuyet { get; set; }
        public string mt_no { get; set; }
        public string loss { get; set; }
        public string HieuSuatSanXuat { get; set; }
        public string OKSanXuat { get; set; }
        public string NgSanXuat { get; set; }
        public string HieusuatOQC { get; set; }
        public string OkThanhPham { get; set; }
        public string NGThanhPham { get; set; }
        public string HangChoKiem { get; set; }
        public string bom_type { get; set; }
     
    }
}