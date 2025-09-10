namespace ChipSys.Admin.Services.Auth.Dto;

/// <summary>
/// ²éÑ¯ÃÜÔ¿
/// </summary>
public class AuthGetPasswordEncryptKeyOutput
{
    /// <summary>
    /// »º´æ¼ü
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// ÃÜÂë¼ÓÃÜÃÜÔ¿
    /// </summary>
    public string EncryptKey { get; set; }

    /// <summary>
    /// ÃÜÂë¼ÓÃÜÏòÁ¿
    /// </summary>
    public string Iv { get;  set; }
}
