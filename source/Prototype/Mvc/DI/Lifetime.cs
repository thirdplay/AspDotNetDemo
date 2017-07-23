using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prototype.Mvc.DI
{
    /// <summary>
    /// DIコンテナによってインスタンスの生存期間を表します。
    /// </summary>
    public enum Lifetime
    {
        /// <summary>
        /// 常に同じインスタンス。（シングルトン）
        /// </summary>
        Singleton,

        /// <summary>
        /// スレッド毎に生成。
        /// </summary>
        Thread,

        /// <summary>
        /// リクエスト毎に生成。
        /// </summary>
        Request,

        /// <summary>
        /// 常に生成。
        /// </summary>
        Resolve,
    }

    /// <summary>
    /// ライフタイムの拡張機能を提供します。
    /// </summary>
    public static class LifetimeExtensions
    {
        /// <summary>
        /// ライフタイムに対応する <see cref="LifetimeManager"/> を生成します。
        /// </summary>
        /// <param name="lifetime">ライフタイム</param>
        /// <returns>LifetimeManagerのインスタンス</returns>
        public static LifetimeManager CreateLifetimeManager(this Lifetime lifetime)
        {
            switch (lifetime)
            {
                case Lifetime.Singleton:
                    return new ContainerControlledLifetimeManager();

                case Lifetime.Thread:
                    return new PerThreadLifetimeManager();

                case Lifetime.Request:
                    return new PerRequestLifetimeManager();

                case Lifetime.Resolve:
                    return new PerResolveLifetimeManager();
            }
            throw new ArgumentException();
        }
    }
}