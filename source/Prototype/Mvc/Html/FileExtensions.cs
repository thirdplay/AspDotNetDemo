using System;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Prototype.Mvc.Html
{
    /// <summary>
    /// ファイル関連のHTML拡張機能を提供します。
    /// </summary>
    public static class FileExtensions
    {
        /// <summary>
        /// 指定された式で表されるオブジェクトの各プロパティについて、ファイル項目の input 要素を返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型</typeparam>
        /// <typeparam name="TValue">値の型</typeparam>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式</param>
        /// <returns>指定された式で表されるオブジェクトの各プロパティで、type 属性が "checkbox" に設定されている HTML の input 要素</returns>
        public static MvcHtmlString FileFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression)
        {
            return htmlHelper.FileFor(expression, ".xlsx");
        }

        /// <summary>
        /// 指定された式で表されるオブジェクトの各プロパティについて、ファイル項目の input 要素を返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型</typeparam>
        /// <typeparam name="TValue">値の型</typeparam>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式</param>
        /// <param name="extension">許可するファイルの拡張子</param>
        /// <returns>指定された式で表されるオブジェクトの各プロパティで、type 属性が "checkbox" に設定されている HTML の input 要素</returns>
        public static MvcHtmlString FileFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, string extension)
        {
            return FileHelper(
                htmlHelper,
                ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData),
                extension);
        }

        /// <summary>
        /// 指定された <see cref="ModelMetadata"/> の HTML input(file) 要素を返します。
        /// </summary>
        /// <param name="html">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
        /// <param name="metadata">この要素を表すメタデータ</param>
        /// <param name="extension">許可するファイルの拡張子</param>
        /// <returns>HTML input 要素</returns>
        private static MvcHtmlString FileHelper(HtmlHelper html, ModelMetadata metadata, string extension)
        {
            var result = new StringBuilder();
            var name = metadata.PropertyName;
            var fullName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            // FILE要素の生成
            var fileFullName = fullName + "-File";
            var file = new TagBuilder("input");
            file.MergeAttribute("type", "file");
            file.MergeAttribute("name", fileFullName, true);
            file.MergeAttribute("accept", extension);
            file.MergeAttribute("data-item-id", fullName);
            file.MergeAttribute("data-file-extension", extension);
            file.GenerateId(fileFullName);
            result.Append(file.ToString(TagRenderMode.SelfClosing));

            // HIDDEN要素の生成
            result.Append(html.Hidden(fullName).ToString());
            result.Append(html.Hidden(fullName + "Original").ToString());

            return MvcHtmlString.Create(result.ToString());
        }
    }
}