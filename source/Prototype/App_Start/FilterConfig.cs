using Prototype.Mvc.Filters;
using System.Web.Mvc;

namespace Prototype
{
    /// <summary>
    /// フィルター設定クラス。
    /// </summary>
    internal static class FilterConfig
    {
        /// <summary>
        /// グローバルフィルターを登録します。
        /// </summary>
        /// <param name="filters">グローバルフィルターコレクション</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GlobalHandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
            filters.Add(new ElapsedTimeAttribute());
        }
    }
}