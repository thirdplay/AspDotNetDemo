using Prototype.Mvc.Html;
using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Prototype.Mvc.Html
{
    /// <summary>
    /// リストボックス関連のHTML拡張機能を提供します。
    /// </summary>
    public static class ListBoxExtensions
    {
        /// <summary>
        /// 指定された式で表されるオブジェクトの各プロパティについて、指定されたリスト項目および HTML 属性を使用して HTML select 要素を返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型</typeparam>
        /// <typeparam name="TProperty">プロパティの型</typeparam>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式</param>
        /// <param name="htmlAttributes">この要素に設定する HTML 属性を格納するオブジェクト</param>
        /// <returns>式で表されるオブジェクト内の各プロパティの HTML select 要素</returns>
        public static MvcHtmlString ListBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return htmlHelper.ListBoxFor(expression, null, htmlAttributes);
        }
    }
}