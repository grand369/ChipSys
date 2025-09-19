using Microsoft.Extensions.Localization;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Resources;

namespace ChipSys.Admin.Core.Resources;

/// <summary>
/// Admin核心库国际化
/// </summary>
[InjectSingleton]
public class AdminCoreLocalizer: ModuleLocalizer
{
    public AdminCoreLocalizer(IStringLocalizer<AdminCoreLocalizer> localizer) : base(localizer)
    {
    }
}

