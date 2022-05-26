using Mvc_VD.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Mvc_VD.data
{
    public class ApplicationDbcontext : DbContext
    {
        public ApplicationDbcontext()
            : base("name=MSSQL")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

       
    }
}