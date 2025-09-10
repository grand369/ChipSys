using Mapster;
using ChipSys.Admin.Services.Role.Dto;

namespace ChipSys.Admin.Services.Role;

/// <summary>
/// ”≥…‰≈‰÷√
/// </summary>
public class MapConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
        .NewConfig<RoleGetOutput, RoleGetOutput>()
        .Map(dest => dest.OrgIds, src => src.Orgs.Select(a => a.Id));
    }
}
