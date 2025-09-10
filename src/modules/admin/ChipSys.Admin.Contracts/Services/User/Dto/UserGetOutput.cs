using System.Text.Json.Serialization;
using ChipSys.Admin.Domain.Role;

namespace ChipSys.Admin.Services.User.Dto;

/// <summary>
/// �û�
/// </summary>
public class UserGetOutput : UserUpdateInput
{
    /// <summary>
    /// ��ɫ�б�
    /// </summary>
    [JsonIgnore]
    public ICollection<RoleEntity> Roles { get; set; }

    /// <summary>
    /// ��ɫId�б�
    /// </summary>
    public override long[] RoleIds => Roles?.Select(a => a.Id)?.ToArray();
}
