using System;
using System.Runtime.Serialization;
using System.Security;

namespace Prototype.Mvc.Exceptions
{
    /// <summary>
    /// アプリケーション実行中に発生するチェックエラーを表します。 この例外の内容はログに出力されません。
    /// </summary>
    [Serializable]
    public class CheckException : Exception
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public CheckException() : base()
        {
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="message">エラーを説明するメッセージ</param>
        public CheckException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="message">エラーを説明するメッセージ</param>
        /// <param name="innerException">現在の例外の原因である例外</param>
        public CheckException(string message, Exception innerException)
            : base(message)
        {
        }

        /// <summary>
        /// シリアル化されたデータを使用して <see cref="Exception"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="info">スローされる例外に関するシリアル化されたオブジェクトデータを保持する情報</param>
        /// <param name="context">ソースまたは宛先に関するコンテキスト情報</param>
        [SecuritySafeCritical]
        protected CheckException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}