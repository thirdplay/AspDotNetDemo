using Prototype.Mvc.DI;
using Microsoft.Practices.ServiceLocation;
using System;
using System.IO;
using System.Web;

namespace Prototype.Services
{
    /// <summary>
    /// ファイル操作サービスのインターフェース。
    /// </summary>
    [Service(typeof(FileService))]
    public interface IFileService
    {
        /// <summary>
        /// クライアントからアップロードされたファイルをサーバに保存します。
        /// </summary>
        /// <param name="httpContext">HTTPコンテキスト</param>
        /// <param name="fileContext">アップロードされたファイル</param>
        /// <param name="screenId">画面ID</param>
        /// <param name="itemId">項目ID</param>
        /// <returns>ファイル名</returns>
        string SaveUploadedFileToServer(HttpContextBase httpContext, HttpPostedFileBase fileContext, string screenId, string itemId);
    }

    /// <summary>
    /// ファイルに関する情報、およびそれらを操作する手段を提供します。
    /// </summary>
    public class FileService : ServiceBase, IFileService
    {
        /// <summary>
        /// 業務サービス
        /// </summary>
        private readonly IBusinessService businessService;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="serviceLocator">サービスロケーター</param>
        public FileService(IServiceLocator serviceLocator)
            : base()
        {
            businessService = serviceLocator.GetInstance<IBusinessService>();
        }

        #region IFileService members

        /// <summary>
        /// クライアントからアップロードされたファイルをサーバに保存します。
        /// </summary>
        /// <param name="httpContext">HTTPコンテキスト</param>
        /// <param name="fileContext">アップロードされたファイル</param>
        /// <param name="screenId">画面ID</param>
        /// <param name="itemId">項目ID</param>
        /// <returns>ファイル名</returns>
        public string SaveUploadedFileToServer(HttpContextBase httpContext, HttpPostedFileBase fileContext, string screenId, string itemId)
        {
            if (fileContext == null)
            {
                throw new ArgumentException(nameof(fileContext));
            }
            var fileInfo = businessService.CreateUploadFileInfo(httpContext, screenId, itemId, Path.GetExtension(fileContext.FileName));
            using (var fileStream = File.Create(fileInfo.FullName))
            {
                fileContext.InputStream.CopyTo(fileStream);
            }

            return fileInfo.Name;
        }

        #endregion IFileService members
    }
}