using Microsoft.Practices.ServiceLocation;
using System.Data.Common;

namespace Prototype.Repositories
{
    /// <summary>
    /// リポジトリの基底クラス。
    /// </summary>
    public interface IRepositoryBase
    {
    }

    /// <summary>
    /// リポジトリの基底クラス。
    /// </summary>
    public abstract class RepositoryBase : IRepositoryBase
    {
        /// <summary>
        /// サービスロケーター
        /// </summary>
        private readonly IServiceLocator serviceLocator;

        /// <summary>
        /// テーブル名
        /// </summary>
        protected string TableName { get; }

        /// <summary>
        /// DB接続を取得します。
        /// </summary>
        protected DbConnection Connection => serviceLocator.GetInstance<DbConnection>();

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="serviceLocator">サービスロケーター</param>
        /// <param name="tableName">テーブル名</param>
        protected RepositoryBase(IServiceLocator serviceLocator, string tableName)
        {
            this.serviceLocator = serviceLocator;
            this.TableName = tableName;
        }
    }
}