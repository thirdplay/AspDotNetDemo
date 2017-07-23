using Prototype.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [RegularExpression(@"^a\d{7}", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MS014")]
        [DisplayName("ユーザID")]
        public string UserId { get; set; }
    }
}