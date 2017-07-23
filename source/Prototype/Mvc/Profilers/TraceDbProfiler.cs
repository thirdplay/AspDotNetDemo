using NLog;
using Oracle.ManagedDataAccess.Client;
using StackExchange.Profiling.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

namespace Prototype.Mvc.Profilers
{
    /// <summary>
    /// トレースDBプロファイラ。
    /// </summary>
    public class TraceDbProfiler : IDbProfiler
    {
        /// <summary>
        /// ストップウォッチ
        /// </summary>
		private Stopwatch stopwatch;

        /// <summary>
        /// コマンドテキスト
        /// </summary>
		private string commandText;

        /// <summary>
        /// パラメータ
        /// </summary>
		private string parameters;

        /// <summary>
        /// ロガー
        /// </summary>
        private static readonly Logger logger = LogManager.GetLogger(@"SqlLogger");

        #region IDbProfiler members

        /// <summary>
        /// アクティブ状態を表す値を取得します。
        /// </summary>
        public bool IsActive => true;

        /// <summary>
        /// コマンドが完了された時に呼ばれます。
        /// </summary>
        /// <param name="profiledDbCommand">SQLステートメントを表すインターフェイス</param>
        /// <param name="executeType">SQL文のカテゴリ</param>
        /// <param name="reader">取得結果セットを読み込むインターフェイス</param>
        public void ExecuteFinish(IDbCommand profiledDbCommand, SqlExecuteType executeType, DbDataReader reader)
        {
            this.commandText = GetCommandText(profiledDbCommand);
            this.parameters = string.Join(", ", profiledDbCommand.Parameters.Cast<OracleParameter>()
                .Select(x => $"{x.ParameterName}:{x.Value}"));
            this.parameters = "{" + this.parameters + "}";

            if (executeType != SqlExecuteType.Reader)
            {
                stopwatch.Stop();
                logger.Trace($"SqlExecute({stopwatch.ElapsedMilliseconds}):{commandText}");
                logger.Trace($"SqlParameters:{parameters}");
            }
        }

        /// <summary>
        /// コマンドが開始された時に呼ばれます。
        /// </summary>
        /// <param name="profiledDbCommand">SQLステートメントを表すインターフェイス</param>
        /// <param name="executeType">SQL文のカテゴリ</param>
        public void ExecuteStart(IDbCommand profiledDbCommand, SqlExecuteType executeType)
        {
            stopwatch = Stopwatch.StartNew();
        }

        /// <summary>
        /// エラー発生時に呼ばれます。
        /// </summary>
        /// <param name="profiledDbCommand">SQLステートメントを表すインターフェイス</param>
        /// <param name="executeType">SQL文のカテゴリ</param>
        /// <param name="exception">発生した例外</param>
        public void OnError(IDbCommand profiledDbCommand, SqlExecuteType executeType, Exception exception)
        {
            logger.Trace($"SqlError:{GetCommandText(profiledDbCommand)}");
            logger.Trace(exception);
        }

        /// <summary>
        /// Readerが完了した時に呼ばれます。
        /// </summary>
        /// <param name="reader">取得結果セットを読み込むインターフェイス</param>
        public void ReaderFinish(IDataReader reader)
        {
            stopwatch.Stop();
            logger.Trace($"SqlExecute({stopwatch.ElapsedMilliseconds:#,0}ms):{commandText}");
            logger.Trace($"SqlParameters:{parameters}");
        }

        #endregion IDbProfiler members

        /// <summary>
        /// コマンドテキストを取得します。
        /// </summary>
        /// <param name="profiledDbCommand">SQLステートメントを表すインターフェイス</param>
        /// <returns>コマンドテキスト</returns>
        private string GetCommandText(IDbCommand profiledDbCommand)
        {
            return string.Join(" ", profiledDbCommand.CommandText
                .Split('\n')
                .Select(x => x.Trim()))
                .Trim();
        }
    }
}