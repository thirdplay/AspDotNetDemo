using System;
using System.Text;

namespace Prototype.Mvc.Validations
{
    /// <summary>
    /// プロパティで許容される最大値を指定します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxValueAttribute : ValidationBaseAttribute
    {
        /// <summary>
        /// 許容される最大値を取得します。
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">許容される最大値</param>
        public MaxValueAttribute(int value) : base("MS012")
        {
            this.Value = value;
        }

        /// <summary>
        /// 指定したエラーメッセージに書式を適用します
        /// </summary>
        /// <param name="name">書式設定された文字列に含まれる名前</param>
        /// <returns>許容される最大値を説明する文字列</returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(this.ErrorMessageString, name, this.Value);
        }

        /// <summary>
        /// プロパティが最大値を超過していないことを確認します。
        /// </summary>
        /// <param name="value">検証するプロパティの値</param>
        /// <returns>検証が成功した場合はtrue。それ以外の場合はfalse。</returns>
        protected override bool IsValidCore(object value)
        {
            // 値が null または空の場合、チェック対象外
            var str = value as string;
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }

            // 数値形式への変換
            decimal result;
            if (!decimal.TryParse(str, out result))
            {
                return true;
            }

            if (result <= this.Value)
            {
                return true;
            }
            return false;
        }
    }
}