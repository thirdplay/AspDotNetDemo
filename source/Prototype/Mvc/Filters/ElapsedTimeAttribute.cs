using NLog;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace Prototype.Mvc.Filters
{
    /// <summary>
    /// 経過時間を測定する属性を表します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ElapsedTimeAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// アクション用のストップウォッチ
        /// </summary>
        private readonly Stopwatch actionWatch = null;

        /// <summary>
        /// アクション結果用のストップウォッチ
        /// </summary>
        private readonly Stopwatch resultWatch = null;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public ElapsedTimeAttribute()
        {
            this.actionWatch = new Stopwatch();
            this.resultWatch = new Stopwatch();
        }

        /// <summary>
        /// アクションメソッド実行前に呼び出されます。
        /// </summary>
        /// <param name="filterContext">フィルターコンテキスト</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            this.actionWatch.Restart();
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// アクションメソッド実行後に呼び出されます。
        /// </summary>
        /// <param name="filterContext">フィルターコンテキスト</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            this.actionWatch.Stop();

            if (!filterContext.Canceled && filterContext.Exception == null)
            {
                return;
            }

            var message = GetElapsedTimeResult(
                filterContext.HttpContext.Request.Url,
                filterContext.HttpContext.Request.HttpMethod,
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                filterContext.ActionDescriptor.ActionName,
                (HttpStatusCode)filterContext.HttpContext.Response.StatusCode,
                this.actionWatch.Elapsed,
                null
            );
            logger.Info(message);
        }

        /// <summary>
        /// アクション結果実行前に呼び出されます
        /// </summary>
        /// <param name="filterContext">フィルターコンテキスト</param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            this.resultWatch.Restart();
            base.OnResultExecuting(filterContext);
        }

        /// <summary>
        /// アクション結果実行後に呼び出されます
        /// </summary>
        /// <param name="filterContext">フィルターコンテキスト</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            this.resultWatch.Stop();
            var message = GetElapsedTimeResult(
                filterContext.HttpContext.Request.Url,
                filterContext.HttpContext.Request.HttpMethod,
                filterContext.RouteData.Values["controller"].ToString(),
                filterContext.RouteData.Values["action"].ToString(),
                (HttpStatusCode)filterContext.HttpContext.Response.StatusCode,
                this.actionWatch.Elapsed,
                this.resultWatch.Elapsed
            );
            logger.Info(message);
        }

        /// <summary>
        /// 経過時間の結果文字列を取得します。
        /// </summary>
        /// <param name="uri">要求URI</param>
        /// <param name="method">HTTPメソッド</param>
        /// <param name="controller">コントローラー名</param>
        /// <param name="action">アクション名</param>
        /// <param name="statusCode">HTTPステータスコード</param>
        /// <param name="actionElapsed">アクション時間</param>
        /// <param name="resultElapsed">アクション結果時間</param>
        private static string GetElapsedTimeResult(Uri uri, string method, string controller, string action, HttpStatusCode statusCode, TimeSpan actionElapsed, TimeSpan? resultElapsed)
        {
            var totalElapsed = resultElapsed.HasValue ? actionElapsed + resultElapsed.Value : actionElapsed;
            var builder = new StringBuilder();
            builder.AppendLine("経過時間");
            builder.AppendLine("----------------------------------------");
            builder.AppendFormat($"Request Uri      : {uri}\n");
            builder.AppendFormat($"Http Method      : {method}\n");
            builder.AppendFormat($"Controller       : {controller}\n");
            builder.AppendFormat($"Action           : {action}\n");
            builder.AppendFormat($"Status Code      : {statusCode}\n");
            builder.AppendFormat($"Elapsed [Action] : {actionElapsed}\n");
            builder.AppendFormat($"Elapsed [Result] : {resultElapsed}\n");
            builder.AppendFormat($"Elapsed [Total]  : {totalElapsed}\n");
            builder.Append("----------------------------------------");
            return builder.ToString();
        }
    }
}