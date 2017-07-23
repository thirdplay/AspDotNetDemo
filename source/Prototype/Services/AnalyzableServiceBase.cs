using Microsoft.Practices.ServiceLocation;

namespace Prototype.Services
{
    /// <summary>
    /// 解析サービスの基底クラス。
    /// </summary>
    public abstract class AnalyzableServiceBase : ServiceBase
    {
        /// <summary>
        /// 業務サービス
        /// </summary>
        protected IBusinessService BusinessService { get; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="serviceLocator">サービスロケーター</param>
        protected AnalyzableServiceBase(IServiceLocator serviceLocator)
            : base()
        {
            BusinessService = serviceLocator.GetInstance<IBusinessService>();
        }
    }
}