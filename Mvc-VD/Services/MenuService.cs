using Mvc_VD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.Services
{
    public interface IMenuSEvervice
    {

    }
    public class MenuService: IMenuSEvervice
    {
        private Entities _db;
        public MenuService(IDbFactory dbFactory)
        {
            _db = dbFactory.Init();
        }
       
    }
}