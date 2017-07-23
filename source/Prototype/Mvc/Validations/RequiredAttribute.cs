using System;

namespace Prototype.Mvc.Validations
{
    /// <summary>
    /// プロパティの値が必須であることを指定します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredAttribute : ValidationBaseAttribute
    {
        /// <summary>
        /// 空の文字列を許可するかどうかを示す値
        /// </summary>
        public bool AllowEmptyStrings { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RequiredAttribute() : base("MS001")
        {
        }

        /// <summary>
        /// 必須データフィールド値が空でないことをチェックします。
        /// </summary>
        /// <param name="value">検証するデータフィールド値</param>
        /// <returns>検証が成功した場合はtrue。それ以外の場合はfalse。</returns>
        protected override bool IsValidCore(object value)
        {
            if (value == null)
            {
                return false;
            }

            // 空の文字列が許可されていない場合にのみ、文字列の長さをチェックする
            var stringValue = value as string;
            if (stringValue != null && !AllowEmptyStrings)
            {
                if (stringValue.Trim().Length == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}