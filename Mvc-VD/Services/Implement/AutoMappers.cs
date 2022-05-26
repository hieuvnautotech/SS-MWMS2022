using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Mvc_VD.Models;
using Mvc_VD.Models.WOModel;
using Mvc_VD.Services.ShinsungNew.Iservices;

namespace Mvc_VD.Services.ShinsungNew.Services
{
    public class AutoMappers : IautoMappers
    {
        public Mapper MapperActual()
        {
            var config = new MapperConfiguration(cfg=> {
                cfg.CreateMap<w_actual, Models.WOModel.w_actual_dto>()
                    .ForMember(a => a.IdActual, b => b.MapFrom(c => c.id_actual))
                    .ForMember(a => a.AtNo, b => b.MapFrom(c => c.at_no))
                    .ForMember(a => a.Type, b => b.MapFrom(c => c.type))
                    .ForMember(a => a.Product, b => b.MapFrom(c => c.product))
                    .ForMember(a => a.Actual, b => b.MapFrom(c => c.actual))
                    .ForMember(a => a.Defect, b => b.MapFrom(c => c.defect))
                    .ForMember(a => a.Name, b => b.MapFrom(c => c.name))
                    .ForMember(a => a.Date, b => b.MapFrom(c => c.date))
                    .ForMember(a => a.Level, b => b.MapFrom(c => c.level))
                    .ForMember(a => a.UnitPr, b => b.MapFrom(c => c.don_vi_pr))
                    .ForMember(a => a.ItemVcd, b => b.MapFrom(c => c.item_vcd))
                    .ForMember(a => a.CreateId, b => b.MapFrom(c => c.reg_id))
                    .ForMember(a => a.CreateDate, b => b.MapFrom(c => c.reg_dt))
                    .ForMember(a => a.ChangeId, b => b.MapFrom(c => c.chg_id))
                    .ForMember(a => a.ChangeDate, b => b.MapFrom(c => c.chg_dt));

                //cfg.CreateMap<w_actual_dto, w_actual>()
                //    .ForMember(a => a.id_actual, b => b.MapFrom(c => c.IdActual))
                //    .ForMember(a => a.at_no, b => b.MapFrom(c => c.AtNo))
                //    .ForMember(a => a.type, b => b.MapFrom(c => c.Type))
                //    .ForMember(a => a.product, b => b.MapFrom(c => c.Product))
                //    .ForMember(a => a.actual, b => b.MapFrom(c => c.Actual))
                //    .ForMember(a => a.defect, b => b.MapFrom(c => c.Defect))
                //    .ForMember(a => a.name, b => b.MapFrom(c => c.Name))
                //    .ForMember(a => a.date, b => b.MapFrom(c => c.Date))
                //    .ForMember(a => a.level, b => b.MapFrom(c => c.Level))
                //    .ForMember(a => a.don_vi_pr, b => b.MapFrom(c => c.UnitPr))
                //    .ForMember(a => a.item_vcd, b => b.MapFrom(c => c.ItemVcd))
                //    .ForMember(a => a.reg_id, b => b.MapFrom(c => c.CreateId))
                //    .ForMember(a => a.reg_dt, b => b.MapFrom(c => c.CreateDate))
                //    .ForMember(a => a.chg_id, b => b.MapFrom(c => c.ChangeId))
                //    .ForMember(a => a.chg_dt, b => b.MapFrom(c => c.ChangeDate));

            });
            return new Mapper(config);
        }
       
    }
}