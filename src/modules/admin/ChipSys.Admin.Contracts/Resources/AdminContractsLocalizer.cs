using Microsoft.Extensions.Localization;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Resources;

namespace ChipSys.Admin.Contracts.Resources;

/// <summary>
/// Admin��Լ����ʻ�
/// </summary>
[InjectSingleton]
public class AdminContractsLocalizer : ModuleLocalizer
{
    public AdminContractsLocalizer(IStringLocalizer<AdminContractsLocalizer> localizer) : base(localizer)
    {
    }
}
