using Microsoft.Extensions.Localization;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Resources;

/// <summary>
/// Admin���ʻ�
/// </summary>
[InjectSingleton]
public class AdminLocalizer: ModuleLocalizer
{
    public AdminLocalizer(IStringLocalizer<AdminLocalizer> localizer) : base(localizer)
    {
    }
}
