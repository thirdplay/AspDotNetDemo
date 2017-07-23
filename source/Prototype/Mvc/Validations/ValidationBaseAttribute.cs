using Prototype.Resources;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Prototype.Mvc.Validations
{
    /// <summary>
    /// 入力検証の基底クラス。
    /// </summary>
    public abstract class ValidationBaseAttribute : ValidationAttribute
    {
        /// <summary>
        /// リソース名
        /// </summary>
        public string ResourceName { get; private set; }

        /// <summary>
        /// 入力検証を強制的に有効にするかどうかを示す値。
        /// </summary>
        public bool ForceEnabled { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="resourceName">検証が失敗した場合にエラーメッセージリソース種類のプロパティ値を参照するために使用するリソース名</param>
        protected ValidationBaseAttribute(string resourceName)
        {
            ResourceName = resourceName;
            ErrorMessageResourceType = typeof(Messages);
            ErrorMessageResourceName = resourceName;
            ErrorMessage = null;
        }

        /// <summary>
        /// 入力検証が有効かどうか判断します。
        /// </summary>
        /// <param name="validationContext">検証操作に関するコンテキスト情報</param>
        /// <returns>有効な場合はtrue。無効な場合はfalse。</returns>
        protected bool IsEnabled(ValidationContext validationContext)
        {
            // 強制有効状態の場合、無条件で有効にする
            if (ForceEnabled)
            {
                return true;
            }

            // 入力検証の条件をチェック
            var attributes = validationContext.GetAttributes(typeof(ConditionalAttribute)) as ConditionalAttribute[];
            if (attributes != null && attributes.Any(x => !x.IsMatch(validationContext)))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 現在の検証属性に対して指定された値を検証します。
        /// </summary>
        /// <param name="value">検証するデータフィールド値</param>
        /// <param name="validationContext">検証操作に関するコンテキスト情報</param>
        /// <returns>検証要求の結果</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = ValidationResult.Success;

            // 入力検証の有効チェック
            if (IsEnabled(validationContext))
            {
                // 入力検証の実行
                if (!IsValidCore(value))
                {
                    string[] memberNames = validationContext.MemberName != null ? new string[] { validationContext.MemberName } : null;
                    result = new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName), memberNames);
                }
            }

            return result;
        }

        /// <summary>
        /// 入力検証のコア処理。
        /// </summary>
        /// <param name="value">検証するデータフィールド値</param>
        /// <returns>検証要求の結果</returns>
        protected abstract bool IsValidCore(object value);
    }
}