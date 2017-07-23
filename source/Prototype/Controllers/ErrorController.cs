using Microsoft.Practices.ServiceLocation;
using System.Net;
using System.Web.Mvc;

namespace Prototype.Controllers
{
	/// <summary>
	/// エラー画面を制御するコントローラー。
	/// </summary>
	public class ErrorController : ControllerBase
	{
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="serviceLocator">サービスロケーター</param>
		public ErrorController(IServiceLocator serviceLocator)
			: base(serviceLocator)
		{
		}

		/// <summary>
		/// 500 InternalServerError。
		/// </summary>
		/// <returns>アクション結果</returns>
		[HttpGet]
		public ActionResult Index()
		{
			Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			return View();
		}

		/// <summary>
		/// 404 NotFound。
		/// </summary>
		/// <returns>アクション結果</returns>
		[HttpGet]
		public ActionResult NotFound()
		{
			Response.StatusCode = (int)HttpStatusCode.NotFound;

			return View();
		}
	}
}