using Microsoft.Extensions.Localization;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Resources;

namespace ChipSys.Admin.Contracts.Resources;

/// <summary>
/// Admin契约库国际化
/// </summary>
[InjectSingleton]
public class AdminContractsLocalizer : ModuleLocalizer
{
    public AdminContractsLocalizer(IStringLocalizer<AdminContractsLocalizer> localizer) : base(localizer)
    {
    }
}
