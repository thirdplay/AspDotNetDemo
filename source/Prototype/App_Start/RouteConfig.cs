using System.Web.Mvc;
using System.Web.Routing;

namespace Prototype
{
    /// <summary>
    /// ルート設定クラス。
    /// </summary>
    internal static class RouteConfig
    {
        /// <summary>
        /// ルートを設定します。
        /// </summary>
        /// <param name="routes">ルートコレクション</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Login", action = "Index" }
            );
        }
    }
}