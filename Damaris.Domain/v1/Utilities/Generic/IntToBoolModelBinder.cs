using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Utilities.Generic
{
    /// <summary>
    /// Convert integer to bool value
    /// </summary>
    public class IntToBoolModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var value = valueProviderResult.FirstValue;

            if (int.TryParse(value, out var intValue))
            {
                bindingContext.Result = ModelBindingResult.Success(intValue == 0 ? false : true);

            }
            else if (bool.TryParse(value, out var boolValue))
            {
                bindingContext.Result = ModelBindingResult.Success(boolValue);

            }
            else if (string.IsNullOrWhiteSpace(value))
            {
                bindingContext.Result = ModelBindingResult.Success(false);

            }
            else
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName,
                    $"{bindingContext.ModelName} should be a int, bool or empty string.");
            }

            return Task.CompletedTask;
        }
    }
}