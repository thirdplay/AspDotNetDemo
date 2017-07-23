using Prototype.Factories;
using Prototype.Mvc.DI;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Prototype
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    internal class UnityConfig
    {
        #region Unity Container

        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        #endregion Unity Container

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or API controllers
        /// (unless you want to change the defaults), as Unity allows resolving a concrete type even
        /// if it was not previously registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a
            //       Microsoft.Practices.Unity.Configuration to the using statements. container.LoadConfiguration();
            // TODO: Register your types here container.RegisterType<IProductRepository, ProductRepository>();

            // DbConnection
            container.RegisterType<DbConnection>(
                new PerRequestLifetimeManager(),
                new InjectionFactory(_ =>
                {
                    return DbConnectionFactory.Create();
                })
            );

            // コンポーネント属性の型を登録
            RegisterTypesOfComponent(container);

            // ServiceLocatorの作成、登録
            IServiceLocator serviceLocator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }

        /// <summary>
        /// Unityコンテナにコンポーネント属性の型を登録します。
        /// </summary>
        /// <param name="container">Unityコンテナ</param>
        private static void RegisterTypesOfComponent(IUnityContainer container)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => x.IsInterface && x.GetCustomAttribute(typeof(ComponentAttribute)) != null);
            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute(typeof(ComponentAttribute)) as ComponentAttribute;
                container.RegisterType(type, attr.TargetType, attr.Lifetime.CreateLifetimeManager());
            }
        }
    }
}