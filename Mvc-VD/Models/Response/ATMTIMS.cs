using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class ATMTIMS
    {
        public int waiting { get; set; }
        public string model { get; set; }
        public string unit_cd { get; set; }
        public string prj_nm { get; set; }
        public int waitold { get; set; }
        public string at_no { get; set; }
        public string product { get; set; }
        public string product_nm { get; set; }
        public string reg_dt { get; set; }
        public string min_day { get; set; }
        public string max_day { get; set; }
        public string remark { get; set; }
        public string SanLuongLyThuyet { get; set; }
        public string SoMetLyThuyet { get; set; }
        public string loss { get; set; }
        public string HieuSuatSanXuat { get; set; }
        public string OKSanXuat { get; set; }
        public string NgSanXuat { get; set; }
        public decimal? m_lieu { get; set; }
        public decimal? need_m { get; set; }
        public decimal? actual { get; set; }
        public int id_actual { get; set; }
        public decimal? HS { get; set; }
        public string mt_no { get; set; }
        public string mt_nm { get; set; }
        public string bom_type { get; set; }
        public decimal? actual_lt { get; set; }
        public int NG { get; set; }
        public int target { get; set; }
        public int count_day { get; set; }
    }
}