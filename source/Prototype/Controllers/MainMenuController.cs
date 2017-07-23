using Microsoft.Practices.ServiceLocation;
using System.Web.Mvc;

namespace Prototype.Controllers
{
    /// <summary>
    /// メインメニュー画面のコントローラー。
    /// </summary>
    public class MainMenuController : ControllerBase
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="serviceLocator">サービスロケーター</param>
        public MainMenuController(IServiceLocator serviceLocator)
            : base(serviceLocator)
        {
        }

        /// <summary>
        /// 初期表示。
        /// </summary>
        /// <returns>アクション結果</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}