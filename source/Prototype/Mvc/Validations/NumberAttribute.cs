using System;
using System.Globalization;

namespace Prototype.Mvc.Validations
{
    /// <summary>
    /// プロパティの値が数値であることを指定します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NumberAttribute : ValidationBaseAttribute
    {
        /// <summary>
        /// 負の値が許可されるかどうかを示す値
        /// </summary>
        public bool AllowNegative { get; set; }

        /// <summary>
        /// 小数を許可されるかどうかを示す値
        /// </summary>
        public bool AllowDecimal { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NumberAttribute() : base("MS010")
        {
        }

        /// <summary>
        /// オブジェクトの指定した値が有効かどうかを判断します。
        /// </summary>
        /// <param name="value">検証対象のオブジェクトの値</param>
        /// <returns>検証が成功した場合はtrue。それ以外の場合はfalse。</returns>
        protected override bool IsValidCore(object value)
        {
            // 値が null または空の場合、チェック対象外
            var str = value as string;
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }

            // 負の値が許可されていない場合のみ、"-"をチェックする
            if (!this.AllowNegative && str.IndexOf("-") >= 0)
            {
                return false;
            }
            // 小数が許可されていない場合のみ、"."をチェックする
            if (!this.AllowDecimal && str.IndexOf(".") >= 0)
            {
                return false;
            }

            // 数値形式への変換
            decimal result;
            if (!decimal.TryParse(str, out result))
            {
                return false;
            }

            return true;
        }
    }
}