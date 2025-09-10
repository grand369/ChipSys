using System.Text.Json.Serialization;
using ChipSys.Admin.Domain.Org;
using ChipSys.Admin.Domain.Role;
using ChipSys.Admin.Domain.User;
using ChipSys.Admin.Domain.UserStaff;

namespace ChipSys.Admin.Services.User.Dto;

/// <summary>
/// ��ɾ���û���ҳ��ѯ��Ӧ
/// </summary>
public class UserGetDeletedUserPageOutput
{
    /// <summary>
    /// ����Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// �˺�
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// �ֻ���
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// �û�����
    /// </summary>
    public UserType Type { get; set; }

    [JsonIgnore]
    public ICollection<RoleEntity> Roles { get; set; }

    /// <summary>
    /// ��ɫ
    /// </summary>
    public string RoleNames => string.Join("��", Roles?.Select(a => a.Name)?.ToArray());

    /// <summary>
    /// ����
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// �Ա�
    /// </summary>
    public Sex? Sex { get; set; }

    /// <summary>
    /// ��������Id
    /// </summary>
    [JsonIgnore]
    public long OrgId { get; set; }

    /// <summary>
    /// ��������
    /// </summary>
    public string OrgPath { get; set; }

    /// <summary>
    /// �����б�
    /// </summary>
    [JsonIgnore]
    public ICollection<OrgEntity> Orgs { get; set; }

    /// <summary>
    /// ��������Id�б�
    /// </summary>
    [JsonIgnore]
    public long[] OrgIds => Orgs?.Select(a => a.Id)?.ToArray();

    /// <summary>
    /// ��������
    /// </summary>
    public string OrgPaths { get; set; }

    /// <summary>
    /// �������û���
    /// </summary>
    public string CreatedUserName { get; set; }

    /// <summary>
    /// ����������
    /// </summary>
    public string CreatedUserRealName { get; set; }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// �޸����û���
    /// </summary>
    public string ModifiedUserName { get; set; }

    /// <summary>
    /// �޸�������
    /// </summary>
    public string ModifiedUserRealName { get; set; }

    /// <summary>
    /// �޸�ʱ��
    /// </summary>
    public DateTime? ModifiedTime { get; set; }
}
