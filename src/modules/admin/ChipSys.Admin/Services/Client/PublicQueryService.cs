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
    private readonly AdminRepositoryBase<ProductSupplierEntity> _productSupplierRep;
    private readonly AdminRepositoryBase<ContactEntity> _contactRep;
    private readonly AdminRepositoryBase<MemberLevelEntity> _memberLevelRep;
    private readonly AdminRepositoryBase<MemberFavoriteEntity> _memberFavoriteRep;
    private readonly AdminLocalizer _adminLocalizer;

    public PublicQueryService(
        AdminRepositoryBase<SupplierEntity> supplierRep,
        AdminRepositoryBase<ProductEntity> productRep,
        AdminRepositoryBase<ProductSupplierEntity> productSupplierRep,
        AdminRepositoryBase<ContactEntity> contactRep,
        AdminRepositoryBase<MemberLevelEntity> memberLevelRep,
        AdminRepositoryBase<MemberFavoriteEntity> memberFavoriteRep,
        AdminLocalizer adminLocalizer)
    {
        _supplierRep = supplierRep;
        _productRep = productRep;
        _productSupplierRep = productSupplierRep;
        _contactRep = contactRep;
        _memberLevelRep = memberLevelRep;
        _memberFavoriteRep = memberFavoriteRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询公开的供应商列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<object>> GetPublicSuppliersAsync(PageInput input)
    {
        // 获取会员权限限制
        var memberLimits = await GetMemberLimitsAsync();
        
        // 设置分页参数
        var pageSize = Math.Min(input.PageSize, 50); // 每页最多50条
        var currentPage = input.CurrentPage;

        // 使用 FreeSql 单表查询 + 子查询判断收藏，避免方言 SQL
        var result = _supplierRep.Select
            .Where(s => s.Status == 1)
            .Count(out var total)
            .OrderByDescending(s => s.CreatedTime)
            .Page(currentPage, Math.Min(pageSize, memberLimits.SupplierDataLimit))
            .ToList(s => new
            {
                s.Id,
                s.CompanyName,
                s.BusinessScope,
                s.Address,
                s.ContactName,
                s.ContactPhone,
                s.ContactEmail,
                IsFavorited = _memberFavoriteRep.Orm.Select<MemberFavoriteEntity>()
                    .Where(a => a.MemberId == User.Id && a.FavoriteType == "Supplier" && a.FavoriteId == s.Id)
                    .Any(),
                s.CreatedTime
            });

        // 后处理：根据权限限制处理联系人信息
        var processedResult = result.Select(item => new
        {
            item.Id,
            item.CompanyName,
            item.BusinessScope,
            item.Address,
            ContactName = memberLimits.ShowFullContactInfo ? item.ContactName : "****",
            ContactPhone = memberLimits.ShowFullContactInfo ? item.ContactPhone : "****",
            ContactEmail = memberLimits.ShowFullContactInfo ? item.ContactEmail : "****",
            item.IsFavorited,
            item.CreatedTime
        }).ToList();

        return new PageOutput<object>
        {
            List = processedResult.Cast<object>().ToList(),
            Total = total
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
    /// 查询公开的产品列表（从供应商产品表查询）
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<object>> GetPublicProductsAsync(PageInput input)
    {
        // 获取会员权限限制
        var memberLimits = await GetMemberLimitsAsync();
        
        // 设置分页参数
        var pageSize = Math.Min(input.PageSize, 50); // 每页最多50条
        var currentPage = input.CurrentPage;

        // 使用 FreeSql 单表（带 Include）+ 子查询判断收藏
        var result = _productSupplierRep.Select
            .Include(ps => ps.Product)
            .Include(ps => ps.Supplier)
            .Where(ps => ps.Status == 1 && ps.Product.Status == 1 && ps.Supplier.Status == 1)
            .Count(out var total)
            .OrderByDescending(ps => ps.CreatedTime)
            .Page(currentPage, Math.Min(pageSize, memberLimits.ProductDataLimit))
            .ToList(ps => new
            {
                Id = ps.Product.Id,
                ProductName = ps.Product.Name,
                Category = ps.Product.Category,
                Description = ps.Product.Description,
                Specification = ps.Product.Specification,
                Unit = ps.Product.Unit,
                Price = ps.CurrentPrice,
                Currency = ps.Currency,
                Condition = ps.Condition,
                MOQ = ps.MOQ,
                StockQty = ps.StockQty,
                LeadTimeDays = ps.LeadTimeDays,
                SupplierId = ps.Supplier.Id,
                SupplierName = ps.Supplier.CompanyName ?? ps.Supplier.Name,
                SupplierCode = ps.Supplier.Code,
                SupplierModel = ps.SupplierModel,
                SupplierProductCode = ps.SupplierProductCode,
                SupplierProductName = ps.SupplierProductName,
                IsFavorited = _memberFavoriteRep.Orm.Select<MemberFavoriteEntity>()
                    .Where(a => a.MemberId == User.Id && a.FavoriteType == "Product" && a.FavoriteId == ps.Product.Id)
                    .Any(),
                ps.CreatedTime
            });

        return new PageOutput<object>
        {
            List = result.Cast<object>().ToList(),
            Total = total
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
    /// <param name="currentPage"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<object> SearchAsync(string keyword, string type = "all", int currentPage = 1, int pageSize = 20)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return new { Suppliers = new List<object>(), Products = new List<object>() };
        }

        // 获取会员权限限制
        var memberLimits = await GetMemberLimitsAsync();
        
        // 设置分页参数
        var pageSizeLimit = Math.Min(pageSize, 50); // 每页最多50条
        var offset = (currentPage - 1) * pageSizeLimit;
        
        var suppliers = new List<object>();
        var products = new List<object>();

        // 搜索供应商（FreeSql 关联 + 分页）
        if (type == "all" || type == "supplier")
        {
            var supplierList = _supplierRep.Select
                .Where(s => s.Status == 1 && s.Name.Contains(keyword))
                .OrderByDescending(s => s.CreatedTime)
                .Page(currentPage, Math.Min(pageSizeLimit, memberLimits.SupplierDataLimit))
                .ToList(s => new
                {
                    s.Id,
                    s.CompanyName,
                    s.BusinessScope,
                    s.Address,
                    s.ContactName,
                    s.ContactPhone,
                    s.ContactEmail,
                    IsFavorited = _memberFavoriteRep.Orm.Select<MemberFavoriteEntity>()
                        .Where(a => a.MemberId == User.Id && a.FavoriteType == "Supplier" && a.FavoriteId == s.Id)
                        .Any(),
                    s.CreatedTime
                });

            // 后处理：根据权限限制处理联系人信息
            var processedSupplierList = supplierList.Select(item => new
            {
                item.Id,
                item.CompanyName,
                item.BusinessScope,
                item.Address,
                ContactName = memberLimits.ShowFullContactInfo ? item.ContactName : "****",
                ContactPhone = memberLimits.ShowFullContactInfo ? item.ContactPhone : "****",
                ContactEmail = memberLimits.ShowFullContactInfo ? item.ContactEmail : "****",
                item.IsFavorited,
                item.CreatedTime
            }).ToList();

            suppliers.AddRange(processedSupplierList);
        }

        // 搜索产品（从供应商产品表查询，FreeSql 关联 + 分页）
        if (type == "all" || type == "product")
        {
            var productList = _productSupplierRep.Select
                .Include(ps => ps.Product)
                .Include(ps => ps.Supplier)
                .Where(ps => ps.Status == 1 && ps.Product.Status == 1 && ps.Supplier.Status == 1
                    && (
                        ps.Product.Name.Contains(keyword) ||
                        ps.Product.Description.Contains(keyword) ||
                        ps.SupplierModel.Contains(keyword) ||
                        ps.SupplierProductName.Contains(keyword) ||
                        true
                    ))
                .OrderByDescending(ps => ps.CreatedTime)
                .Page(currentPage, Math.Min(pageSizeLimit, memberLimits.ProductDataLimit))
                .ToList(ps => new
                {
                    Id = ps.Product.Id,
                    ProductName = ps.Product.Name,
                    Category = ps.Product.Category,
                    Description = ps.Product.Description,
                    Specification = ps.Product.Specification,
                    Unit = ps.Product.Unit,
                    Price = ps.CurrentPrice,
                    Currency = ps.Currency,
                    Condition = ps.Condition,
                    MOQ = ps.MOQ,
                    StockQty = ps.StockQty,
                    LeadTimeDays = ps.LeadTimeDays,
                    SupplierId = ps.Supplier.Id,
                    SupplierName = ps.Supplier.CompanyName ?? ps.Supplier.Name,
                    SupplierCode = ps.Supplier.Code,
                    SupplierModel = ps.SupplierModel,
                    SupplierProductCode = ps.SupplierProductCode,
                    SupplierProductName = ps.SupplierProductName,
                    IsFavorited = _memberFavoriteRep.Orm.Select<MemberFavoriteEntity>()
                        .Where(a => a.MemberId == User.Id && a.FavoriteType == "Product" && a.FavoriteId == ps.Product.Id)
                        .Any(),
                    ps.CreatedTime
                });

            products.AddRange(productList);
        }

        return new { Suppliers = suppliers, Products = products };
    }

    /// <summary>
    /// 获取供应商详细信息（包含联系人）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<object> GetSupplierDetailAsync(long id)
    {
        var supplier = await _supplierRep.GetAsync(id);
        if (supplier == null || supplier.Status != 1)
        {
            throw ResultOutput.Exception(_adminLocalizer["供应商不存在或已停用"]);
        }

        // 获取会员权限限制
        var memberLimits = await GetMemberLimitsAsync();

        // 使用 FreeSql 关联查询获取供应商信息及收藏状态
        var supplierInfo = _supplierRep.Select
            .Where(s => s.Id == id && s.Status == 1)
            .Limit(1)
            .ToList(s => new
            {
                s.Id,
                s.Name,
                s.CompanyName,
                s.Code,
                s.BusinessScope,
                s.Address,
                s.Description,
                s.Website,
                s.Rating,
                s.ContactName,
                s.ContactPhone,
                s.ContactEmail,
                s.CreatedTime,
                IsFavorited = _memberFavoriteRep.Orm.Select<MemberFavoriteEntity>()
                    .Where(a => a.MemberId == User.Id && a.FavoriteType == "Supplier" && a.FavoriteId == s.Id)
                    .Any()
            })
            .FirstOrDefault();

        if (supplierInfo == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["供应商不存在或已停用"]);
        }

        // 使用 FreeSql 关联查询获取联系人列表及收藏状态
        var contacts = _contactRep.Select
            .Where(c => c.SupplierId == id)
            .ToList(c => new
            {
                c.Id,
                c.Name,
                c.Phone,
                c.Email,
                c.Position,
                IsFavorited = _memberFavoriteRep.Orm.Select<MemberFavoriteEntity>()
                    .Where(a => a.MemberId == User.Id && a.FavoriteType == "Contact" && a.FavoriteId == c.Id)
                    .Any()
            });

        // 后处理：根据权限限制处理联系人信息
        var processedContacts = contacts.Select(c => new
        {
            c.Id,
            c.Name,
            Phone = memberLimits.ShowFullContactInfo ? c.Phone : "****",
            Email = memberLimits.ShowFullContactInfo ? c.Email : "****",
            c.Position,
            c.IsFavorited
        }).ToList();

        return new
        {
            Supplier = new
            {
                supplierInfo.Id,
                supplierInfo.Name,
                supplierInfo.CompanyName,
                supplierInfo.Code,
                supplierInfo.BusinessScope,
                supplierInfo.Address,
                supplierInfo.Description,
                supplierInfo.Website,
                supplierInfo.Rating,
                ContactName = memberLimits.ShowFullContactInfo ? supplierInfo.ContactName : "****",
                ContactPhone = memberLimits.ShowFullContactInfo ? supplierInfo.ContactPhone : "****",
                ContactEmail = memberLimits.ShowFullContactInfo ? supplierInfo.ContactEmail : "****",
                supplierInfo.IsFavorited,
                supplierInfo.CreatedTime
            },
            Contacts = processedContacts
        };
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
