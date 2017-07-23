using Prototype.Constants;
using System.Web;

namespace Prototype.Mvc.Extensions
{
    /// <summary>
    /// セッションへのアクセス機能を拡張します。
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// ユーザIDを取得します。
        /// </summary>
        /// <param name="session">セッション</param>
        /// <returns>ユーザID</returns>
        public static string GetUserId(this HttpSessionStateBase session)
        {
            return session[SessionKey.UserId] as string ?? "";
        }

        /// <summary>
        /// ユーザIDを設定します。
        /// </summary>
        /// <param name="session">セッション</param>
        /// <param name="userId">ユーザID</param>
        public static void SetUserId(this HttpSessionStateBase session, string userId)
        {
            session[SessionKey.UserId] = userId;
        }
    }
}