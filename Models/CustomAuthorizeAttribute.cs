using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class CustomAuthorizeAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.Session.GetString("UserCode");
        if (string.IsNullOrEmpty(user))
        {
            context.Result = new RedirectToActionResult("Login", "Auth", null);
        }
    }
}
