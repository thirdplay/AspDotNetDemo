using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prototype.Mvc.Extensions
{
    /// <summary>
    /// decimalの拡張機能を提供します。
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// decimalの値をdoubleに変換します。
        /// </summary>
        /// <param name="value">変換する値</param>
        /// <returns>変換後の値</returns>
        public static double ToDouble(this decimal value)
        {
            return decimal.ToDouble(value);
        }
    }
}