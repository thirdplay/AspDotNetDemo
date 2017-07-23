using System;

namespace Prototype.Mvc.Validations
{
    /// <summary>
    /// プロパティの値が半角英語であることを指定します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AlphabetAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AlphabetAttribute() : base("MS006", @"[a-zA-Z]+")
        {
        }
    }
}