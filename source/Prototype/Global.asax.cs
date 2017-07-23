using Prototype.Constants;
using Prototype.Mvc.Environment;
using Prototype.Mvc.Profilers;
using Newtonsoft.Json;
using NLog;
using System;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Prototype
{
    /// <summary>
    /// MVCアプリケーションクラス。
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// スレッドプール監視
        /// </summary>
        private ThreadPoolMonitor threadPoolMonitor;

        /// <summary>
        /// アプリケーションの開始処理。
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            LoggerConfig.RegisterLoggers();
            ValidationConfig.RegisterAdapter();
            DapperConfig.RegisterMappings();
            AutoMapperConfig.RegisterMappings();

            // ビューエンジンをRazorViewEngineのみを有効化
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            // MVC応答ヘッダを無効化
            MvcHandler.DisableMvcResponseHeader = true;

            // CSRFトークンのクッキー名を変更
            AntiForgeryConfig.CookieName = "token";

            // ローカル環境以外の場合
            if (AppEnvironment.EnvironmentCode != EnvironmentCode.Local)
            {
                // スレッドプール監視開始
                threadPoolMonitor = new ThreadPoolMonitor();
            }
        }

        /// <summary>
        /// アプリケーションの終了処理。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        protected void Application_End(object sender, EventArgs e)
        {
            // スレッドプール監視停止
            threadPoolMonitor?.Stop();
        }

        /// <summary>
        /// アプリケーションの例外イベント。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            if (Server != null)
            {
                var ex = Server.GetLastError();
                if (ex != null)
                {
                    // AJAX要求の場合、JSONで結果を返す
                    var httpContext = new HttpRequestWrapper(Request);
                    if (httpContext.IsAjaxRequest())
                    {
                        Server.ClearError();
                        Context.Response.StatusCode = (int)HttpStatusCode.OK;
                        Context.Response.Clear();
                        Context.Response.ContentType = "application/json; charset=utf-8";
                        Context.Response.Write(JsonConvert.SerializeObject(new { Result = false, Message = ex.Message }));
                        Context.Response.End();
                    }

                    if (ex is HttpException && ((HttpException)ex).GetHttpCode() == (int)HttpStatusCode.NotFound)
                    {
                        // NotFoundを相手にするとログが大変になるので無視
                        return;
                    }

                    /*
                     * CustomErrorが無効な場合は、Controller内でおきた例外が二重にログ出力されてしまうことに注意。
                     * CustomErrorが有効な場合は、Controller外でおきた例外のみここでログ出力される。
                     */
                    logger.Error(ex);
                }
            }
        }
    }
}