using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Prototype.Mvc.Validations
{
    /// <summary>
    /// アノテーション検証コントロール。
    /// </summary>
    public class AnnotationsValidator
    {
        /// <summary>
        /// 検証エラーメッセージの一覧を返します。
        /// </summary>
        /// <param name="instance">検証するオブジェクト</param>
        /// <param name="validations">検証する検証属性</param>
        /// <param name="value">検証する値</param>
        /// <param name="displayName">検証する値の表示名</param>
        /// <param name="memberName">検証する値の項目名</param>
        /// <returns>エラーメッセージの一覧</returns>
        public IEnumerable<ValidationResult> Validate(object instance, ValidationAttribute[] validations, object value, string displayName, string memberName)
        {
            ValidationContext context = new ValidationContext(instance)
            {
                DisplayName = displayName,
                MemberName = memberName,
            };
            foreach (var validation in validations)
            {
                var result = validation.GetValidationResult(value, context);
                if (result != null)
                {
                    yield return result;
                }
            }
        }
    }
}