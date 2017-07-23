using Dapper;
using System.Runtime.CompilerServices;

namespace Prototype.Mvc.Dapper
{
    /// <summary>
    /// SQLビルダーの拡張機能を提供します。
    /// </summary>
    public class SqlBuilderEx : SqlBuilder
    {
        /// <summary>
        /// StartWith句に条件を追加します。
        /// </summary>
        /// <param name="sql">SQLクエリ</param>
        /// <param name="parameters">パラメータ</param>
        /// <returns>SQLビルダー</returns>
        public SqlBuilderEx StartWith(string sql, dynamic parameters = null)
        {
            AddClause("startwith", sql, parameters, " and ", "start with ", "\n", false);
            return this;
        }

        /// <summary>
        /// 拡張Where句に条件を追加します。
        /// </summary>
        /// <param name="sql">SQLクエリ</param>
        /// <param name="parameters">パラメータ</param>
        /// <returns>SQLビルダー</returns>
        public SqlBuilderEx WhereEx(string sql, dynamic parameters = null)
        {
            AddClause("whereex", sql, parameters, " and ", "where ", "\n", false);
            return this;
        }
    }
}