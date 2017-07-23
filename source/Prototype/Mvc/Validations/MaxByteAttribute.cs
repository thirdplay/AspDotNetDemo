using System;
using System.Text;

namespace Prototype.Mvc.Validations
{
    /// <summary>
    /// プロパティで許容される最大byte数を指定します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxByteAttribute : ValidationBaseAttribute
    {
        /// <summary>
        /// 許容される最大byte数を取得します。
        /// </summary>
        public int Byte { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="byte">許容される最大byte数</param>
        public MaxByteAttribute(int @byte) : base("MS005")
        {
            this.Byte = @byte;
        }

        /// <summary>
        /// 指定したエラーメッセージに書式を適用します
        /// </summary>
        /// <param name="name">書式設定された文字列に含まれる名前</param>
        /// <returns>許容される最大byte数を説明する文字列</returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(this.ErrorMessageString, name, this.Byte);
        }

        /// <summary>
        /// プロパティが最大byte数を超過していないことを確認します。
        /// </summary>
        /// <param name="value">検証するプロパティの値</param>
        /// <returns>検証要求の結果</returns>
        protected override bool IsValidCore(object value)
        {
            // 値が null または空の場合、チェック対象外
            var str = value as string;
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }

            var @byte = Encoding.UTF8.GetByteCount(str);
            if (@byte <= this.Byte)
            {
                return true;
            }
            return false;
        }
    }
}