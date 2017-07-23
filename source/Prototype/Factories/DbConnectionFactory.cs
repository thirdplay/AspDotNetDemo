using Prototype.Mvc.Environment;
using Prototype.Mvc.Profilers;
using Oracle.ManagedDataAccess.Client;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System.Data.Common;

namespace Prototype.Factories
{
    /// <summary>
    /// DB接続を生成する機能を提供します。
    /// </summary>
    public static class DbConnectionFactory
    {
        /// <summary>
        /// DB接続を生成します。
        /// </summary>
        /// <returns>DB接続</returns>
        public static DbConnection Create()
        {
            return new ProfiledDbConnection(
                new OracleConnection(AppEnvironment.ConnectionString),
                new CompositeDbProfiler(MiniProfiler.Current, new TraceDbProfiler()));
        }
    }
}