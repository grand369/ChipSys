using Mapster;
using ChipSys.Admin.Services.Tenant.Dto;

namespace ChipSys.Admin.Services.Tenant;

/// <summary>
/// ”≥…‰≈‰÷√
/// </summary>
public class MapConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
        .NewConfig<TenantGetPageOutput, TenantGetPageOutput>()
        .Map(dest => dest.PkgNames, src => src.Pkgs.Select(a => a.Name));
    }
}
