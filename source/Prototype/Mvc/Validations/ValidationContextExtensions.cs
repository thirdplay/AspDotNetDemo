using System;
using System.ComponentModel.DataAnnotations;

namespace Prototype.Mvc.Validations
{
    /// <summary>
    /// 検証操作に関するコンテキスト情報の拡張機能を提供します。
    /// </summary>
    public static class ValidationContextExtensions
    {
        /// <summary>
        /// <see cref="ValidationContext"/> の対象プロパティから指定された属性を取得します。
        /// </summary>
        /// <param name="validationContext">検証操作に関するコンテキスト情報</param>
        /// <param name="attribute">取得する属性</param>
        /// <returns>属性の配列</returns>
        public static Attribute[] GetAttributes(this ValidationContext validationContext, Type attribute)
        {
            if (validationContext.MemberName == null)
            {
                return Array.Empty<Attribute>();
            }
            var propertyInfo = validationContext.ObjectType.GetProperty(validationContext.MemberName);
            if (propertyInfo == null)
            {
                return Array.Empty<Attribute>();
            }
            return Attribute.GetCustomAttributes(propertyInfo, attribute);
        }
    }
}