using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ChipSys.Admin.Core.Auth;
using ChipSys.Admin.Core.Configs;
using ChipSys.Admin.Core.Handlers;

namespace ChipSys.Admin.Core.Attributes;

/// <summary>
/// ����Ȩ����֤
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class ValidatePermissionAttribute : AuthorizeAttribute, IAuthorizationFilter, IAsyncAuthorizationFilter
{
    private async Task PermissionAuthorization(AuthorizationFilterContext context)
    {
        //�ų���������
        if (context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType() == typeof(AllowAnonymousAttribute)))
            return;

        var serviceProvider = context.HttpContext.RequestServices;
        //��¼��֤
        var user = serviceProvider.GetService<IUser>();
        if (user == null || !(user?.Id > 0))
        {
            context.Result = new ChallengeResult();
            return;
        }

        //�ų���¼�ӿ�
        if (context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType() == typeof(LoginAttribute)))
            return;

        if (user.PlatformAdmin)
        {
            return;
        }

        //�Զ���Ȩ����֤
        var customPermissionHandler = serviceProvider.GetService<ICustomPermissionHandler>();
        if (customPermissionHandler != null)
        {
            var isValid = await customPermissionHandler.ValidateAsync(context);
            if (!isValid)
            {
                return;
            }
        }

        //Ȩ����֤
        if (serviceProvider.GetRequiredService<AppConfig>().Validate.Permission)
        {
            var apiAccess = context.HttpContext.GetEndpoint()?.Metadata?.GetMetadata<ApiAccessAttribute>();
            
            var httpMethod = context.HttpContext.Request.Method;
            var api = context.ActionDescriptor.AttributeRouteInfo.Template;
            var permissionHandler = serviceProvider.GetService<IPermissionHandler>();
            var isValid = await permissionHandler.ValidateAsync(api, httpMethod, apiAccess);
            if (!isValid)
            {
                context.Result = new ForbidResult();
            }
        }
    }

    public async void OnAuthorization(AuthorizationFilterContext context)
    {
        await PermissionAuthorization(context);
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        await PermissionAuthorization(context);
    }
}
