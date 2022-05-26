using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Classes
{
    public class StaffModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string LocationCode { get; set; }
        public string BarCode { get; set; }
        public string PositionName { get; set; }
        public string PositionCode { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? JoinDate { get; set; }
    }
}