using nyms.resident.server.DataProviders.Impl;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Impl;
using nyms.resident.server.Services.Interfaces;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
// using Unity.Mvc5;

namespace nyms.resident.server
{
    public static class UnityConfig
    {
        public static void RegisterComponents(string connectionString)
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            container.RegisterSingleton<IJwtService, JwtService>();
            container.RegisterType<IAuthenticationService, AuthenticationService>();
            container.RegisterType<IUserDataProvider, UserDataProvider>(new InjectionConstructor(connectionString));
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IEnquiryDataProvider, EnquiryDataProvider>(new InjectionConstructor(connectionString));
            container.RegisterType<IEnquiryService, EnquiryService>();

            container.RegisterType<ICareHomeDataProvider, CareHomeDataProvider>(new InjectionConstructor(connectionString));
            container.RegisterType<ICareHomeService, CareHomeService>();
            
            
            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));

            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

        }
    }
}