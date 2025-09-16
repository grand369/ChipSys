using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Services;
using ChipSys.Admin.Resources;
using ChipSys.Admin.Repositories;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Supplier;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Product;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Client.Contracts.Domain.Client;
using ChipSys.Client.Contracts.Services.Client;
using ChipSys.Client.Contracts.Services.Client.Dto;
using FreeSql;
using System.Linq;
using System.Collections.Generic;

namespace ChipSys.Admin.Services.Client;

/// <summary>
/// 会员仪表板服务
/// </summary>
[Order(104)]
[DynamicApi(Area = "client")]
public class MemberDashboardService : BaseService, IMemberDashboardService, IDynamicApi
{
    private readonly AdminRepositoryBase<MemberFavoriteEntity> _memberFavoriteRep;
    private readonly AdminRepositoryBase<SupplyDemandEntity> _supplyDemandRep;
    private readonly AdminRepositoryBase<SupplierApplicationEntity> _supplierApplicationRep;
    private readonly AdminRepositoryBase<SupplierEntity> _supplierRep;
    private readonly AdminRepositoryBase<ProductEntity> _productRep;
    private readonly AdminLocalizer _adminLocalizer;

    public MemberDashboardService(
        AdminRepositoryBase<MemberFavoriteEntity> memberFavoriteRep,
        AdminRepositoryBase<SupplyDemandEntity> supplyDemandRep,
        AdminRepositoryBase<SupplierApplicationEntity> supplierApplicationRep,
        AdminRepositoryBase<SupplierEntity> supplierRep,
        AdminRepositoryBase<ProductEntity> productRep,
        AdminLocalizer adminLocalizer)
    {
        _memberFavoriteRep = memberFavoriteRep;
        _supplyDemandRep = supplyDemandRep;
        _supplierApplicationRep = supplierApplicationRep;
        _supplierRep = supplierRep;
        _productRep = productRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 获取会员仪表板统计数据
    /// </summary>
    /// <returns></returns>
    public async Task<MemberDashboardStatsOutput> GetDashboardStatsAsync()
    {
        var memberId = User.Id;

        // 统计收藏数量
        var favoriteSupplierCount = await _memberFavoriteRep.Select
            .Where(a => a.MemberId == memberId && a.FavoriteType == "Supplier")
            .CountAsync();

        var favoriteProductCount = await _memberFavoriteRep.Select
            .Where(a => a.MemberId == memberId && a.FavoriteType == "Product")
            .CountAsync();

        var favoriteContactCount = await _memberFavoriteRep.Select
            .Where(a => a.MemberId == memberId && a.FavoriteType == "Contact")
            .CountAsync();

        // 统计供求信息数量
        var supplyInfoCount = await _supplyDemandRep.Select
            .Where(a => a.MemberId == memberId && a.InfoType == "Supply")
            .CountAsync();

        var demandInfoCount = await _supplyDemandRep.Select
            .Where(a => a.MemberId == memberId && a.InfoType == "Demand")
            .CountAsync();

        // 获取供应商申请状态
        var supplierApplication = await _supplierApplicationRep.Select
            .Where(a => a.MemberId == memberId)
            .OrderByDescending(a => a.CreatedTime)
            .FirstAsync();

        // 获取最近的收藏
        var recentFavoriteSuppliers = await _memberFavoriteRep.Select
            .Where(a => a.MemberId == memberId && a.FavoriteType == "Supplier")
            .OrderByDescending(a => a.CreatedTime)
            .Take(5)
            .ToListAsync();

        var recentFavoriteProducts = await _memberFavoriteRep.Select
            .Where(a => a.MemberId == memberId && a.FavoriteType == "Product")
            .OrderByDescending(a => a.CreatedTime)
            .Take(5)
            .ToListAsync();

        // 获取最近的供求信息
        var recentSupplyDemands = await _supplyDemandRep.Select
            .Where(a => a.MemberId == memberId)
            .OrderByDescending(a => a.CreatedTime)
            .Take(5)
            .ToListAsync();

        return new MemberDashboardStatsOutput
        {
            FavoriteSupplierCount = (int)favoriteSupplierCount,
            FavoriteProductCount = (int)favoriteProductCount,
            FavoriteContactCount = (int)favoriteContactCount,
            SupplyInfoCount = (int)supplyInfoCount,
            DemandInfoCount = (int)demandInfoCount,
            SupplierApplicationStatus = supplierApplication?.Status,
            RecentFavoriteSuppliers = recentFavoriteSuppliers.Select(f => new RecentFavoriteOutput
            {
                Id = f.Id,
                FavoriteId = f.FavoriteId,
                FavoriteName = f.FavoriteName,
                Name = f.FavoriteName,
                Type = f.FavoriteType,
                CreatedTime = f.CreatedTime ?? DateTime.Now
            }).ToList(),
            RecentFavoriteProducts = recentFavoriteProducts.Select(f => new RecentFavoriteOutput
            {
                Id = f.Id,
                FavoriteId = f.FavoriteId,
                FavoriteName = f.FavoriteName,
                Name = f.FavoriteName,
                Type = f.FavoriteType,
                CreatedTime = f.CreatedTime ?? DateTime.Now
            }).ToList(),
            RecentSupplyDemands = recentSupplyDemands.Select(s => new RecentSupplyDemandOutput
            {
                Id = s.Id,
                Title = s.Title ?? s.ProductName,
                InfoType = s.InfoType,
                ProductName = s.ProductName,
                Status = s.Status,
                CreatedTime = s.CreatedTime ?? DateTime.Now
            }).ToList()
        };
    }

    /// <summary>
    /// 获取会员收藏统计
    /// </summary>
    /// <returns></returns>
    public async Task<object> GetFavoriteStatsAsync()
    {
        var memberId = User.Id;

        // 统计总收藏数
        var total = await _memberFavoriteRep.Select
            .Where(a => a.MemberId == memberId)
            .CountAsync();

        // 按类型统计
        var byType = new Dictionary<string, int>();
        var types = new[] { "Supplier", "Product", "Contact" };

        foreach (var type in types)
        {
            var count = await _memberFavoriteRep.Select
                .Where(a => a.MemberId == memberId && a.FavoriteType == type)
                .CountAsync();
            byType[type] = (int)count;
        }

        return new { Total = total, ByType = byType };
    }

    /// <summary>
    /// 获取会员供求信息统计
    /// </summary>
    /// <returns></returns>
    public async Task<object> GetSupplyDemandStatsAsync()
    {
        var memberId = User.Id;

        // 统计总供求信息数
        var total = await _supplyDemandRep.Select
            .Where(a => a.MemberId == memberId)
            .CountAsync();

        // 按类型和状态统计
        var byTypeAndStatus = new Dictionary<string, Dictionary<string, int>>();
        var types = new[] { "Supply", "Demand", "Inquiry" };
        var statuses = new[] { "Draft", "Published", "Closed" };

        foreach (var type in types)
        {
            byTypeAndStatus[type] = new Dictionary<string, int>();
            foreach (var status in statuses)
            {
                var count = await _supplyDemandRep.Select
                    .Where(a => a.MemberId == memberId && a.InfoType == type && a.Status == status)
                    .CountAsync();
                byTypeAndStatus[type][status] = (int)count;
            }
        }

        return new { Total = total, ByTypeAndStatus = byTypeAndStatus };
    }
}
