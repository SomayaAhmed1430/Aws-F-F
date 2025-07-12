using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aws_F_F.Filters
{
    public class AdminOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isAdmin = context.HttpContext.Session.GetString("IsAdmin");
            if (string.IsNullOrEmpty(isAdmin))
            {
                context.Result = new RedirectToActionResult("Admin", "Home", null);
            }
        }
    }
}
