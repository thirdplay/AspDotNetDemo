using System.ComponentModel;

namespace Prototype.ViewModels
{
    /// <summary>
    /// ファイルコントローラーのViewModel
    /// </summary>
    public class FileViewModel : ViewModelBase
    {
        /// <summary>
        /// 画面ID
        /// </summary>
        public string ScreenId { get; set; }

        /// <summary>
        /// 項目ID
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName { get; set; }
    }
}