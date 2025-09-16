namespace ChipSys.Client.Contracts.Services.Client.Dto;

/// <summary>
/// 会员角色配置
/// </summary>
public static class ClientRoles
{
    /// <summary>
    /// 会员角色
    /// </summary>
    public const string Member = "Member";

    /// <summary>
    /// 供应商角色
    /// </summary>
    public const string Supplier = "Supplier";
}

/// <summary>
/// 会员权限配置
/// </summary>
public static class ClientRolePermissions
{
    /// <summary>
    /// 会员角色权限
    /// </summary>
    public static readonly Dictionary<string, string[]> MemberPermissions = new()
    {
        [ClientRoles.Member] = new[]
        {
            // 收藏管理权限
            ClientPermissions.MemberFavoriteView,
            ClientPermissions.MemberFavoriteAdd,
            ClientPermissions.MemberFavoriteUpdate,
            ClientPermissions.MemberFavoriteDelete,

            // 供求信息权限
            ClientPermissions.SupplyDemandView,
            ClientPermissions.SupplyDemandAdd,
            ClientPermissions.SupplyDemandUpdate,
            ClientPermissions.SupplyDemandDelete,
            ClientPermissions.SupplyDemandPublish,

            // 供应商申请权限
            ClientPermissions.SupplierApplicationView,
            ClientPermissions.SupplierApplicationSubmit,

            // 公开查询权限（通过PublicQueryService）
            "client.public.supplier.view",
            "client.public.product.view",
            "client.public.search"
        }
    };

    /// <summary>
    /// 供应商角色权限（在会员权限基础上增加）
    /// </summary>
    public static readonly Dictionary<string, string[]> SupplierPermissions = new()
    {
        [ClientRoles.Supplier] = MemberPermissions[ClientRoles.Member].Concat(new[]
        {
            // 供应商特有权限
            "client.supplier.product.manage",
            "client.supplier.inquiry.manage",
            "client.supplier.quote.manage"
        }).ToArray()
    };
}
