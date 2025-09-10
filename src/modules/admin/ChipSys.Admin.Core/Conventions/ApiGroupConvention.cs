using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using ChipSys.DynamicApi.Attributes;

namespace ChipSys.Admin.Core.Conventions;

/// <summary>
/// Api����Լ��
/// </summary>
public class ApiGroupConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        if (controller.Attributes?.Count > 0)
        {
            foreach (var attribute in controller.Attributes)
            {
                if (attribute is AreaAttribute area)
                {
                    if (controller.ApiExplorer.GroupName.IsNull())
                    {
                        controller.ApiExplorer.GroupName = area.RouteValue?.ToLower();
                    }
                    break;
                }
                else if (attribute is DynamicApiAttribute dynamicApi)
                {
                    if (controller.ApiExplorer.GroupName.IsNull())
                    {
                        controller.ApiExplorer.GroupName = dynamicApi.Area?.ToLower();
                    }
                    break;
                }
            }
        }
    }
}
