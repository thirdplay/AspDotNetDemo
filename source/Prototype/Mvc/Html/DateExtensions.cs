using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace Prototype.Mvc.Html
{
	/// <summary>
	/// 日付関連のHTML拡張機能を提供します。
	/// </summary>
	public static class DateExtensions
	{
		/// <summary>
		/// 指定された式で表されるプロパティ（期間日付）の HTML input 要素および div 要素を返します。
		/// </summary>
		/// <typeparam name="TModel">モデルの型</typeparam>
		/// <typeparam name="TValue">値の型</typeparam>
		/// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
		/// <param name="expression">表示するプロパティを識別する式</param>
		/// <returns>式で表されるプロパティの HTML input 要素および div 要素</returns>
		public static MvcHtmlString PeriodDateFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression)
		{
			return htmlHelper.PeriodDateFor(
				expression,
				new { @class = "form-control" });
		}

		/// <summary>
		/// 指定された式で表されるプロパティ（期間日付）の HTML input 要素および div 要素を返します。
		/// </summary>
		/// <typeparam name="TModel">モデルの型</typeparam>
		/// <typeparam name="TValue">値の型</typeparam>
		/// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
		/// <param name="expression">表示するプロパティを識別する式</param>
		/// <param name="inputHtmlAttributes">input 要素に設定する HTML 属性を格納するオブジェクト</param>
		/// <returns>式で表されるプロパティの HTML input 要素および div 要素</returns>
		public static MvcHtmlString PeriodDateFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object inputHtmlAttributes)
		{
			return htmlHelper.PeriodDateFor(
				expression,
				inputHtmlAttributes,
				new { @class = "input-group" });
		}

		/// <summary>
		/// 指定された式で表されるプロパティ（期間日付）の HTML input 要素および div 要素を返します。
		/// </summary>
		/// <typeparam name="TModel">モデルの型</typeparam>
		/// <typeparam name="TValue">値の型</typeparam>
		/// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
		/// <param name="expression">表示するプロパティを識別する式</param>
		/// <param name="inputHtmlAttributes">input 要素に設定する HTML 属性を格納するオブジェクト</param>
		/// <param name="divHtmlAttributes">div 要素に設定する HTML 属性を格納するオブジェクト</param>
		/// <returns>式で表されるプロパティの HTML input 要素および div 要素</returns>
		public static MvcHtmlString PeriodDateFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object inputHtmlAttributes, object divHtmlAttributes)
		{
			return DateHelper(
				htmlHelper,
				ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData),
				HtmlHelper.AnonymousObjectToHtmlAttributes(inputHtmlAttributes),
				HtmlHelper.AnonymousObjectToHtmlAttributes(divHtmlAttributes));
		}

		/// <summary>
		/// 指定された <see cref="ModelMetadata"/> の HTML input 要素および span要素を返します。
		/// </summary>
		/// <param name="html">このメソッドによって拡張される HTML ヘルパー インスタンス</param>
		/// <param name="metadata">この要素を表すメタデータ</param>
		/// <param name="inputHtmlAttributes">input 要素に設定する HTML 属性を格納するオブジェクト</param>
		/// <param name="divHtmlAttributes">div 要素に設定する HTML 属性を格納するオブジェクト</param>
		/// <returns>HTML input 要素および span 要素</returns>
		private static MvcHtmlString DateHelper(HtmlHelper html, ModelMetadata metadata, IDictionary<string, object> inputHtmlAttributes = null, IDictionary<string, object> divHtmlAttributes = null)
		{
			if (metadata == null)
			{
				throw new ArgumentNullException(nameof(metadata));
			}

			var result = new StringBuilder();
			var name = metadata.PropertyName;
			var fullName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
			var value = html.FormatValue(metadata.Model, "{0:yyyy/MM/dd}");

			// テキストボックスの作成
			var input = new TagBuilder("input");
			input.MergeAttributes(inputHtmlAttributes, replaceExisting: true);
			input.MergeAttribute("id", fullName);
			input.MergeAttribute("type", "text");
			input.MergeAttribute("name", fullName, true);
			input.MergeAttribute("value", value);
			input.AddCssClass("datetext");

			// カレンダーアイコンの作成
			var span = new TagBuilder("span");
			span.AddCssClass("input-group-addon");
			span.InnerHtml = @"<i class=""glyphicon glyphicon-calendar""></i>";

			// DIVの作成
			if (divHtmlAttributes != null)
			{
				var div = new TagBuilder("div");
				div.MergeAttributes(divHtmlAttributes, replaceExisting: true);
				div.MergeAttribute("id", fullName + "-date");
				div.AddCssClass($"date {name}Div");
				div.InnerHtml = input.ToString(TagRenderMode.SelfClosing) + span.ToString();
				result.Append(div.ToString());
			}
			else
			{
				result.Append(input.ToString(TagRenderMode.SelfClosing));
				result.Append(span.ToString());
			}

			return MvcHtmlString.Create(result.ToString());
		}
	}
}