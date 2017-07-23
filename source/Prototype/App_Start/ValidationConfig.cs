using Prototype.Mvc.Validations;
using System.Web.Mvc;

namespace Prototype
{
    /// <summary>
    /// バリデーションの設定クラス。
    /// </summary>
    internal static class ValidationConfig
    {
        /// <summary>
        /// アダプターを登録します。
        /// </summary>
        public static void RegisterAdapter()
        {
            // モデルバインダーの設定
            ModelBinders.Binders.DefaultBinder = new ValidationModelBinder();
        }
    }
}