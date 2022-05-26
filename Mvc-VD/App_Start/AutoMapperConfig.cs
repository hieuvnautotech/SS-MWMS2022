using AutoMapper;
using Mvc_VD.Models;
using Mvc_VD.Models.NewVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_VD.App_Start
{
	public static class AutoMapperConfig
	{
		public static IMapper Mapper { get; set; }
		public static void Init()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<comm_dt, CommCode>()
                .ReverseMap();
            });
            Mapper = config.CreateMapper();
        }
	}
}