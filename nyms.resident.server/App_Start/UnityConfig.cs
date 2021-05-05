using nyms.resident.server.DataProviders.Impl;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Invoice;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Impl;
using nyms.resident.server.Services.Interfaces;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http.Headers;
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
            container.RegisterType<IInvoiceDataProvider, InvoiceDataProvider>(new InjectionConstructor(connectionString));
            container.RegisterType<IInvoiceService, InvoiceService>();
            container.RegisterType<IResidentContactDataProvider, ResidentContactDataProvider>(new InjectionConstructor(connectionString));
            container.RegisterType<ISocialWorkerDataProvider, SocialWorkerDataProvider>(new InjectionConstructor(connectionString));
            // requires additional parameters
            container.RegisterType<IResidentDataProvider, ResidentDataProvider>(new InjectionConstructor(connectionString,
                typeof(IResidentContactDataProvider), typeof(ISocialWorkerDataProvider)));
            container.RegisterType<IResidentService, ResidentService>();
            container.RegisterType<IFeeCalculatorService, FeeCalculatorService>();
            
            container.RegisterType<IPaymentProviderDataProvider, PaymentProviderDataProvider>(new InjectionConstructor(connectionString));
            container.RegisterType<IPaymentTypeDataProvider, PaymentTypeDataProvider>(new InjectionConstructor(connectionString));
            container.RegisterType<IBillingCycleDataProvider, BillingCycleDataProvider>(new InjectionConstructor(connectionString));

            container.RegisterType<IScheduleDataProvider, ScheduleDataProvider>(new InjectionConstructor(connectionString));
            container.RegisterType<IScheduleService, ScheduleService>();
            container.RegisterType<IDatabaseSetupProvider, DatabaseSetupProvider>(new InjectionConstructor(connectionString));
            container.RegisterType<IReportService, ReportService>();

            container.RegisterType<ISpendCategoriesDataProvider, SpendCategoriesDataProvider>(new InjectionConstructor(connectionString));
            container.RegisterType<ISpendCategoriesService, SpendCategoriesService>();
            container.RegisterType<ISpendBudgetDataProvider, SpendBudgetDataProvider>(new InjectionConstructor(connectionString));
            container.RegisterType<ISpendBudgetService, SpendBudgetService>();
            container.RegisterType<ISecurityDataProvider, SecurityDataProvider>(new InjectionConstructor(connectionString));
            container.RegisterType<ISecurityService, SecurityService>();

            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));

            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

        }
    }
}