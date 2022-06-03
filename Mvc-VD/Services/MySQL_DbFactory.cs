using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.SqlServer.Server;
using Mvc_VD.data;
using Mvc_VD.Models;
using Mvc_VD.Respositories.Irepository;
using Mvc_VD.Respositories.Repository;
using Mvc_VD.Services.Implement;
using Mvc_VD.Services.Implement.MMS;
using Mvc_VD.Services.Implement.QMS;
using Mvc_VD.Services.Interface;
using Mvc_VD.Services.Interface.MMS;
using Mvc_VD.Services.Interface.QMS;
using Mvc_VD.Services.ShinsungNew;
using Mvc_VD.Services.ShinsungNew.Iservices;
using Mvc_VD.Services.TIMS;
using System;
using System.Reflection;
using System.Web.Mvc;

namespace Mvc_VD.Services
{
    public class MySQL_DbFactory
    {
        public static void Builder()
        {
            SetAutofacContainer();
        }

        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();
            builder.RegisterType<WOService>().As<IWOService>().InstancePerRequest();
            builder.RegisterType<TestWoServiceNew>().As<ItestWoServiceNew>().InstancePerRequest();
            builder.RegisterType<DMSService>().As<IDMSService>().InstancePerRequest();
            builder.RegisterType<EntityService>().As<IEntityService>().InstancePerRequest();
            builder.RegisterType<TIMSService>().As<ITIMSService>().InstancePerRequest();
            builder.RegisterType<WorkRequestService>().As<IWorkRequestService>().InstancePerRequest();  
            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();
           // builder.RegisterType<HomeService>().As<IHomeService>().InstancePerRequest();
            builder.RegisterType<WIPService>().As<IWIPService>().InstancePerRequest();
            builder.RegisterType<WMSService>().As<IWMSService>().InstancePerRequest();
           
            builder.RegisterType<DapperDbConnectionFactory>().As<IDbConnectionFactory>().InstancePerRequest();
            builder.RegisterType<DevManagementService>().As<IdevManagementService>().InstancePerRequest();
            builder.RegisterType<HomeServices>().As<IhomeService>().InstancePerRequest();
            builder.RegisterType<CommonService>().As<IcommonService>().InstancePerRequest();
            builder.RegisterType<WMSServices>().As<IWMSServices>().InstancePerRequest();
            builder.RegisterType<TimsServices>().As<ITimsService>().InstancePerRequest();
            builder.RegisterType<UserServices>().As<IUserServices>().InstancePerRequest();
            builder.RegisterType<CreateBuyerQRService>().As<ICreateBuyerQRService>().InstancePerRequest();


            builder.RegisterType<ActualWOService>().As<IActualWOService>().InstancePerRequest();
            builder.RegisterType<WIPServices>().As<IWIPServices>().InstancePerRequest();
            builder.RegisterType<StandardManagementWO>().As<IStandardManagementWO>().InstancePerRequest();
            builder.RegisterType<SupplierQRServices>().As<ISupplierQRServiecs>().InstancePerRequest();
            builder.RegisterType<StandardServices>().As<IStandardServices>().InstancePerRequest();
            builder.RegisterType<MenuServices>().As<IMenuServices>().InstancePerRequest();
            builder.RegisterType<QMSService>().As<IQMSService>().InstancePerRequest();

            builder.RegisterType<FGMWServices>().As<IFGMWServices>().InstancePerRequest();
            builder.RegisterType<HieuCommonService>().As<IHieuCommonServices>().InstancePerRequest();

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
    public interface IDbFactory : IDisposable
    {
        Entities Init();
        //ApplicationDbcontext Getmssqlcontext();
    }

    public class DbFactory : Disposable, IDbFactory
    {
        private Entities dbContext;
        //private ApplicationDbcontext mssqlcontext;
        public Entities Init()
        {
            return dbContext ?? (dbContext = new Entities());
        }
        //public ApplicationDbcontext Getmssqlcontext()
        //{
        //    return mssqlcontext ?? (mssqlcontext = new ApplicationDbcontext());
        //}

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
            //if (mssqlcontext != null)
            //    mssqlcontext.Dispose();
        }
    }
    public class Disposable : IDisposable
    {
        private bool isDisposed;

        ~Disposable()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (!isDisposed && disposing)
            {
                DisposeCore();
            }

            isDisposed = true;
        }

        // Ovveride this to dispose custom objects
        protected virtual void DisposeCore()
        {
        }
    }
}

