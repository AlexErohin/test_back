using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IdleMonitor.Infrastructure
{
    public class DictionaryModelBinder : Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder
    {
        public Task BindModelAsync(Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException("bindingContext");

            var buffer = new byte[Convert.ToInt32(bindingContext.ActionContext.HttpContext.Request.ContentLength)];
            bindingContext.ActionContext.HttpContext.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            var body = Encoding.UTF8.GetString(buffer);
            Dictionary<string, object> result = JSONSerializer.Deserialize<Dictionary<string, object>>(body);
            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }

    public class ListDictionaryModelBinder : Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder
    {
        public Task BindModelAsync(Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException("bindingContext");

            var buffer = new byte[Convert.ToInt32(bindingContext.ActionContext.HttpContext.Request.ContentLength)];
            bindingContext.ActionContext.HttpContext.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            var body = Encoding.UTF8.GetString(buffer);
            List<Dictionary<string, object>> result = JSONSerializer.Deserialize<List<Dictionary<string, object>>>(body);
            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }
}
