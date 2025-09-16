using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Services;
using ChipSys.Admin.Resources;
using ChipSys.Admin.Repositories;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Supplier;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Product;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Client.Contracts.Services.Client;
using ChipSys.Client.Contracts.Domain.Client;
using ChipSys.Client.Contracts.Services.Client.Dto;
using FreeSql;
using System.Linq;
using System.Collections.Generic;

namespace ChipSys.Admin.Services.Client;

/// <summary>
/// 公开查询服务（供会员查看公开信息）
/// </summary>
[Order(103)]
[DynamicApi(Area = "client")]
public class PublicQueryService : BaseService, IPublicQueryService, IDynamicApi
{
    private readonly AdminRepositoryBase<SupplierEntity> _supplierRep;
    private readonly AdminRepositoryBase<ProductEntity> _productRep;
    private readonly AdminRepositoryBase<MemberLevelEntity> _memberLevelRep;
    private readonly AdminLocalizer _adminLocalizer;

    public PublicQueryService(
        AdminRepositoryBase<SupplierEntity> supplierRep,
        AdminRepositoryBase<ProductEntity> productRep,
        AdminRepositoryBase<MemberLevelEntity> memberLevelRep,
        AdminLocalizer adminLocalizer)
    {
        _supplierRep = supplierRep;
        _productRep = productRep;
        _memberLevelRep = memberLevelRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询公开的供应商列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<object>> GetPublicSuppliersAsync(object input)
    {
        // 获取会员权限限制
        var memberLimits = await GetMemberLimitsAsync();
        
        var list = await _supplierRep.Select
            .Where(a => a.Status == 1) // 只显示激活的供应商
            .Take(memberLimits.SupplierDataLimit) // 根据会员等级限制数据量
            .ToListAsync();

        // 根据会员等级处理联系人信息
        var result = list.Select(s => new
        {
            s.Id,
            s.CompanyName,
            s.BusinessScope,
            s.Address,
            ContactName = memberLimits.ShowFullContactInfo ? s.ContactName : "****",
            ContactPhone = memberLimits.ShowFullContactInfo ? s.ContactPhone : "****",
            ContactEmail = memberLimits.ShowFullContactInfo ? s.ContactEmail : "****",
            s.CreatedTime
        }).ToList();

        return new PageOutput<object>
        {
            List = result.Cast<object>().ToList(),
            Total = result.Count
        };
    }

    /// <summary>
    /// 查询公开的供应商详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<object> GetPublicSupplierAsync(long id)
    {
        var supplier = await _supplierRep.GetAsync(id);
        if (supplier == null || supplier.Status != 1)
        {
            throw ResultOutput.Exception(_adminLocalizer["供应商不存在或已停用"]);
        }

        // 获取会员权限限制
        var memberLimits = await GetMemberLimitsAsync();

        return new
        {
            supplier.Id,
            supplier.CompanyName,
            supplier.BusinessScope,
            supplier.Address,
            supplier.Description,
            ContactName = memberLimits.ShowFullContactInfo ? supplier.ContactName : "****",
            ContactPhone = memberLimits.ShowFullContactInfo ? supplier.ContactPhone : "****",
            ContactEmail = memberLimits.ShowFullContactInfo ? supplier.ContactEmail : "****",
            supplier.CreatedTime
        };
    }

    /// <summary>
    /// 查询公开的产品列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<object>> GetPublicProductsAsync(object input)
    {
        // 获取会员权限限制
        var memberLimits = await GetMemberLimitsAsync();
        
        var list = await _productRep.Select
            .Where(a => a.Status == 1) // 只显示激活的产品
            .Take(memberLimits.ProductDataLimit) // 根据会员等级限制数据量
            .ToListAsync();

        var result = list.Select(p => new
        {
            p.Id,
            p.Name,
            p.Category,
            p.Description,
            p.Specification,
            p.Unit,
            p.Price,
            p.SupplierId,
            p.CreatedTime
        }).ToList();

        return new PageOutput<object>
        {
            List = result.Cast<object>().ToList(),
            Total = result.Count
        };
    }

    /// <summary>
    /// 查询公开的产品详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<object> GetPublicProductAsync(long id)
    {
        var product = await _productRep.GetAsync(id);
        if (product == null || product.Status != 1)
        {
            throw ResultOutput.Exception(_adminLocalizer["产品不存在或已停用"]);
        }

        return new
        {
            product.Id,
            product.Name,
            product.Category,
            product.Description,
            product.Specification,
            product.Unit,
            product.Price,
            product.SupplierId,
            product.CreatedTime
        };
    }

    /// <summary>
    /// 查询供应商的产品列表
    /// </summary>
    /// <param name="supplierId"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<object>> GetSupplierProductsAsync(long supplierId, object input)
    {
        // 验证供应商是否存在且激活
        var supplier = await _supplierRep.GetAsync(supplierId);
        if (supplier == null || supplier.Status != 1)
        {
            throw ResultOutput.Exception(_adminLocalizer["供应商不存在或已停用"]);
        }

        // 获取会员权限限制
        var memberLimits = await GetMemberLimitsAsync();
        
        var list = await _productRep.Select
            .Where(a => a.SupplierId == supplierId && a.Status == 1)
            .Take(memberLimits.ProductDataLimit)
            .ToListAsync();

        var result = list.Select(p => new
        {
            p.Id,
            p.Name,
            p.Category,
            p.Description,
            p.Specification,
            p.Unit,
            p.Price,
            p.SupplierId,
            p.CreatedTime
        }).ToList();

        return new PageOutput<object>
        {
            List = result.Cast<object>().ToList(),
            Total = result.Count
        };
    }

    /// <summary>
    /// 搜索供应商和产品
    /// </summary>
    /// <param name="keyword"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public async Task<object> SearchAsync(string keyword, string type = "all")
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return new { Suppliers = new List<object>(), Products = new List<object>() };
        }

        // 获取会员权限限制
        var memberLimits = await GetMemberLimitsAsync();
        
        var suppliers = new List<object>();
        var products = new List<object>();

        // 搜索供应商
        if (type == "all" || type == "supplier")
        {
            var supplierList = await _supplierRep.Select
                .Where(a => a.Status == 1 && 
                           (a.CompanyName.Contains(keyword) || a.BusinessScope.Contains(keyword)))
                .Take(memberLimits.SupplierDataLimit)
                .ToListAsync();

            suppliers.AddRange(supplierList.Select(s => new
            {
                s.Id,
                s.CompanyName,
                s.BusinessScope,
                s.Address,
                ContactName = memberLimits.ShowFullContactInfo ? s.ContactName : "****",
                ContactPhone = memberLimits.ShowFullContactInfo ? s.ContactPhone : "****",
                ContactEmail = memberLimits.ShowFullContactInfo ? s.ContactEmail : "****",
                s.CreatedTime
            }));
        }

        // 搜索产品
        if (type == "all" || type == "product")
        {
            var productList = await _productRep.Select
                .Where(a => a.Status == 1 && 
                           (a.Name.Contains(keyword) || a.Description.Contains(keyword) || a.Category.Name.Contains(keyword)))
                .Take(memberLimits.ProductDataLimit)
                .ToListAsync();

            products.AddRange(productList.Select(p => new
            {
                p.Id,
                p.Name,
                p.Category,
                p.Description,
                p.Specification,
                p.Unit,
                p.Price,
                p.SupplierId,
                p.CreatedTime
            }));
        }

        return new { Suppliers = suppliers, Products = products };
    }

    /// <summary>
    /// 获取会员权限限制
    /// </summary>
    /// <returns></returns>
    private async Task<(int SupplierDataLimit, int ProductDataLimit, bool ShowFullContactInfo)> GetMemberLimitsAsync()
    {
        var memberLevel = await _memberLevelRep.Select
            .Where(a => a.MemberId == User.Id && a.Enabled)
            .OrderByDescending(a => a.CreatedTime)
            .FirstAsync();

        if (memberLevel == null)
        {
            // 默认免费会员权限
            return (50, 50, false);
        }

        return (memberLevel.SupplierDataLimit, memberLevel.ProductDataLimit, memberLevel.ShowFullContactInfo);
    }
}
