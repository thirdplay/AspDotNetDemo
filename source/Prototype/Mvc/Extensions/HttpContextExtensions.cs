using System.Web;
using System.Web.Routing;

namespace Prototype.Mvc.Extensions
{
	/// <summary>
	/// HTTPコンテキストの拡張機能を拡張します。
	/// </summary>
	public static class HttpContextExtensions
	{
		/// <summary>
		/// コントローラー名を取得します。
		/// </summary>
		/// <param name="httpContext">HTTPコンテキスト</param>
		/// <returns>コントローラー名</returns>
		public static string GetControllerName(this HttpContextBase httpContext)
		{
			var routeDate = RouteTable.Routes.GetRouteData(httpContext);
			return routeDate.Values["controller"].ToString();
		}

		/// <summary>
		/// 画面IDを取得します。
		/// </summary>
		/// <param name="httpContext">HTTPコンテキスト</param>
		/// <returns>画面ID</returns>
		public static string GetScreenId(this HttpContextBase httpContext)
		{
			return httpContext.GetControllerName().ToUpper();
		}
	}
}