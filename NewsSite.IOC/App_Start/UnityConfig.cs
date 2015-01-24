using Microsoft.Practices.Unity;
using NewsSite.Core.Database.Tables;
using NewsSite.Data.Repository;
using NewsSite.Data.UnitOfWork;
using NewsSite.Service.MembershipService;
using System.Web.Mvc;
using Unity.Mvc5;

namespace NewsSite.IOC
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            RegisterTypes(container);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            container.BindInRequestScope<IGenericRepository<User>, GenericRepository<User>>();
            container.BindInRequestScope<IGenericRepository<Role>, GenericRepository<Role>>();
            //container.BindInRequestScope<IGenericRepository<City>, GenericRepository<City>>();
            //container.BindInRequestScope<IGenericRepository<District>, GenericRepository<District>>();
            //container.BindInRequestScope<IGenericRepository<UserType>, GenericRepository<UserType>>();

            container.BindInRequestScope<IUnitOfWork, UnitOfWork>();

            container.BindInRequestScope<IMembershipService, MembershipService>();
            //container.BindInRequestScope<IRoleService, RoleService>();
            //container.BindInRequestScope<ILocationService, LocationService>();
        }
    }

    /// <summary>
    /// Bind the given interface in request scope
    /// </summary>
    public static class IOCExtensions
    {
        public static void BindInRequestScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            container.RegisterType<T1, T2>(new HierarchicalLifetimeManager());
        }

        public static void BindInSingletonScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            container.RegisterType<T1, T2>(new ContainerControlledLifetimeManager());
        }
    }
}