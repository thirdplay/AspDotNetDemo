using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prototype.Mvc.Extensions
{
    /// <summary>
    /// モデルバインディング状態の拡張機能を提供します。
    /// </summary>
    public static class ModelStateExtensions
    {
        /// <summary>
        /// モデルバインディング状態のエラーを取得します。
        /// </summary>
        /// <returns>エラーの列挙</returns>
        public static IEnumerable GetErrors(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                var errorModel = modelState.Keys
                    .Where(x => modelState[x].Errors.Count > 0)
                    .Select(x => new
                    {
                        key = x,
                        errors = modelState[x].Errors.Select(y => y.ErrorMessage).ToArray()
                    });
                return errorModel;
            }
            return Enumerable.Empty<IEnumerable>();
        }
    }
}