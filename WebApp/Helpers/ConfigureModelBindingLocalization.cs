using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebApp.Helpers
{
    /// <summary>
    /// Configuration of model binder localization
    /// </summary>
    public class ConfigureModelBindingLocalization: IConfigureOptions<MvcOptions>
    {
        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="options"></param>
        public void Configure(MvcOptions options)
        {
            options.ModelBindingMessageProvider.SetValueIsInvalidAccessor((value) => string.Format("Value {0} is invalid", value));
            
        }
    }
}