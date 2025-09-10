using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using ChipSys.Admin.Core.Enums;

namespace ChipSys.Admin.Core.Attributes;

/// <summary>
/// �Զ���·�� /api/{version}/[area]/[controler]/[action]
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class VersionRouteAttribute : RouteAttribute, IApiDescriptionGroupNameProvider
{
    public string GroupName { get; set; }

    public VersionRouteAttribute(ApiVersion version = ApiVersion.V2, string action = "[action]")
        : base($"/api/{version}/[area]/[controller]/{action}")
    {
        GroupName = version.ToString();
    }
}
