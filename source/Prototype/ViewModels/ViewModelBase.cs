using Prototype.Mvc.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Prototype.ViewModels
{
    /// <summary>
    /// ViewModelの基底クラス。
    /// </summary>
    [Serializable]
    public abstract class ViewModelBase : IValidatableObject
    {
        /// <summary>
        /// アノテーション検証コントロール
        /// </summary>
        [NonSerialized]
        private AnnotationsValidator validator;

        /// <summary>
        /// ボタン
        /// </summary>
        [NonSerialized]
        private string button;

        /// <summary>
        /// メッセージ
        /// </summary>
        [NonSerialized]
        private string message;

        /// <summary>
        /// セッションから読み込んだかどうかを示す値
        /// </summary>
        [NonSerialized]
        private bool isLoaded;

        /// <summary>
        /// ボタンを取得または設定します。
        /// </summary>
        public string Button
        {
            get { return button; }
            set { button = value; }
        }

        /// <summary>
        /// メッセージを取得または設定します。
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// セッションから読み込んだかどうかを示す値を取得または設定します。
        /// </summary>
        public bool IsLoaded
        {
            get { return isLoaded; }
            set { isLoaded = value; }
        }

        /// <summary>
        /// 単項目チェック時に、指定したオブジェクトが有効かどうかを判断します。
        /// </summary>
        /// <param name="bindingContext">バインディング コンテキスト</param>
        /// <returns>検証の失敗の情報を保持するコレクション</returns>
        public virtual IEnumerable<ValidationResult> ValidateSingleItem(ModelBindingContext bindingContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        /// <summary>
        /// 指定したオブジェクトが有効かどうかを判断します。
        /// </summary>
        /// <param name="validationContext">検証コンテキスト</param>
        /// <returns>検証の失敗の情報を保持するコレクション</returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        /// <summary>
        /// 検証エラーメッセージの一覧を取得します。
        /// </summary>
        /// <param name="validations">検証する検証属性</param>
        /// <param name="value">検証する値</param>
        /// <param name="displayName">検証する値の表示名</param>
        /// <param name="memberName">検証する値の項目名</param>
        /// <returns>エラーメッセージの一覧</returns>
        protected IEnumerable<ValidationResult> Validate(ValidationBaseAttribute[] validations, object value, string displayName, string memberName)
        {
            foreach (var validation in validations)
            {
                validation.ForceEnabled = true;
            }
            validator = validator ?? new AnnotationsValidator();
            return validator.Validate(validator, validations, value, displayName, memberName);
        }
    }
}