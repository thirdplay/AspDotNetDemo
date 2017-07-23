using System;

namespace Prototype.Constants
{
    /// <summary>
    /// サポートされているExcelのファイル形式。
    /// </summary>
    public enum SupportedExcelFormat
    {
        /// <summary>
        /// Excelブック
        /// </summary>
        Xlsx,

        /// <summary>
        /// Excelマクロ有効ブック
        /// </summary>
        Xlsm,

        /// <summary>
        /// Csv
        /// </summary>
        Csv,
    }

    /// <summary>
    /// Excelファイル形式の拡張機能を提供します。
    /// </summary>
    public static class SupportedExcelFormatExtensions
    {
        /// <summary>
        /// Excelファイル形式の拡張子を取得します。
        /// </summary>
        /// <param name="format">Excelファイル形式</param>
        /// <returns>ファイル拡張子</returns>
        public static string GetExtension(this SupportedExcelFormat format)
        {
            switch (format)
            {
                case SupportedExcelFormat.Xlsx:
                    return ".xlsx";

                case SupportedExcelFormat.Xlsm:
                    return ".xlsm";

                case SupportedExcelFormat.Csv:
                    return ".csv";
            }
            throw new ArgumentException();
        }
    }
}