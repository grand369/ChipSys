using ChipSys.Admin.Core.Dto;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChipSys.Client.Contracts.Services.Client.Dto;

namespace ChipSys.Client.Contracts.Services.Client;

/// <summary>
/// 会员等级服务接口
/// </summary>
public interface IMemberLevelService
{
    /// <summary>
    /// 查询会员等级
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<MemberLevelGetOutput> GetAsync(long id);

    /// <summary>
    /// 查询会员等级分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    Task<PageOutput<MemberLevelGetPageOutput>> GetPageAsync(PageInput<MemberLevelGetPageInput> input);

    /// <summary>
    /// 获取会员当前等级
    /// </summary>
    /// <param name="memberId"></param>
    /// <returns></returns>
    Task<MemberLevelGetOutput> GetCurrentLevelAsync(long memberId);

    /// <summary>
    /// 升级会员等级
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    Task<long> UpgradeLevelAsync(MemberLevelAddInput input);

    /// <summary>
    /// 更新会员等级
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    Task UpdateLevelAsync(MemberLevelUpdateInput input);

    /// <summary>
    /// 检查会员权限
    /// </summary>
    /// <param name="memberId"></param>
    /// <param name="permissionType"></param>
    /// <returns></returns>
    Task<bool> CheckPermissionAsync(long memberId, string permissionType);
}
