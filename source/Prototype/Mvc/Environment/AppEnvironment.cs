using Prototype.Constants;
using System;
using System.Configuration;
using System.Web.Configuration;
using System.Web.Hosting;

namespace Prototype.Mvc.Environment
{
    /// <summary>
    /// 現在の環境に関する情報、およびそれらを操作する手段を提供します。
    /// </summary>
    public static class AppEnvironment
    {
        /// <summary>
        /// 共通ディレクトリの接尾辞
        /// </summary>
        public static string CommonDirSuffix { get; } = ConfigurationManager.AppSettings["CommonDirSuffix"];

        /// <summary>
        /// 共通ディレクトリの仮想パス
        /// </summary>
        public static string CommonVirtualPath { get; } = $"/Common{CommonDirSuffix}";

        /// <summary>
        /// ログディレクトリ
        /// </summary>
        public static string LogsDir { get; } = HostingEnvironment.MapPath($"{CommonVirtualPath}/Logs");

        /// <summary>
        /// 解析結果の基本ディレクトリ
        /// </summary>
        public static string ResultBaseDir { get; } = HostingEnvironment.MapPath($"{CommonVirtualPath}/Result");

        /// <summary>
        /// アップロードの基本ディレクトリ
        /// </summary>
        public static string UploadBaseDir { get; } = HostingEnvironment.MapPath($"{CommonVirtualPath}/Upload");

        /// <summary>
        /// 一時ファイルの基本ディレクトリ
        /// </summary>
        public static string TemporaryBaseDir { get; } = HostingEnvironment.MapPath($"{CommonVirtualPath}/Temporary");

        /// <summary>
        /// 接続文字列
        /// </summary>
        public static string ConnectionString { get; } = ConfigurationManager.ConnectionStrings["Prototype"].ConnectionString;

        /// <summary>
        /// 環境コード
        /// </summary>
        public static EnvironmentCode EnvironmentCode { get; } = (EnvironmentCode)Enum.Parse(typeof(EnvironmentCode), ConfigurationManager.AppSettings["EnvironmentCode"]);

        /// <summary>
        /// 最大要求サイズ
        /// </summary>
        public static int MaxRequestLength { get; } = (ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection).MaxRequestLength;
    }
}