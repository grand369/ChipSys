namespace ChipSys.Admin.Services.Auth.Dto;

/// <summary>
/// �û���Ϣ
/// </summary>
public class AuthGetUserInfoOutput
{
    /// <summary>
    /// �û�������Ϣ
    /// </summary>
    public AuthUserProfileOutput User { get; set; }

    /// <summary>
    /// �û��˵��б�
    /// </summary>
    public List<AuthUserMenuOutput> Menus { get; set; }

    /// <summary>
    /// �û�Ȩ���б�
    /// </summary>
    public List<string> Permissions { get; set; }
}
