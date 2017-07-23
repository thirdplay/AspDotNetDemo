using Prototype.Mvc.Extensions;
using Prototype.ViewModels;
using Microsoft.Practices.ServiceLocation;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Security;

namespace Prototype.Controllers
{
    /// <summary>
    /// ログイン画面のコントローラー。
    /// </summary>
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="serviceLocator">サービスロケーター</param>
        public LoginController(IServiceLocator serviceLocator)
            : base(serviceLocator)
        {
        }

        /// <summary>
        /// 初期表示アクション。
        /// </summary>
        /// <param name="model">ログイン画面のViewModel</param>
        /// <returns>アクション結果</returns>
        [HttpGet]
        public ActionResult Index(LoginViewModel model)
        {
#if DEBUG
            try
            {
                // デバッグ時はホスト名を初期値に入れる
                var hostName = System.Net.Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).HostName;
                model.UserId = hostName;
            }
            catch
            {
            }
#endif
            return View(model);
        }

        /// <summary>
        /// ログイン。
        /// </summary>
        /// <param name="model">ログイン画面のViewModel</param>
        /// <returns>アクション結果</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            // ログイン処理
            FormsAuthentication.SetAuthCookie(model.UserId, false);
            HttpContext.Session.SetUserId(model.UserId);

            return CreateSuccessResult();
        }

        /// <summary>
        /// ログアウト。
        /// </summary>
        /// <returns>アクション結果</returns>
        [HttpGet]
        public ActionResult Logout()
        {
            // ログアウト処理
            FormsAuthentication.SignOut();
            HttpContext.Session.RemoveAll();

            return RedirectToAction("Index");
        }
    }
}