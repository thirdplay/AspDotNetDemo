using Prototype.Mvc.Html;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Prototype.Mvc.Html
{
    /// <summary>
    /// チェックボックス関連のHTML拡張機能を提供します。
    /// </summary>
    public static class CheckExtensions
    {
        /// <summary>
        /// 指定された式で表されるオブジェクトの各プロパティについて、チェック ボックスの input 要素および label 要素を返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型</typeparam>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式</param>
        /// <param name="inputHtmlAttributes">input 要素に設定する HTML 属性を格納するオブジェクト</param>
        /// <param name="labelHtmlAttributes">label 要素に設定する HTML 属性を格納するオブジェクト</param>
        /// <returns>指定された式で表されるオブジェクトの各プロパティで、type 属性が "checkbox" に設定されている HTML の input 要素</returns>
        public static MvcHtmlString CheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, object inputHtmlAttributes, object labelHtmlAttributes)
        {
            return CheckBoxHelper(
                htmlHelper,
                ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData),
                htmlHelper.CheckBoxFor(expression, inputHtmlAttributes),
                HtmlHelper.AnonymousObjectToHtmlAttributes(labelHtmlAttributes));
        }

        /// <summary>
        /// 指定された <see cref="ModelMetadata"/> の HTML radio 要素を返します。
        /// </summary>
        /// <param name="html">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="metadata">この要素を表すメタデータ</param>
        /// <param name="inputHtml">input要素のHTML</param>
        /// <param name="labelHtmlAttributes">label要素に設定する HTML 属性を格納するオブジェクト</param>
        /// <returns>HTML check 要素</returns>
        private static MvcHtmlString CheckBoxHelper(HtmlHelper html, ModelMetadata metadata, MvcHtmlString inputHtml, IDictionary<string, object> labelHtmlAttributes)
        {
            var result = new StringBuilder();
            var name = metadata.PropertyName;
            var displayName = metadata.DisplayName;
            var fullName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            var label = new TagBuilder("label");
            label.Attributes.Add("for", fullName);
            label.InnerHtml = inputHtml.ToString();
            if (labelHtmlAttributes.Count > 0)
            {
                label.MergeAttributes(labelHtmlAttributes, replaceExisting: true);
                label.InnerHtml += " " + displayName;
            }
            result.Append(label.ToString());

            return MvcHtmlString.Create(result.ToString());
        }
    }
}