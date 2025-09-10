using Microsoft.Extensions.Localization;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Resources;

/// <summary>
/// Admin¹ú¼Ê»¯
/// </summary>
[InjectSingleton]
public class AdminLocalizer: ModuleLocalizer
{
    public AdminLocalizer(IStringLocalizer<AdminLocalizer> localizer) : base(localizer)
    {
    }
}
