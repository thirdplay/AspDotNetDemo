using Prototype.Mvc.Environment;
using Prototype.Mvc.Extensions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototype.Mvc.Dapper
{
    /// <summary>
    /// Dapperの拡張機能を提供します。
    /// </summary>
    public static class DapperExtensions
    {
        #region SqlClause

        /// <summary>
        /// SQL条件を表します。
        /// </summary>
        private class SqlClause
        {
            /// <summary>
            /// SQLクエリ
            /// </summary>
            public string Sql { get; set; }

            /// <summary>
            /// SQLパラメーター
            /// </summary>
            public DynamicParameters Parameters { get; set; }
        }

        #endregion SqlClause

        /// <summary>
        /// IN句の最大数
        /// </summary>
        private const int InPharseListMax = 1000;

        /// <summary>
        /// Where句にIn句を追加します。
        /// </summary>
        /// <typeparam name="TSqlBuilder">SQLビルダーを継承したクラス</typeparam>
        /// <typeparam name="TParam">パラメータの型</typeparam>
        /// <param name="sqlBuilder">SQLビルダー</param>
        /// <param name="columnName">カラム名</param>
        /// <param name="parameterName">パラメータ名</param>
        /// <param name="searchList">IN句の検索リスト</param>
        /// <returns>SQLビルダー</returns>
        public static TSqlBuilder WhereIn<TSqlBuilder, TParam>(this TSqlBuilder sqlBuilder, string columnName, string parameterName, IEnumerable<TParam> searchList)
            where TSqlBuilder : SqlBuilder
        {
            var result = CreateInPharse(columnName, parameterName, searchList);
            sqlBuilder.Where(result.Sql, result.Parameters);
            return sqlBuilder;
        }

        /// <summary>
        /// Where句にOr条件のIn句を追加します。
        /// </summary>
        /// <typeparam name="TSqlBuilder">SQLビルダーを継承したクラス</typeparam>
        /// <typeparam name="TParam">パラメータの型</typeparam>
        /// <param name="sqlBuilder">SQLビルダー</param>
        /// <param name="columnName">カラム名</param>
        /// <param name="parameterName">パラメータ名</param>
        /// <param name="searchList">IN句の検索リスト</param>
        /// <returns>SQLビルダー</returns>
        public static TSqlBuilder OrWhereIn<TSqlBuilder, TParam>(this TSqlBuilder sqlBuilder, string columnName, string parameterName, IEnumerable<TParam> searchList)
            where TSqlBuilder : SqlBuilder
        {
            var result = CreateInPharse(columnName, parameterName, searchList);
            sqlBuilder.OrWhere(result.Sql, result.Parameters);
            return sqlBuilder;
        }

        /// <summary>
        /// Having句にIn句を追加します。
        /// </summary>
        /// <typeparam name="TSqlBuilder">SQLビルダーを継承したクラス</typeparam>
        /// <typeparam name="TParam">パラメータの型</typeparam>
        /// <param name="sqlBuilder">SQLビルダー</param>
        /// <param name="columnName">カラム名</param>
        /// <param name="parameterName">パラメータ名</param>
        /// <param name="searchList">IN句の検索リスト</param>
        /// <returns>SQLビルダー</returns>
        public static TSqlBuilder HavingIn<TSqlBuilder, TParam>(this TSqlBuilder sqlBuilder, string columnName, string parameterName, IEnumerable<TParam> searchList)
            where TSqlBuilder : SqlBuilder
        {
            var result = CreateInPharse(columnName, parameterName, searchList);
            sqlBuilder.Having(result.Sql, result.Parameters);
            return sqlBuilder;
        }

        /// <summary>
        /// StartWith句にIn句を追加します。
        /// </summary>
        /// <typeparam name="TSqlBuilder">SQLビルダーを継承したクラス</typeparam>
        /// <typeparam name="TParam">パラメータの型</typeparam>
        /// <param name="sqlBuilder">SQLビルダー</param>
        /// <param name="columnName">カラム名</param>
        /// <param name="parameterName">パラメータ名</param>
        /// <param name="searchList">IN句の検索リスト</param>
        /// <returns>SQLビルダー</returns>
        public static TSqlBuilder StartWithIn<TSqlBuilder, TParam>(this TSqlBuilder sqlBuilder, string columnName, string parameterName, IEnumerable<TParam> searchList)
            where TSqlBuilder : SqlBuilderEx
        {
            var result = CreateInPharse(columnName, parameterName, searchList);
            sqlBuilder.StartWith(result.Sql, result.Parameters);
            return sqlBuilder;
        }

        /// <summary>
        /// 動的なパラメータを追加します。
        /// </summary>
        /// <typeparam name="TSqlBuilder">SQLビルダーを継承したクラス</typeparam>
        /// <param name="sqlBuilder">SQLビルダー</param>
        /// <param name="name">パラメータ名</param>
        /// <param name="value">パラメータ値</param>
        /// <returns>SQLビルダー</returns>
        public static TSqlBuilder AddParameter<TSqlBuilder>(this TSqlBuilder sqlBuilder, string name, object value)
            where TSqlBuilder : SqlBuilder
        {
            var param = new DynamicParameters();
            param.Add(name, value);
            sqlBuilder.AddParameters(param);
            return sqlBuilder;
        }

        /// <summary>
        /// 指定された条件を満たす場合、指定メソッドを実行します。
        /// </summary>
        /// <param name="sqlBuilder">SQLビルダー</param>
        /// <param name="condition">メソッドを実行するかどうかを判定するメソッド</param>
        /// <param name="action">実行するメソッド</param>
        /// <returns>SQLビルダー</returns>
        public static TSqlBuilder Conditional<TSqlBuilder>(this TSqlBuilder sqlBuilder, Func<bool> condition, Action<TSqlBuilder> action)
            where TSqlBuilder : SqlBuilder
        {
            if (condition())
            {
                action(sqlBuilder);
            }
            return sqlBuilder;
        }

        /// <summary>
        /// 検索リストの制限を考慮したIn句を作成します。
        /// </summary>
        /// <typeparam name="T">パラメータの型</typeparam>
        /// <param name="columnName">カラム名</param>
        /// <param name="parameterName">パラメータ名</param>
        /// <param name="searchList">IN句の検索リスト</param>
        /// <returns>SQL条件</returns>
        private static SqlClause CreateInPharse<T>(string columnName, string parameterName, IEnumerable<T> searchList)
        {
            var sql = "";
            var parameters = new DynamicParameters();

            if (searchList.Count() < InPharseListMax)
            {
                // 検索リストが1000件以下の場合、そのまま追加
                sql = $"{columnName} in :{parameterName}";
                parameters.Add(parameterName, searchList);
            }
            else
            {
                // 検索リストが1000件以上ある場合、1000件ごとに分割して条件を作成する
                var sqlList = new List<string>();
                foreach (var param in searchList.Chunk(InPharseListMax).Select((x, i) => new { Value = x, Index = i }))
                {
                    sqlList.Add($"{columnName} in:{parameterName + param.Index}");
                    parameters.Add($"{parameterName + param.Index}", param.Value);
                }
                sql = $"({string.Join(" or ", sqlList)})";
            }

            return new SqlClause()
            {
                Sql = sql,
                Parameters = parameters
            };
        }
    }
}