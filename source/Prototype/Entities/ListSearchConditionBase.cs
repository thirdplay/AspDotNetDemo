namespace Prototype.Entities
{
    /// <summary>
    /// 一覧の検索条件の基底クラス。
    /// </summary>
    public abstract class ListSearchConditionBase
    {
        /// <summary>
        /// 開始行
        /// </summary>
        public int? StartRow { get; set; }

        /// <summary>
        /// 終了行
        /// </summary>
        public int? EndRow { get; set; }

        /// <summary>
        /// ソート対象のカラム名リスト
        /// </summary>
        public string[] SortColumnList { get; set; }

        /// <summary>
        /// ページング情報に従い、検索条件の開始行と終了行を設定します。
        /// </summary>
        /// <param name="pageNumber">現在のページ番号</param>
        /// <param name="pageSize">1ページに表示する件数</param>
        public void SetLimit(int pageNumber, int pageSize)
        {
            this.StartRow = (pageNumber - 1) * pageSize + 1;
            this.EndRow = this.StartRow + pageSize - 1;
        }
    }
}