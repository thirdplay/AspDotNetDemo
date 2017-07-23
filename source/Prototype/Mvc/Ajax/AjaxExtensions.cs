using Prototype.ViewModels;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System;
using System.Web.Helpers;

namespace Prototype.Mvc.Ajax
{
    /// <summary>
    /// AJAXの拡張機能を提供します。
    /// </summary>
    public static class AjaxExtensions
    {
        /// <summary>
        /// AJAX スクリプトを実行するためのオプション設定を作成します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型</typeparam>
        /// <param name="ajaxHelper">AJAX を使用した HTML マークアップの表示に使用される <see cref="AjaxHelper"/> オブジェクト</param>
        /// <param name="httpMethod">HTTP 要求メソッド ("Get" または "Post")</param>
        /// <param name="onBegin">ページが更新される直前に呼び出される JavaScript 関数の名前</param>
        /// <param name="onComplete">応答データがインスタンス化された後、ページが更新される前に呼び出される JavaScript 関数</param>
        /// <param name="onFailure">ページの更新が失敗した場合に呼び出される JavaScript 関数</param>
        /// <param name="onSuccess">ページが正常に更新された後に呼び出される JavaScript 関数</param>
        /// <param name="updateTargetId">サーバーからの応答を使用して更新される DOM 要素の ID</param>
        /// <returns>AJAX スクリプトを実行するためのオプション設定</returns>
        public static AjaxOptions CreateAjaxOptions<TModel>(this AjaxHelper<TModel> ajaxHelper,
            [Optional, DefaultParameterValue("POST")] string httpMethod,
            [Optional, DefaultParameterValue("PROTOTYPE.ajax.onBegin")] string onBegin,
            [Optional, DefaultParameterValue("PROTOTYPE.ajax.onComplete")] string onComplete,
            [Optional, DefaultParameterValue("PROTOTYPE.ajax.onFailure")] string onFailure,
            [Optional, DefaultParameterValue("PROTOTYPE.ajax.onSuccess")] string onSuccess,
            [Optional, DefaultParameterValue("")] string updateTargetId)
        {
            return AjaxOptionsHelper(
                httpMethod,
                onBegin,
                onComplete,
                onFailure,
                onSuccess,
                updateTargetId
            );
        }

        /// <summary>
        /// 検索処理を実行するためのオプション設定を作成します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型</typeparam>
        /// <param name="ajaxHelper">AJAX を使用した HTML マークアップの表示に使用される <see cref="AjaxHelper"/> オブジェクト</param>
        /// <param name="httpMethod">HTTP 要求メソッド ("Get" または "Post")</param>
        /// <returns>AJAX スクリプトを実行するためのオプション設定</returns>
        public static AjaxOptions CreateAjaxOptionsSearch<TModel>(this AjaxHelper<TModel> ajaxHelper,
            [Optional, DefaultParameterValue("POST")] string httpMethod)
        {
            return CreateAjaxOptions(ajaxHelper, httpMethod: httpMethod, updateTargetId: "SearchResult");
        }

        /// <summary>
        /// AJAX スクリプトを実行するためのオプション設定を作成します。
        /// </summary>
        /// <param name="httpMethod">HTTP 要求メソッド ("Get" または "Post")</param>
        /// <param name="onBegin">ページが更新される直前に呼び出される JavaScript 関数の名前</param>
        /// <param name="onComplete">応答データがインスタンス化された後、ページが更新される前に呼び出される JavaScript 関数</param>
        /// <param name="onFailure">ページの更新が失敗した場合に呼び出される JavaScript 関数</param>
        /// <param name="onSuccess">ページが正常に更新された後に呼び出される JavaScript 関数</param>
        /// <param name="updateTargetId">サーバーからの応答を使用して更新される DOM 要素の ID</param>
        /// <returns>AJAX スクリプトを実行するためのオプション設定</returns>
        private static AjaxOptions AjaxOptionsHelper(string httpMethod, string onBegin, string onComplete, string onFailure, string onSuccess, string updateTargetId)
        {
            return new AjaxOptions
            {
                HttpMethod = httpMethod,
                OnBegin = onBegin,
                OnComplete = onComplete,
                OnFailure = onFailure,
                OnSuccess = onSuccess,
                UpdateTargetId = updateTargetId,
            };
        }

        /// <summary>
        /// 指定されたラベルについて、のソートリンクの a 要素を返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型</typeparam>
        /// <param name="ajaxHelper">AJAX を使用した HTML マークアップの表示に使用される <see cref="AjaxHelper"/> オブジェクト</param>
        /// <param name="linkText">アンカー要素の内部テキスト</param>
        /// <param name="sortColumn">ソート対象のカラム</param>
        /// <param name="ajaxOptions">非同期要求のオプションを提供するオブジェクト</param>
        /// <returns>HTML a 要素</returns>
        public static MvcHtmlString SortLink<TModel>(this AjaxHelper<TModel> ajaxHelper, string linkText, string sortColumn, AjaxOptions ajaxOptions)
        {
            return ajaxHelper.SortLink(linkText, "SearchUpdate", sortColumn, ajaxOptions);
        }

        /// <summary>
        /// 指定されたラベルについて、のソートリンクの a 要素を返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型</typeparam>
        /// <param name="ajaxHelper">AJAX を使用した HTML マークアップの表示に使用される <see cref="AjaxHelper"/> オブジェクト</param>
        /// <param name="linkText">アンカー要素の内部テキスト</param>
        /// <param name="actionName">アクションメソッドの名前</param>
        /// <param name="sortColumn">ソート対象のカラム</param>
        /// <param name="ajaxOptions">非同期要求のオプションを提供するオブジェクト</param>
        /// <returns>HTML a 要素</returns>
        public static MvcHtmlString SortLink<TModel>(this AjaxHelper<TModel> ajaxHelper, string linkText, string actionName, string sortColumn, AjaxOptions ajaxOptions)
        {
            var result = new StringBuilder();
            var model = ajaxHelper.ViewData.Model as ListViewModelBase;
            if (model == null)
            {
                throw new ArgumentException(nameof(ajaxHelper.ViewData.Model));
            }

            // リンク要素の生成
            var anchor = ajaxHelper.ActionLink(linkText, actionName, new { SortColumn = sortColumn }, ajaxOptions).ToString();
            result.Append(anchor.ToString());

            // ソート対象のカラムがソート中の場合、並び替え方向を示すアイコンを表示する
            if (model.SortColumn == sortColumn)
            {
                // ラベル要素の生成
                var span = new TagBuilder("span");
                span.AddCssClass(model.SortDirection == SortDirection.Ascending
                    ? "fa fa-sort-asc"
                    : "fa fa-sort-desc");
                result.Append(" " + span);
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}