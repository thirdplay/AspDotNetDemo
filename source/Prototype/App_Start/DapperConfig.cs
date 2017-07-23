using Dapper;
using Prototype.Mvc.Extensions;
using System.Linq;
using System.Reflection;
using NLog;

namespace Prototype
{
    /// <summary>
    /// Dapperの設定クラス。
    /// </summary>
    internal class DapperConfig
    {
        /// <summary>
        /// 型マッピングを設定します。
        /// </summary>
        public static void RegisterMappings()
        {
            RegisterMappings("Prototype.Entities");
        }

        /// <summary>
        /// 型マッピングを設定します。
        /// </summary>
        /// <param name="namespace">名前空間</param>
        public static void RegisterMappings(string @namespace)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => x.IsClass && x.Namespace != null && x.Namespace.StartsWith(@namespace));
            foreach (var target in types)
            {
                SqlMapper.SetTypeMap(target, new CustomPropertyTypeMap(target, (type, columnName) =>
                    type.GetProperty(columnName.SnakeToPascal())));
            }
        }
    }
}