using System;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace Prototype.Mvc.Html
{
    /// <summary>
    /// 入力検証の拡張機能を提供します。
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// メッセージ領域のHTMLマークアップを返します。
        /// </summary>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <returns>メッセージ領域を表す div 要素</returns>
        public static MvcHtmlString MessageArea(this HtmlHelper htmlHelper)
        {
            return MessageAreaHelper(htmlHelper, "messageArea");
        }

        /// <summary>
        /// モーダル用のメッセージ領域のHTMLマークアップを返します。
        /// </summary>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <returns>メッセージ領域を表す div 要素</returns>
        public static MvcHtmlString ModalMessageArea(this HtmlHelper htmlHelper)
        {
            return MessageAreaHelper(htmlHelper, "modalMessageArea");
        }

        /// <summary>
        /// 指定された式で表される各データ フィールドについて、検証エラーメッセージの HTML マークアップを返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型</typeparam>
        /// <typeparam name="TProperty">プロパティの型</typeparam>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式</param>
        /// <returns>エラー メッセージを含む span 要素。</returns>
        public static MvcHtmlString ValidationFieldFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.ValidationFieldHelper(
                ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData),
                ExpressionHelper.GetExpressionText(expression)
            );
        }

        /// <summary>
        /// 指定された式で表される各データ フィールドについて、検証エラーメッセージの HTML マークアップを返します。
        /// </summary>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="modelMetadata">モデルのメタデータ</param>
        /// <param name="expression">ラムダ式のモデル名</param>
        /// <returns>エラー メッセージを含む span 要素。</returns>
        private static MvcHtmlString ValidationFieldHelper(this HtmlHelper htmlHelper, ModelMetadata modelMetadata, string expression)
        {
            var result = new StringBuilder();
            var modelName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expression);

            var tagBuilder = new TagBuilder("span");
            tagBuilder.AddCssClass("field-validation-valid text-danger");
            tagBuilder.MergeAttribute("data-valmsg-for", modelName);
            result.Append(tagBuilder.ToString());

            return MvcHtmlString.Create(result.ToString());
        }

        /// <summary>
        /// メッセージ領域のHTMLマークアップを返します。
        /// </summary>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="id">メッセージ領域のID</param>
        /// <returns>メッセージ領域を表す div 要素</returns>
        private static MvcHtmlString MessageAreaHelper(HtmlHelper htmlHelper, string id)
        {
            var result = new StringBuilder();

            var tagBuilder = new TagBuilder("div");
            tagBuilder.MergeAttribute("id", id);
            tagBuilder.MergeAttribute("style", "display:none;");
            tagBuilder.AddCssClass("alert alert-dismissible alert-danger");
            result.Append(tagBuilder.ToString());

            return MvcHtmlString.Create(result.ToString());
        }
    }
}