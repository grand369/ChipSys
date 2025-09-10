using ChipSys.Admin.Domain.Tenant;
using ChipSys.Admin.Domain.User;
using ChipSys.Admin.Domain.User.Dto;
using ChipSys.Admin.Services.User.Dto;

namespace ChipSys.Admin.Core.Auth;

/// <summary>
/// �û���Ϣ�ӿ�
/// </summary>
public interface IUser
{
    /// <summary>
    /// �û�Id
    /// </summary>
    long Id { get; }

    /// <summary>
    /// �û���
    /// </summary>
    string UserName { get; }

    /// <summary>
    /// ����
    /// </summary>
    string Name { get; }

    /// <summary>
    /// �û�����
    /// </summary>
    UserType Type { get; }

    /// <summary>
    /// Ĭ���û�
    /// </summary>
    bool DefaultUser { get; }

    /// <summary>
    /// ƽ̨����Ա
    /// </summary>
    bool PlatformAdmin { get; }

    /// <summary>
    /// �⻧����Ա
    /// </summary>
    bool TenantAdmin { get; }

    /// <summary>
    /// �⻧Id
    /// </summary>
    long? TenantId { get; }

    /// <summary>
    /// �⻧����
    /// </summary>
    TenantType? TenantType { get; }

    /// <summary>
    /// ���ݿ�ע���
    /// </summary>
    string DbKey { get; }

    /// <summary>
    /// ����Ȩ��
    /// </summary>
    DataPermissionOutput DataPermission { get; }

    /// <summary>
    /// �û�Ȩ��
    /// </summary>
    UserGetPermissionOutput UserPermission { get; }

    /// <summary>
    /// ����û��Ƿ�ӵ��ĳ��Ȩ�޵�
    /// </summary>
    /// <param name="permissionCode">Ȩ�޵����</param>
    /// <returns></returns>
    bool HasPermission(string permissionCode);

    /// <summary>
    /// ����û��Ƿ�ӵ����ЩȨ�޵�
    /// </summary>
    /// <param name="permissionCodes">Ȩ�޵�����б�</param>
    /// <param name="all">�Ƿ�ȫ������</param>
    /// <returns></returns>
    bool HasPermissions(string[] permissionCodes, bool all = false);
}
