using Prototype.Mvc.Environment;
using System;
using System.Linq;

using System.Text.RegularExpressions;

namespace Prototype.Mvc.Extensions
{
    /// <summary>
    /// 文字列の拡張機能を提供します。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// スネークケースをパスカルケースに変換します。
        /// </summary>
        /// <param name="value">変換する文字列</param>
        /// <returns>変換後の文字列</returns>
        public static string SnakeToPascal(this string value)
        {
            return value.ToLower()
                .Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
                .Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }

        /// <summary>
        /// パスカルケースをスネークケースに変換します。
        /// </summary>
        /// <param name="value">変換する文字列</param>
        /// <returns>変換後の文字列</returns>
        public static string PascalToSnake(this string value)
        {
            return Regex.Replace(value, "([A-Z])", "_$1").ToUpper().Trim('_');
        }

        /// <summary>
        /// 文字列が null か <see cref="string.Empty"/> の場合、デフォルト値を返します。
        /// </summary>
        /// <param name="value">対象の文字列</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>文字列が null <see cref="string.Empty"/> の場合、デフォルト値。それ以外の場合は value。</returns>
        public static string DefaultIfEmpty(this string value, string defaultValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            return value;
        }

        /// <summary>
        /// LIKE検索文字列をエスケープして返します。
        /// </summary>
        /// <param name="str">エスケープ対象のLIKE検索文字列</param>
        /// <param name="escapeStr">エスケープ文字</param>
        /// <returns>エスケープ後の文字列</returns>
        public static string EscapeLikeSearchString(this string str, char escapeStr = '\\')
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            return Regex.Replace(str, @"([\%_％＿])", escapeStr + "$1");
        }
    }
}