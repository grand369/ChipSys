using System.Text.Json.Serialization;
using ChipSys.Admin.Domain.Pkg;

namespace ChipSys.Admin.Services.Tenant.Dto;

public class TenantGetOutput : TenantUpdateInput
{
    /// <summary>
    /// �ײ��б�
    /// </summary>
    [JsonIgnore]
    public ICollection<PkgEntity> Pkgs { get; set; }

    /// <summary>
    /// �ײ�Id�б�
    /// </summary>
    public override long[] PkgIds => Pkgs?.Select(a => a.Id)?.ToArray();
}
