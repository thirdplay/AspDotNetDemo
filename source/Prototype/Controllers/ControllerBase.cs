using Prototype.Constants;
using Prototype.Mvc.Extensions;
using Prototype.Resources;
using Prototype.ViewModels;
using Microsoft.Practices.ServiceLocation;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Prototype.Controllers
{
    /// <summary>
    /// コントローラーの基底クラス。
    /// </summary>
    public abstract class ControllerBase : Controller
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="serviceLocator">サービスロケーター</param>
        protected ControllerBase(IServiceLocator serviceLocator)
            : base()
        {
        }

        /// <summary>
        /// 既定値のViewModelを取得します。
        /// </summary>
        /// <returns>ViewModel</returns>
        public virtual ViewModelBase GetDefaultViewModel()
        {
            return null;
        }

        /// <summary>
        /// 1ページの表示件数の既定値を取得します。
        /// </summary>
        /// <returns>1ページの表示件数</returns>
        public virtual int GetDefaultPageSize()
        {
            return 0;
        }

        /// <summary>
        /// アクション メソッドの呼び出し前に呼び出されます。
        /// </summary>
        /// <param name="filterContext">現在の要求およびアクションに関する情報</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Request.IsAjaxRequest() && !ModelState.IsValid)
            {
                // メッセージ取得
                string message = null;
                if (ModelState.ContainsKey("Message"))
                {
                    message = string.Join(@"\n",
                        ModelState["Message"].Errors
                        .Select(x => x.ErrorMessage));
                }

                // エラー時のJSON結果の作成
                filterContext.Result = CreateErrorResult(message);
            }
        }

        /// <summary>
        /// 成功時のJSON結果を作成します。
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="jsonRequestBehavior">クライアントからの HTTP GET 要求を許可するかどうかを示す値</param>
        /// <returns>JSON結果</returns>
        protected JsonResult CreateSuccessResult(
            [Optional, DefaultParameterValue("")] string fileName,
            [Optional, DefaultParameterValue(JsonRequestBehavior.DenyGet)] JsonRequestBehavior jsonRequestBehavior)
        {
            var res = new JsonResult()
            {
                Data = new
                {
                    Result = true,
                    FileName = fileName,
                },
                JsonRequestBehavior = jsonRequestBehavior
            };
            return res;
        }

        /// <summary>
        /// エラー時のJSON結果を作成します。
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        /// <returns>JSON結果</returns>
        protected JsonResult CreateErrorResult(string message = null)
        {
            message = message ?? Messages.MS011;
            return new JsonResult()
            {
                Data = new
                {
                    Result = false,
                    Message = message,
                    Errors = ModelState.GetErrors()
                }
            };
        }
    }
}