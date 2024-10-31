using Microsoft.AspNetCore.Mvc.Filters;

namespace ProductCatalog.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiResponseAttribute : ActionFilterAttribute
    {
        public bool Enabled { get; }
        public ApiResponseAttribute(bool enabled = true)
        {
            Enabled = enabled;
        }
    }
}
