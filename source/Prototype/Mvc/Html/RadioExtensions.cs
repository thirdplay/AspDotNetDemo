using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Prototype.Mvc.Html
{
    /// <summary>
    /// ラジオボタン関連のHTML拡張機能を提供します。
    /// </summary>
    public static class RadioExtensions
    {
        /// <summary>
        /// 指定された式で表される列挙のそれぞれの値の HTML radio 要素を返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型</typeparam>
        /// <typeparam name="TEnum">値の型</typeparam>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="expression">表示する値を含むオブジェクトを特定する式</param>
        /// <returns>式で表される列挙のそれぞれの値の HTML radio 要素</returns>
        public static MvcHtmlString EnumRadioButtonFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
        {
            return htmlHelper.EnumRadioButtonFor(expression);
        }

        /// <summary>
        /// 指定された式で表される列挙のそれぞれの値の HTML radio 要素を返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型</typeparam>
        /// <typeparam name="TEnum">値の型</typeparam>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="expression">表示する値を含むオブジェクトを特定する式</param>
        /// <param name="labelHtmlAttributes">ラベル要素に設定する HTML 属性を格納するオブジェクト</param>
        /// <returns>式で表される列挙のそれぞれの値の HTML radio 要素</returns>
        public static MvcHtmlString EnumRadioButtonFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object labelHtmlAttributes)
        {
            return RadioButtonHelper(
                htmlHelper,
                ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData),
                null,
                HtmlHelper.AnonymousObjectToHtmlAttributes(labelHtmlAttributes));
        }

        /// <summary>
        /// 指定された式で表される列挙のうち、指定された値の HTML radio 要素を返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型</typeparam>
        /// <typeparam name="TEnum">値の型</typeparam>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="expression">表示する値を含むオブジェクトを特定する式</param>
        /// <param name="target">表示する対象の列挙値</param>
        /// <returns>式で表される列挙の指定された値の HTML radio 要素</returns>
        public static MvcHtmlString EnumRadioButtonFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, TEnum target)
        {
            return htmlHelper.EnumRadioButtonFor(expression, target, null);
        }

        /// <summary>
        /// 指定された式で表される列挙のうち、指定された値の HTML radio 要素を返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型</typeparam>
        /// <typeparam name="TEnum">値の型</typeparam>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="expression">表示する値を含むオブジェクトを特定する式</param>
        /// <param name="target">表示する対象の列挙値</param>
        /// <param name="labelHtmlAttributes">ラベル要素に設定する HTML 属性を格納するオブジェクト</param>
        /// <returns>式で表される列挙の指定された値の HTML radio 要素</returns>
        public static MvcHtmlString EnumRadioButtonFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, TEnum target, object labelHtmlAttributes)
        {
            return RadioButtonHelper(
                htmlHelper,
                ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData),
                target as Enum,
                HtmlHelper.AnonymousObjectToHtmlAttributes(labelHtmlAttributes));
        }

        /// <summary>
        /// 指定された <see cref="ModelMetadata"/> の HTML radio 要素を返します。
        /// </summary>
        /// <param name="html">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="metadata">この要素を表すメタデータ</param>
        /// <param name="target">表示する対象の列挙値</param>
        /// <param name="labelHtmlAttributes">ラベル要素に設定する HTML 属性を格納するオブジェクト</param>
        /// <returns>HTML radio 要素</returns>
        private static MvcHtmlString RadioButtonHelper(HtmlHelper html, ModelMetadata metadata, Enum target, IDictionary<string, object> labelHtmlAttributes = null)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }

            var name = metadata.PropertyName;
            var fullName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            var result = new StringBuilder();

            var currentValue = metadata.Model as Enum;
            var selectList = EnumHelper.GetSelectList(metadata.ModelType, currentValue);
            var targetValue = target?.ToString("d");

            foreach (var selectListItem in selectList)
            {
                // 対象の要素が指定されている場合、指定されていない要素は処理しない
                if (targetValue != null && targetValue != selectListItem.Value)
                {
                    continue;
                }

                // ラジオ要素の生成 id は fullName-Value とする
                var id = fullName + "-" + selectListItem.Value;
                var radio = new TagBuilder("input");
                radio.MergeAttribute("id", id);
                radio.MergeAttribute("type", "radio");
                radio.MergeAttribute("name", fullName, true);
                radio.MergeAttribute("value", selectListItem.Value);

                if (selectListItem.Selected)
                {
                    radio.MergeAttribute("checked", "checked");
                }

                // ラベル要素の生成
                var label = new TagBuilder("label");
                label.MergeAttributes(labelHtmlAttributes, replaceExisting: true);
                label.Attributes.Add("for", id);
                label.InnerHtml = radio.ToString(TagRenderMode.SelfClosing) + " " + selectListItem.Text;
                result.Append(label.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}