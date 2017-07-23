using System.ComponentModel.DataAnnotations;

namespace Prototype.Constants
{
    /// <summary>
    /// 現在の環境を表します。
    /// </summary>
    public enum EnvironmentCode : int
    {
        /// <summary>
        /// ローカル環境
        /// </summary>
        [Display(Name = "ローカル環境")]
        Local = 1,

        /// <summary>
        /// 開発環境
        /// </summary>
        [Display(Name = "開発環境")]
        Development,

        /// <summary>
        /// 本番環境
        /// </summary>
        [Display(Name = "本番環境")]
        Production,
    }
}