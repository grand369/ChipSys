using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Services;
using ChipSys.Admin.Resources;
using ChipSys.Admin.Repositories;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Client.Contracts.Services.Client.Dto;
using ChipSys.Client.Contracts.Domain.Client;
using ChipSys.Admin.Domain.Admin;
using ChipSys.Admin.Domain.User;
using ChipSys.Admin.Domain.Role;
using ChipSys.Admin.Domain.UserRole;
using FreeSql;
using System.Text.Json;

namespace ChipSys.Admin.Services.Admin;

/// <summary>
/// 管理员端供应商申请管理服务
/// </summary>
[Order(109)]
[DynamicApi(Area = "admin")]
public class SupplierApplicationManageService : BaseService, IDynamicApi
{
    private readonly AdminRepositoryBase<SupplierApplicationEntity> _supplierApplicationRep;
    private readonly AdminRepositoryBase<UserEntity> _userRep;
    private readonly AdminRepositoryBase<RoleEntity> _roleRep;
    private readonly AdminRepositoryBase<UserRoleEntity> _userRoleRep;
    private readonly AdminLocalizer _adminLocalizer;

    public SupplierApplicationManageService(
        AdminRepositoryBase<SupplierApplicationEntity> supplierApplicationRep,
        AdminRepositoryBase<UserEntity> userRep,
        AdminRepositoryBase<RoleEntity> roleRep,
        AdminRepositoryBase<UserRoleEntity> userRoleRep,
        AdminLocalizer adminLocalizer)
    {
        _supplierApplicationRep = supplierApplicationRep;
        _userRep = userRep;
        _userRoleRep = userRoleRep;
        _roleRep = roleRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 获取申请详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<SupplierApplicationGetOutput> GetAsync(long id)
    {
        var result = await _supplierApplicationRep.GetAsync<SupplierApplicationGetOutput>(id);
        if (result == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["申请记录不存在"]);
        }

        return result;
    }

    /// <summary>
    /// 获取申请分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<SupplierApplicationGetOutput>> GetPageAsync(PageInput<SupplierApplicationGetPageInput> input)
    {
        var memberId = input.Filter?.MemberId;
        var status = input.Filter?.Status;
        var companyName = input.Filter?.CompanyName;

        var list = await _supplierApplicationRep.Select
            .WhereDynamicFilter(input.DynamicFilter)
            .WhereIf(memberId.HasValue, a => a.MemberId == memberId)
            .WhereIf(status.NotNull(), a => a.Status == status)
            .WhereIf(companyName.NotNull(), a => a.CompanyName.Contains(companyName))
            .Count(out var total)
            .OrderByDescending(a => a.CreatedTime)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<SupplierApplicationGetOutput>();

        return new PageOutput<SupplierApplicationGetOutput>
        {
            List = list,
            Total = total
        };
    }

    /// <summary>
    /// 审核供应商申请
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public async Task ReviewAsync(SupplierApplicationReviewInput input)
    {
        var application = await _supplierApplicationRep.GetAsync(input.Id);
        if (application == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["申请记录不存在"]);
        }

        if (application.Status != "pending")
        {
            throw ResultOutput.Exception(_adminLocalizer["该申请已审核"]);
        }

        // 更新申请状态
        application.Status = input.Status;
        application.ReviewComment = input.ReviewComment;
        application.ReviewerId = User.Id;
        application.ReviewTime = DateTime.Now;

        await _supplierApplicationRep.UpdateAsync(application);

        // 如果审核通过，更新用户角色
        if (input.Status == "approved")
        {
            await UpdateUserRole(application.MemberId);
        }

        // 发送站内信通知
        await SendNotification(application, input.Status);
    }

    /// <summary>
    /// 更新用户角色
    /// </summary>
    /// <param name="memberId"></param>
    /// <returns></returns>
    private async Task UpdateUserRole(long memberId)
    {
        // 获取用户信息
        var user = await _userRep.GetAsync(memberId);
        if (user == null) return;

        // 获取供应商角色
        var supplierRole = await _roleRep.Select
            .Where(a => a.Name == "Supplier")
            .FirstAsync();

        if (supplierRole == null) return;

        // 检查是否已有供应商角色
        var existingUserRole = await _userRoleRep.Select
            .Where(a => a.UserId == memberId && a.RoleId == supplierRole.Id)
            .FirstAsync();

        if (existingUserRole == null)
        {
            // 添加供应商角色
            var userRole = new UserRoleEntity
            {
                UserId = memberId,
                RoleId = supplierRole.Id,
                CreatedTime = DateTime.Now
            };

            await _userRoleRep.InsertAsync(userRole);
        }
    }

    /// <summary>
    /// 发送站内信通知
    /// </summary>
    /// <param name="application"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    private async Task SendNotification(SupplierApplicationEntity application, string status)
    {
        // 这里应该实现站内信发送逻辑
        // 为了演示，这里只是记录日志
        var message = status == "approved" 
            ? $"恭喜！您的供应商申请已通过审核，现在您可以使用供应商功能了。"
            : $"很抱歉，您的供应商申请未通过审核。原因：{application.ReviewComment}";

        // 实际实现中，这里应该调用站内信服务
        Console.WriteLine($"发送站内信给用户 {application.MemberId}: {message}");
    }
}
