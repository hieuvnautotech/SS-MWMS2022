using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.Response
{
    public class Getfacline_qc_PhanLoaiReponse
    {
        public int Id { get; set; }

        public string fq_no { get; set; }

        public string ml_tims { get; set; }

        public DateTime work_dt { get; set; }

        public int ok_qty { get; set; }

        public int defeacr_qty { get; set; }
    }
}