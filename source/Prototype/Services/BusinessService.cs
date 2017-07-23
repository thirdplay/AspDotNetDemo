using Prototype.Constants;
using Prototype.Mvc.DI;
using Prototype.Mvc.Environment;
using Prototype.Mvc.Extensions;
using Microsoft.Practices.ServiceLocation;
using System;
using System.IO;
using System.Web;

namespace Prototype.Services
{
    /// <summary>
    /// 業務サービスのインターフェース。
    /// </summary>
    [Service(typeof(BusinessService))]
    public interface IBusinessService
    {
        /// <summary>
        /// アップロードファイルのファイル情報を作成します。
        /// </summary>
        /// <param name="httpContext">HTTPコンテキスト</param>
        /// <param name="screenId">画面ID</param>
        /// <param name="itemId">項目ID</param>
        /// <param name="extension">ファイル名の拡張子</param>
        /// <returns>ファイル情報</returns>
        FileInfo CreateUploadFileInfo(HttpContextBase httpContext, string screenId, string itemId, string extension);

        /// <summary>
        /// アップロードファイルのファイル情報を作成します。
        /// </summary>
        /// <param name="httpContext">HTTPコンテキスト</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns>ファイル情報</returns>
        FileInfo CreateUploadFileInfo(HttpContextBase httpContext, string fileName);

        /// <summary>
        /// 解析結果ファイルのファイル情報を作成します。
        /// </summary>
        /// <param name="httpContext">HTTPコンテキスト</param>
        /// <param name="format">ファイル形式</param>
        /// <returns>ファイル情報</returns>
        FileInfo CreateResultFileInfo(HttpContextBase httpContext, SupportedExcelFormat format = SupportedExcelFormat.Xlsx);

        /// <summary>
        /// 一時ファイルのファイル情報を作成します。
        /// </summary>
        /// <param name="httpContext">HTTPコンテキスト</param>
        /// <param name="format">ファイル形式</param>
        /// <returns>ファイル情報</returns>
        FileInfo CreateTemporaryFileInfo(HttpContextBase httpContext, SupportedExcelFormat format = SupportedExcelFormat.Xlsx);
    }

    /// <summary>
    /// 業務に関する情報、およびそれらを操作する手段を提供します。
    /// </summary>
    public class BusinessService : ServiceBase, IBusinessService
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="serviceLocator">サービスロケーター</param>
        public BusinessService(IServiceLocator serviceLocator)
            : base()
        {
        }

        #region IBusinessService members

        /// <summary>
        /// アップロードファイルのファイル情報を作成します。
        /// </summary>
        /// <param name="httpContext">HTTPコンテキスト</param>
        /// <param name="screenId">画面ID</param>
        /// <param name="itemId">項目ID</param>
        /// <param name="extension">ファイル名の拡張子</param>
        /// <returns>ファイル情報</returns>
        public FileInfo CreateUploadFileInfo(HttpContextBase httpContext, string screenId, string itemId, string extension)
        {
            var uploadDir = GetUploadDir(httpContext);
            var fileName = CreateUploadFileName(screenId, itemId, extension);
            return new FileInfo(Path.Combine(uploadDir, fileName));
        }

        /// <summary>
        /// アップロードファイルのファイル情報を作成します。
        /// </summary>
        /// <param name="httpContext">HTTPコンテキスト</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns>ファイル情報</returns>
        public FileInfo CreateUploadFileInfo(HttpContextBase httpContext, string fileName)
        {
            var uploadDir = GetUploadDir(httpContext);
            return new FileInfo(Path.Combine(uploadDir, fileName));
        }

        /// <summary>
        /// 解析結果ファイルのファイル情報を作成します。
        /// </summary>
        /// <param name="httpContext">HTTPコンテキスト</param>
        /// <param name="format">ファイル形式</param>
        /// <returns>ファイル情報</returns>
        public FileInfo CreateResultFileInfo(HttpContextBase httpContext, SupportedExcelFormat format = SupportedExcelFormat.Xlsx)
        {
            var resultDir = GetResultDir(httpContext);
            var fileName = CreateResultFileName(httpContext, format);
            return new FileInfo(Path.Combine(resultDir, fileName));
        }

        /// <summary>
        /// 一時ファイルのファイル情報を作成します。
        /// </summary>
        /// <param name="httpContext">HTTPコンテキスト</param>
        /// <param name="format">ファイル形式</param>
        /// <returns>ファイル情報</returns>
        public FileInfo CreateTemporaryFileInfo(HttpContextBase httpContext, SupportedExcelFormat format = SupportedExcelFormat.Xlsx)
        {
            var temporaryDir = GetTemporaryDir(httpContext);
            var fileName = CreateTemporaryFileName(httpContext, format);
            return new FileInfo(Path.Combine(temporaryDir, fileName));
        }

        #endregion IBusinessService members

        /// <summary>
        /// アップロード先のディレクトリを取得します。
        /// </summary>
        /// <param name="httpContext">HTTPコンテキスト</param>
        /// <returns>アップロード先ディレクトリ</returns>
        /// <remarks>ディレクトリが存在しない場合、新規にディレクトリを作成します。</remarks>
        private string GetUploadDir(HttpContextBase httpContext)
        {
            var userId = httpContext.Session.GetUserId();
            var dir = Path.Combine(AppEnvironment.UploadBaseDir, userId);

            CreateDirectory(dir);
            return dir;
        }

        /// <summary>
        /// 一時ディレクトリを取得します。
        /// </summary>
        /// <param name="httpContext">HTTPコンテキスト</param>
        /// <returns>一時ディレクトリ</returns>
        /// <remarks>一時ディレクトリが存在しない場合、新規にディレクトリを作成します。</remarks>
        private string GetTemporaryDir(HttpContextBase httpContext)
        {
            var userId = httpContext.Session.GetUserId();
            var dir = Path.Combine(AppEnvironment.TemporaryBaseDir, userId);

            CreateDirectory(dir);
            return dir;
        }

        /// <summary>
        /// 解析結果の出力先ディレクトリを取得します。
        /// </summary>
        /// <param name="httpContext">HTTPコンテキスト</param>
        /// <returns>出力先ディレクトリ</returns>
        /// <remarks>出力ディレクトリが存在しない場合、新規にディレクトリを作成します。</remarks>
        private string GetResultDir(HttpContextBase httpContext)
        {
            var userId = httpContext.Session.GetUserId();
            var dir = Path.Combine(AppEnvironment.ResultBaseDir, userId);

            CreateDirectory(dir);
            return dir;
        }

        /// <summary>
        /// アップロードファイルのファイル名を作成します。
        /// </summary>
        /// <param name="screenId">画面ID</param>
        /// <param name="itemId">項目ID</param>
        /// <param name="extension">ファイル名の拡張子</param>
        /// <returns>ファイル名</returns>
        private string CreateUploadFileName(string screenId, string itemId, string extension)
        {
            var date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return Path.Combine($"{screenId}_{itemId}_{date}{extension}");
        }

        /// <summary>
        /// 一時ファイルのファイル名を作成します。
        /// </summary>
        /// <param name="httpContext">HTTPコンテキスト</param>
        /// <param name="format">ファイル形式</param>
        /// <returns>ファイル名</returns>
        private string CreateTemporaryFileName(HttpContextBase httpContext, SupportedExcelFormat format = SupportedExcelFormat.Xlsx)
        {
            var date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var screenId = httpContext.GetScreenId();

            return Path.Combine($"{screenId}_{date}{format.GetExtension()}");
        }

        /// <summary>
        /// 解析結果ファイルのファイル名を作成します。
        /// </summary>
        /// <param name="httpContext">HTTPコンテキスト</param>
        /// <param name="format">ファイル形式</param>
        /// <returns>ファイル名</returns>
        private string CreateResultFileName(HttpContextBase httpContext, SupportedExcelFormat format = SupportedExcelFormat.Xlsx)
        {
            var date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var screenId = httpContext.GetScreenId();

            return Path.Combine($"{screenId}_{date}{format.GetExtension()}");
        }

        /// <summary>
        /// 登録CSVファイルのファイル名を作成します。
        /// </summary>
        /// <param name="httpContext">HTTPコンテキスト</param>
        /// <returns>ファイル名</returns>
        private string CreateRegistrationCsvFileName(HttpContextBase httpContext)
        {
            var date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var screenId = httpContext.GetScreenId();

            return Path.Combine($"{screenId}_{date}.csv");
        }

        /// <summary>
        /// 指定されたディレクトリが存在しない場合、ディレクトリを作成します。
        /// </summary>
        /// <param name="dir">ディレクトリ</param>
        private void CreateDirectory(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}