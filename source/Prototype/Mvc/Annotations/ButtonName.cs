using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Prototype.Mvc.Annotations
{
    /// <summary>
    /// アクション メソッドの選択に影響する属性を表します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonNameAttribute : ActionMethodSelectorAttribute
    {
        /// <summary>
        /// ボタン名の一覧
        /// </summary>
        public string[] ButtonNames { get; private set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="buttonNames">ボタン名の一覧</param>
        public ButtonNameAttribute(params string[] buttonNames)
        {
            this.ButtonNames = buttonNames;
        }

        /// <summary>
        /// アクション メソッドの選択が、指定されたコントローラー コンテキストで有効かどうかを判断します。
        /// </summary>
        /// <param name="controllerContext">コントローラー コンテキスト</param>
        /// <param name="methodInfo">アクション メソッドに関する情報</param>
        /// <returns>アクション メソッドの選択が、指定されたコントローラー コンテキストで有効である場合は true。それ以外の場合は false。</returns>
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            var buttonName = controllerContext.Controller.ValueProvider.GetValue("Button")?.AttemptedValue as string;
            return this.ButtonNames.Any(x => x == buttonName);
        }
    }
}