using Prototype.Constants;
using System;

namespace Prototype.Mvc.DI
{
    /// <summary>
    /// DIコンテナによって依存性を注入するサービスに使用される属性を表します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class ServiceAttribute : ComponentAttribute
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="targetType">インターフェイスに対して注入する型</param>
        public ServiceAttribute(Type targetType) : base(targetType, Lifetime.Singleton)
        {
        }
    }
}