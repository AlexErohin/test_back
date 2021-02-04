using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace IdleMonitor.Infrastructure
{
    public class ModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            IModelBinder binder = null;

            if (context.Metadata.ModelType == typeof(Dictionary<string, object>))
                binder = new DictionaryModelBinder();

            if (context.Metadata.ModelType == typeof(List<Dictionary<string, object>>))
                binder = new ListDictionaryModelBinder();

            return binder;
        }
    }
}
