using System.Web.Http.Filters;

namespace ProductCatalog.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeGetwayAttribute : ActionFilterAttribute
    {
        public string[] Roles;
        public AuthorizeGetwayAttribute(params string[] Roles)
        {
            Roles = Roles;
        }


    }
}
