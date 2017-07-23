using System.ComponentModel.DataAnnotations;

namespace Prototype.Constants
{
    /// <summary>
    /// 結果コードを表します。
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 失敗
        /// </summary>
        [Display(Name = "失敗")]
        Failure = 0,

        /// <summary>
        /// 成功
        /// </summary>
        [Display(Name = "成功")]
        Success,

        /// <summary>
        /// 未完了
        /// </summary>
        [Display(Name = "未完了")]
        Incomplete = 9,
    }
}