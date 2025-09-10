using Mapster;
using ChipSys.Admin.Domain.Permission;
using ChipSys.Admin.Services.Permission.Dto;

namespace ChipSys.Admin.Services.Admin.Permission;

/// <summary>
/// ӳ������
/// </summary>
public class MapConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
        .NewConfig<PermissionEntity, PermissionGetDotOutput>()
        .Map(dest => dest.ApiIds, src => src.Apis.Select(a => a.Id));
    }
}
