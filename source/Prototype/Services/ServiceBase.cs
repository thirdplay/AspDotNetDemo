using NLog;
using System;

namespace Prototype.Services
{
    /// <summary>
    /// サービスの基底クラス。
    /// </summary>
    public abstract class ServiceBase : IDisposable
    {
        /// <summary>
        /// ロガー
        /// </summary>
        protected Logger Logger { get; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        protected ServiceBase()
        {
            Logger = LogManager.GetLogger(this.GetType().FullName);
        }

        #region IDispose members

        /// <summary>
        /// このインスタンスによって使用されているリソースを全て破棄します。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// このインスタンスによって使用されているリソースを全て破棄します。
        /// </summary>
        /// <param name="disposing">呼び出し元がDisposeメソッドかどうかを示す値</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        #endregion IDispose members
    }
}