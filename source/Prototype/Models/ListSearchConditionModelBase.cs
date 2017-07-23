using System;
using System.Web.Helpers;

namespace Prototype.Models
{
    /// <summary>
    /// 一覧画面の検索条件モデルの基底クラス。
    /// </summary>
    public class ListSearchConditionModelBase
    {
        /// <summary>
        /// 現在のページ番号
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 1ページの表示件数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// ソート対象のカラム
        /// </summary>
        public string SortColumn { get; set; }

        /// <summary>
        /// ソートの並び替え方向
        /// </summary>
        public SortDirection SortDirection { get; set; }

        /// <summary>
        /// ソート対象のカラムリスト
        /// </summary>
        public virtual string[] SortColumnList
        {
            get
            {
                if (string.IsNullOrEmpty(SortColumn))
                {
                    return null;
                }
                var sortDirection = SortDirection == SortDirection.Ascending ? "asc" : "desc";
                return new string[] {
                    $"{SortColumn} {sortDirection}"
                };
            }
        }
    }
}