using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Prototype.Mvc.Extensions
{
    /// <summary>
    /// 表示名の拡張機能を提供します。
    /// </summary>
    public static class DisplayExtensions
    {
        /// <summary>
        /// 指定されたカスタム属性のプロバイダから表示名を取得します。
        /// </summary>
        /// <param name="provider">カスタム属性のプロバイダ</param>
        /// <returns>表示名</returns>
        public static string GetDisplayName(this ICustomAttributeProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            var attributes = provider.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
            return attributes != null && attributes.Any()
                ? attributes[0].Name
                : null;
        }

        /// <summary>
        /// 列挙型の表示名を取得します。
        /// </summary>
        /// <param name="value">列挙型</param>
        /// <returns>表示名</returns>
        public static string GetDisplayName(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            var info = type.GetField(name);

            return GetDisplayName(info);
        }
    }
}