using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace kurs.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            var request = routeContext.HttpContext.Request;
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
