using System;

namespace Prototype.Mvc.Validations
{
    /// <summary>
    /// プロパティの値が半角英数であることを指定します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AlphaNumberAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AlphaNumberAttribute() : base("MS007", @"[a-zA-Z0-9]+")
        {
        }
    }
}