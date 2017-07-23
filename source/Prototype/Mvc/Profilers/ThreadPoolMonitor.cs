using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Prototype.Mvc.Profilers
{
    /// <summary>
    /// スレッドプールを監視する機能を提供します。
    /// </summary>
    public class ThreadPoolMonitor
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static readonly Logger logger = LogManager.GetLogger("ThreadLogger");

        /// <summary>
        /// スレッドプールの監視が有効かどうかを示す値。
        /// </summary>
        private volatile bool enabled = true;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public ThreadPoolMonitor()
        {
            Task.Run(() => MonitoringProcess());
        }

        /// <summary>
        /// スレッドプールの監視を停止します。
        /// </summary>
        public void Stop()
        {
            this.enabled = false;
        }

        /// <summary>
        /// 監視処理。
        /// </summary>
        public void MonitoringProcess()
        {
            while (this.enabled)
            {
                OutputThreadStatus();
                Thread.Sleep(60000);
            }
        }

        /// <summary>
        /// スレッドプールの利用状況を出力します。
        /// </summary>
        public void OutputThreadStatus()
        {
            try
            {
                // ThreadPoolの利用状況を取得
                var now = DateTime.Now;
                int availableWorkerThreads = 0;
                int availableCompletionPortThreads = 0;
                int maxWorkerThreads = 0;
                int maxCompletionPortThreads = 0;
                ThreadPool.GetAvailableThreads(out availableWorkerThreads, out availableCompletionPortThreads);
                ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxCompletionPortThreads);

                // とりあえずTSV形式で蓄積型で記録
                logger.Trace(string.Join("\t", new object[]
                {
                    now,
                    availableWorkerThreads,
                    maxWorkerThreads,
                    availableCompletionPortThreads,
                    maxCompletionPortThreads,
                }));
            }
            catch
            {
                // 例外は無視する
            }
        }
    }
}