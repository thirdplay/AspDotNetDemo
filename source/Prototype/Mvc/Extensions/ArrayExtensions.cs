using System;

namespace Prototype.Mvc.Extensions
{
    /// <summary>
    /// 配列の拡張機能を提供します
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// 配列が null または空配列かどうかを判定します。
        /// </summary>
        /// <param name="array">対象配列</param>
        /// <returns>null か空配列の場合はtrue。それ以外はfalseを返す</returns>
        public static bool IsNullOrEmpty(this Array array)
        {
            return array == null || array.Length == 0;
        }
    }
}