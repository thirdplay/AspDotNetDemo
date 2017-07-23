using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Prototype.Mvc.Validations
{
    /// <summary>
    /// プロパティの値が日付であることを指定します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DateAttribute : ValidationBaseAttribute
    {
        /// <summary>
        /// チェックする日付の書式
        /// </summary>
        public string Format { get; set; } = "yyyy/MM/dd";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DateAttribute() : base("MS009")
        {
        }

        /// <summary>
        /// データフィールド値が日付であるとをチェックします。
        /// </summary>
        /// <param name="value">検証するデータフィールド値</param>
        /// <returns>検証が成功した場合はtrue。それ以外の場合はfalse。</returns>
        protected override bool IsValidCore(object value)
        {
            var str = value as string;
            if (!string.IsNullOrEmpty(str))
            {
                // 日付形式への変換
                DateTime result;
                if (!DateTime.TryParseExact(str, Format, null, DateTimeStyles.None, out result))
                {
                    return false;
                }
            }
            return true;
        }
    }
}