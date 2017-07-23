using Prototype.ViewModels;
using System.Web.Mvc;

namespace Prototype.Mvc.Validations
{
    /// <summary>
    /// バリデーション機能を追加したモデルバインダー。
    /// </summary>
    public class ValidationModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// 指定されたコントローラー コンテキストおよびバインディング コンテキストを使用して、モデルを値にバインドします。
        /// </summary>
        /// <param name="controllerContext">コントローラー コンテキスト</param>
        /// <param name="bindingContext">バインディング コンテキスト</param>
        /// <returns>バインドされた値</returns>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext);

            // ViewModelBaseの場合、単項目チェックを行う
            var viewModel = model as ViewModelBase;
            if (viewModel != null)
            {
                foreach (var validationResult in viewModel.ValidateSingleItem(bindingContext))
                {
                    foreach (var memberName in validationResult.MemberNames)
                    {
                        if (!bindingContext.ModelState.ContainsKey(memberName))
                        {
                            bindingContext.ModelState.Add(memberName, new ModelState());
                        }
                        bindingContext.ModelState[memberName].Errors.Add(validationResult.ErrorMessage);
                    }
                }
            }

            return model;
        }
    }
}