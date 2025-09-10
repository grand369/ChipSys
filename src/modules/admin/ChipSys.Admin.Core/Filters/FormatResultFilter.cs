using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ChipSys.Admin.Core.Dto;
using ChipSys.DynamicApi.Attributes;

namespace ChipSys.Admin.Core.Filters;

/// <summary>
/// �����ʽ��������
/// </summary>
public class FormatResultFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var actionExecutedContext = await next();
        
        if (actionExecutedContext.Exception != null)
        {
            return;
        }

        if (context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType() == typeof(NonFormatResultAttribute)))
        {
            return;
        }

        IActionResult result = actionExecutedContext.Result;
        
        var formatResult = result switch
        {
            ViewResult => false,
            PartialViewResult => false,
            ViewComponentResult => false,
            PageResult => false,
            FileResult => false,
            SignInResult => false,
            SignOutResult => false,
            RedirectToPageResult => false,
            RedirectToRouteResult => false,
            RedirectResult => false,
            RedirectToActionResult => false,
            LocalRedirectResult => false,
            ChallengeResult => false,
            ForbidResult => false,
            BadRequestObjectResult => false,
            _ => true,
        };

        if (!formatResult)
        {
            return;
        }

        var data = result switch
        {
            ContentResult contentResult => contentResult.Content,
            ObjectResult objectResult => objectResult.Value,
            JsonResult jsonResult => jsonResult.Value,
            _ => null,
        };

        actionExecutedContext.Result = new JsonResult(new ResultOutput<dynamic>().Ok(data));
    }
}
