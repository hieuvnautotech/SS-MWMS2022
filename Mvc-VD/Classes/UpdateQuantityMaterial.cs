using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Classes
{
    public class UpdateQuantityMaterial
    {
        public List<string> listId { get; set; }
        public int? gr_qty { get; set; }
        public string lot_mt_no { get; set; }
    }
}