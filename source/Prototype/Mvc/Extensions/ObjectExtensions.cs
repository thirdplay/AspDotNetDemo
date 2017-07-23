namespace Prototype.Mvc.Extensions
{
    /// <summary>
    /// オブジェクトの拡張機能を提供します
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 指定されたプロパティ名の値を取得します。
        /// </summary>
        /// <param name="obj">対象オブジェクト</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <returns>プロパティの値</returns>
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(obj);
            }
            return null;
        }
    }
}