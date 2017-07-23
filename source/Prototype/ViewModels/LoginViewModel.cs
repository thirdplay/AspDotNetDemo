using System.ComponentModel;
using RequiredAttribute = Prototype.Mvc.Validations.RequiredAttribute;

namespace Prototype.ViewModels
{
    /// <summary>
    /// ログイン画面のViewModel。
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        /// <summary>
        /// ユーザID
        /// </summary>
        [Required]
        [DisplayName("ユーザID")]
        public string UserId { get; set; }
    }
}