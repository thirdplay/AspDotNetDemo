using System;

namespace Prototype.Mvc.Validations
{
    /// <summary>
    /// プロパティの値が半角英数記号であることを指定します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AlphaNumberSymbolAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AlphaNumberSymbolAttribute() : base("MS008", @"[a-zA-Z0-9 -/:-@\[-\`\{-\~]+")
        {
        }
    }
}