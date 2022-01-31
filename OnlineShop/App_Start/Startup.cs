using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.Owin;
using Model.Dao;
using Model.EF;
using Owin;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(OnlineShop.App_Start.Startup))]

namespace OnlineShop.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigAutofac(app);
        }

        //install Microsoft.Owin.host.systemWeb 
        public void ConfigAutofac(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<ShopOnlineDbContext>().AsSelf().InstancePerRequest();

            builder.RegisterType<ProductDao>().As<IProduct>().InstancePerRequest();


            Autofac.IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));//web mvc5
            //wab api

       }
    }
}
