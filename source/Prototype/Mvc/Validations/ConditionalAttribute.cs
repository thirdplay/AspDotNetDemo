using Prototype.Mvc.Extensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace Prototype.Mvc.Validations
{
    /// <summary>
    /// 入力検証の条件を表す属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ConditionalAttribute : Attribute
    {
        /// <summary>
        /// 入力検証の条件を反転するかどうかを示す値。
        /// </summary>
        public bool Inverse { get; set; }

        /// <summary>
        /// 対象のプロパティ名。
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// 条件を満たす値。
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="propertyName">対象のプロパティ名</param>
        /// <param name="value">条件を満たす値</param>
        public ConditionalAttribute(string propertyName, object value)
        {
            this.PropertyName = propertyName;
            this.Value = value;
        }

        /// <summary>
        /// 入力検証が条件を満たすかどうかチェックします。
        /// </summary>
        /// <param name="validationContext">検証操作に関するコンテキスト情報</param>
        /// <returns>有効の場合はtrue。それ以外の場合はfalse。</returns>
        public bool IsMatch(ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance;
            var propertyValue = model.GetPropertyValue(PropertyName);

            if (Inverse)
            {
                return !object.Equals(propertyValue, Value);
            }
            return object.Equals(propertyValue, Value);
        }
    }
}