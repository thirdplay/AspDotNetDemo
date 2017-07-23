using PagedList;
using System;
using System.Web.Helpers;

namespace Prototype.ViewModels
{
    /// <summary>
    /// 一覧画面のViewModelの基底クラス。
    /// </summary>
    [Serializable]
    public abstract class ListViewModelBase : ViewModelBase
    {
        /// <summary>
        /// 現在のページ番号
        /// </summary>
        public int? PageNumber { get; set; }

        /// <summary>
        /// 1ページの表示件数
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// ソート対象のカラム
        /// </summary>
        public string SortColumn { get; set; }

        /// <summary>
        /// ソートの並び替え方向
        /// </summary>
        public SortDirection SortDirection { get; set; }

        /// <summary>
        /// ページング情報を含む一覧
        /// </summary>
        public abstract IPagedList PagedList { get; }
    }
}