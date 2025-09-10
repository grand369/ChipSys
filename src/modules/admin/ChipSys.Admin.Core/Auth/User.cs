using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ChipSys.Common.Extensions;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Tools.Cache;
using ChipSys.Admin.Domain.User;
using ChipSys.Admin.Domain.Tenant;
using ChipSys.Admin.Domain.User.Dto;
using ChipSys.Admin.Services.User.Dto;

namespace ChipSys.Admin.Core.Auth;

/// <summary>
/// �û���Ϣ
/// </summary>
public class User : IUser
{
    private readonly IHttpContextAccessor _accessor;

    public User(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    /// <summary>
    /// �û�Id
    /// </summary>
    public virtual long Id
    {
        get
        {
            var id = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.UserId);
            if (id != null && id.Value.NotNull())
            {
                return id.Value.ToLong();
            }
            return 0;
        }
    }

    /// <summary>
    /// �û���
    /// </summary>
    public string UserName
    {
        get
        {
            var name = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.UserName);

            if (name != null && name.Value.NotNull())
            {
                return name.Value;
            }

            return "";
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public string Name
    {
        get
        {
            var name = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.Name);

            if (name != null && name.Value.NotNull())
            {
                return name.Value;
            }

            return "";
        }
    }

    /// <summary>
    /// �⻧Id
    /// </summary>
    public virtual long? TenantId
    {
        get
        {
            var tenantId = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.TenantId);
            if (tenantId != null && tenantId.Value.NotNull())
            {
                return tenantId.Value.ToLong();
            }
            return null;
        }
    }

    /// <summary>
    /// �û�����
    /// </summary>
    public virtual UserType Type
    {
        get
        {
            var userType = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.UserType);
            if (userType != null && userType.Value.NotNull())
            {
                return (UserType)Enum.Parse(typeof(UserType), userType.Value, true);
            }
            return UserType.DefaultUser;
        }
    }

    /// <summary>
    /// Ĭ���û�
    /// </summary>
    public virtual bool DefaultUser
    {
        get
        {
            return Type == UserType.DefaultUser;
        }
    }


    /// <summary>
    /// ƽ̨����Ա
    /// </summary>
    public virtual bool PlatformAdmin
    {
        get
        {
            return Type == UserType.PlatformAdmin;
        }
    }

    /// <summary>
    /// �⻧����Ա
    /// </summary>
    public virtual bool TenantAdmin
    {
        get
        {
            return Type == UserType.TenantAdmin;
        }
    }

    /// <summary>
    /// �⻧����
    /// </summary>
    public virtual TenantType? TenantType
    {
        get
        {
            var tenantType = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.TenantType);
            if (tenantType != null && tenantType.Value.NotNull())
            {
                return (TenantType)Enum.Parse(typeof(TenantType), tenantType.Value, true);
            }
            return null;
        }
    }

    /// <summary>
    /// ���ݿ�ע���
    /// </summary>
    public virtual string DbKey
    {
        get
        {
            var dbKey = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.DbKey);
            if (dbKey != null && dbKey.Value.NotNull())
            {
                return dbKey.Value;
            }
            return "";
        }
    }

    /// <summary>
    /// �������Ȩ��
    /// </summary>
    /// <returns></returns>
    DataPermissionOutput GetDataPermission()
    {
        var cache = _accessor?.HttpContext?.RequestServices.GetRequiredService<ICacheTool>();
        if (cache == null)
        {
            return null;
        }
        else
        {
            return cache.Get<DataPermissionOutput>(CacheKeys.GetDataPermissionKey(Id));
        }
    }

    /// <summary>
    /// ����Ȩ��
    /// </summary>
    public virtual DataPermissionOutput DataPermission => GetDataPermission();

    /// <summary>
    /// ����û�Ȩ��
    /// </summary>
    /// <returns></returns>
    UserGetPermissionOutput GetUserPermission()
    {
        var cache = _accessor?.HttpContext?.RequestServices.GetRequiredService<ICacheTool>();
        if (cache == null)
        {
            return null;
        }
        else
        {
            return cache.Get<UserGetPermissionOutput>(CacheKeys.GetUserPermissionKey(Id));
        }
    }

    /// <summary>
    /// �û�Ȩ��
    /// </summary>
    public virtual UserGetPermissionOutput UserPermission => GetUserPermission();

    /// <summary>
    /// ����û��Ƿ�ӵ��ĳ��Ȩ�޵�
    /// </summary>
    /// <param name="permissionCode">Ȩ�޵����</param>
    /// <returns></returns>
    public virtual bool HasPermission(string permissionCode)
    {
        ArgumentNullException.ThrowIfNull(permissionCode, nameof(permissionCode));

        return HasPermissions([permissionCode]);
    }

    /// <summary>
    /// ����û��Ƿ�ӵ����ЩȨ�޵�
    /// </summary>
    /// <param name="permissionCodes">Ȩ�޵�����б�</param>
    /// <param name="all">�Ƿ�ȫ������</param>
    /// <returns></returns>
    public virtual bool HasPermissions(string[] permissionCodes, bool all = false)
    {
        ArgumentNullException.ThrowIfNull(permissionCodes, nameof(permissionCodes));

        if (PlatformAdmin)
        {
            return true;
        }

        var valid = false;
        if (all)
        {
            valid = UserPermission.Codes.All(a => permissionCodes.Contains(a));
        }
        else
        {
            valid = UserPermission.Codes.Any(a => permissionCodes.Contains(a));
        }

        return valid;
    }
}
