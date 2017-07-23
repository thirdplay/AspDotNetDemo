namespace Prototype.Entities
{
    /// <summary>
    /// 選択肢のリスト項目を表します。
    /// </summary>
    public class ListItem
    {
        /// <summary>
        /// 選択されている項目のテキスト
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 選択されている項目の値
        /// </summary>
        public string Value { get; set; }
    }
}