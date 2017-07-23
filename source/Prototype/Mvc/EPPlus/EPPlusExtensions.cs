using OfficeOpenXml;
using System.Linq;

namespace Prototype.Mvc.EPPlus
{
    /// <summary>
    /// EPPlusの拡張機能を提供します。
    /// </summary>
    public static class EPPlusExtensions
    {
        /// <summary>
        /// ワークシートが <see cref="ExcelWorksheets"/> に存在するか判定します。
        /// </summary>
        /// <param name="worksheets">ワークシートのコレクション</param>
        /// <param name="sheetName">判定するシート名</param>
        /// <returns>存在する場合はtrue。存在しない場合はfalse。</returns>
        public static bool Contains(this ExcelWorksheets worksheets, string sheetName)
        {
            foreach (var worksheet in worksheets)
            {
                if (worksheet.Name == sheetName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// アクティブなワークシートを取得します。
        /// </summary>
        /// <param name="excel">Excelパッケージ</param>
        /// <returns>存在する場合はtrue。存在しない場合はfalse。</returns>
        public static ExcelWorksheet GetActiveSheet(this ExcelPackage excel)
        {
            return excel.Workbook.Worksheets.FirstOrDefault(x => x.View.TabSelected);
        }

        /// <summary>
        /// 新しい空のワークシートを追加します。
        /// </summary>
        /// <param name="worksheets">ワークシートコレクション</param>
        /// <param name="sheetName">ワークシートの名前</param>
        /// <param name="fontName">フォント名</param>
        /// <returns>ワークシート</returns>
        public static ExcelWorksheet Add(this ExcelWorksheets worksheets, string sheetName, string fontName)
        {
            var worksheet = worksheets.Add(sheetName);
            worksheet.Cells.Style.Font.Name = fontName;
            return worksheet;
        }
    }
}