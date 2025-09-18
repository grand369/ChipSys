using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Services;
using ChipSys.Admin.Resources;
using ChipSys.Admin.Repositories;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Admin.Domain.Admin;
using FreeSql;

namespace ChipSys.Admin.Services.Admin;

/// <summary>
/// 支付配置管理服务
/// </summary>
[Order(111)]
[DynamicApi(Area = "admin")]
public class PaymentConfigService : BaseService, IDynamicApi
{
    private readonly AdminRepositoryBase<PaymentConfigEntity> _paymentConfigRep;
    private readonly AdminLocalizer _adminLocalizer;

    public PaymentConfigService(
        AdminRepositoryBase<PaymentConfigEntity> paymentConfigRep,
        AdminLocalizer adminLocalizer)
    {
        _paymentConfigRep = paymentConfigRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 获取支付配置列表
    /// </summary>
    /// <returns></returns>
    public async Task<List<PaymentConfigEntity>> GetConfigsAsync()
    {
        return await _paymentConfigRep.Select
            .OrderBy(a => a.PaymentMethod)
            .ToListAsync();
    }

    /// <summary>
    /// 获取支付配置详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PaymentConfigEntity> GetConfigAsync(long id)
    {
        var config = await _paymentConfigRep.GetAsync(id);
        if (config == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["支付配置不存在"]);
        }

        return config;
    }

    /// <summary>
    /// 添加支付配置
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public async Task<long> AddConfigAsync(PaymentConfigEntity config)
    {
        // 检查是否已存在相同支付方式的配置
        var existing = await _paymentConfigRep.Select
            .Where(a => a.PaymentMethod == config.PaymentMethod && a.Enabled)
            .FirstAsync();

        if (existing != null)
        {
            throw ResultOutput.Exception(_adminLocalizer["该支付方式已存在配置"]);
        }

        config.CreatedTime = DateTime.Now;
        await _paymentConfigRep.InsertAsync(config);
        return config.Id;
    }

    /// <summary>
    /// 更新支付配置
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public async Task UpdateConfigAsync(PaymentConfigEntity config)
    {
        var existing = await _paymentConfigRep.GetAsync(config.Id);
        if (existing == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["支付配置不存在"]);
        }

        Mapper.Map(config, existing);
        await _paymentConfigRep.UpdateAsync(existing);
    }

    /// <summary>
    /// 删除支付配置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteConfigAsync(long id)
    {
        var config = await _paymentConfigRep.GetAsync(id);
        if (config == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["支付配置不存在"]);
        }

        await _paymentConfigRep.DeleteAsync(id);
    }

    /// <summary>
    /// 启用/禁用支付配置
    /// </summary>
    /// <param name="id"></param>
    /// <param name="enabled"></param>
    /// <returns></returns>
    public async Task ToggleConfigAsync(long id, bool enabled)
    {
        var config = await _paymentConfigRep.GetAsync(id);
        if (config == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["支付配置不存在"]);
        }

        config.Enabled = enabled;
        await _paymentConfigRep.UpdateAsync(config);
    }

    /// <summary>
    /// 测试支付配置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> TestConfigAsync(long id)
    {
        var config = await _paymentConfigRep.GetAsync(id);
        if (config == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["支付配置不存在"]);
        }

        try
        {
            // 这里可以调用支付API进行测试
            // 为了演示，简单返回true
            return true;
        }
        catch
        {
            return false;
        }
    }
}
