using Prototype.Services;
using Prototype.ViewModels;
using Microsoft.Practices.ServiceLocation;
using System.IO;
using System.Web.Mvc;

namespace Prototype.Controllers
{
    /// <summary>
    /// 共通のファイル処理を提供するコントローラー。
    /// </summary>
    public class FileController : ControllerBase
    {
        /// <summary>
        /// 業務サービス
        /// </summary>
        private readonly IBusinessService businessService;

        /// <summary>
        /// ファイルサービス
        /// </summary>
        private readonly IFileService fileService;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="serviceLocator">サービスロケーター</param>
        public FileController(IServiceLocator serviceLocator)
            : base(serviceLocator)
        {
            businessService = serviceLocator.GetInstance<IBusinessService>();
            fileService = serviceLocator.GetInstance<IFileService>();
        }

        /// <summary>
        /// ファイルアップロードアクション。
        /// </summary>
        /// <param name="model">ViewModel</param>
        /// <returns>アクション結果</returns>
        [HttpPost]
        public ActionResult UploadFile(FileViewModel model)
        {
            // ファイルが既にアップロード済みの場合、使いまわす
            if (!string.IsNullOrEmpty(model.FileName))
            {
                var fileInfo = businessService.CreateUploadFileInfo(HttpContext, model.FileName);
                if (fileInfo.Exists)
                {
                    return CreateSuccessResult(model.FileName);
                }
            }

            // アップロードされたファイルをサーバに保存する
            var fileName = fileService.SaveUploadedFileToServer(HttpContext, Request.Files[0], model.ScreenId, model.ItemId);
            return CreateSuccessResult(fileName);
        }
    }
}