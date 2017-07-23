using Prototype.Mvc.Environment;
using NLog;

namespace Prototype
{
    /// <summary>
    /// ロガーの設定クラス。
    /// </summary>
    internal static class LoggerConfig
    {
        /// <summary>
        /// ロガーを登録します。
        /// </summary>
        public static void RegisterLoggers()
        {
            GlobalDiagnosticsContext.Set("outputDir", AppEnvironment.LogsDir);
            LogManager.Configuration.Reload();
        }
    }
}