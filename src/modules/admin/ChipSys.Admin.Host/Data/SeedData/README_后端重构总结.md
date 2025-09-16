# 后端询价报价模块重构总结

## 📋 重构概述

将原来的询价报价合并模块拆分为两个独立的模块，符合实际业务逻辑：
- **询价模块**：会员发布询价需求
- **报价模块**：供应商对询价进行报价

## 🗂️ 新增文件列表

### 1. 实体类 (Domain)
```
Admin.Core/src/modules/admin/ChipSys.Admin.Contracts/Domain/Client/
├── InquiryEntity.cs          # 询价实体
└── QuoteEntity.cs            # 报价实体
```

### 2. DTO类 (Data Transfer Objects)
```
Admin.Core/src/modules/admin/ChipSys.Admin.Contracts/Services/Client/Dto/
├── InquiryDto.cs             # 询价DTO
└── QuoteDto.cs               # 报价DTO
```

### 3. 服务接口 (Service Interfaces)
```
Admin.Core/src/modules/admin/ChipSys.Admin.Contracts/Services/Client/
├── IInquiryService.cs        # 询价服务接口
└── IQuoteService.cs          # 报价服务接口
```

### 4. 服务实现 (Service Implementations)
```
Admin.Core/src/modules/admin/ChipSys.Admin/Services/Client/
├── InquiryService.cs         # 询价服务实现
└── QuoteService.cs           # 报价服务实现
```

## 🗑️ 删除文件列表

### 旧文件已删除
```
Admin.Core/src/modules/admin/ChipSys.Admin.Contracts/Domain/Client/
└── InquiryQuoteEntity.cs     # 旧的询价报价合并实体

Admin.Core/src/modules/admin/ChipSys.Admin.Contracts/Services/Client/Dto/
└── InquiryQuoteDto.cs        # 旧的询价报价合并DTO

Admin.Core/src/modules/admin/ChipSys.Admin.Contracts/Services/Client/
└── IInquiryQuoteService.cs   # 旧的询价报价合并服务接口

Admin.Core/src/modules/admin/ChipSys.Admin/Services/Client/
└── InquiryQuoteService.cs    # 旧的询价报价合并服务实现
```

## 📊 实体类设计

### InquiryEntity (询价实体)
| 字段名 | 类型 | 说明 |
|--------|------|------|
| Id | long | 主键ID |
| Title | string | 询价标题 |
| ProductName | string | 产品名称 |
| ProductModel | string? | 产品型号 |
| ProductBrand | string? | 产品品牌 |
| ProductManufacturer | string? | 产品制造商 |
| Quantity | int | 需求数量 |
| BudgetRange | string? | 预算范围 |
| ExpectedDeliveryDays | int | 期望交期天数 |
| ValidDays | int | 询价有效期天数 |
| TechnicalRequirements | string? | 技术参数要求 |
| QualityRequirements | string? | 质量要求 |
| OtherRequirements | string? | 其他要求 |
| ContactInfo | string | 联系信息 |
| QuoteCount | int | 报价数量 |
| Status | string | 状态 |
| MemberId | long | 会员ID |

### QuoteEntity (报价实体)
| 字段名 | 类型 | 说明 |
|--------|------|------|
| Id | long | 主键ID |
| InquiryId | long | 询价ID（外键） |
| SupplierId | long | 供应商ID |
| SupplierName | string | 供应商名称 |
| QuotePrice | decimal | 报价金额 |
| Currency | string | 货币类型 |
| DeliveryDays | int | 交期天数 |
| MinOrderQuantity | int | 最小起订量 |
| ValidDays | int | 有效期天数 |
| ValidUntil | DateTime? | 有效期至 |
| ProductSpecification | string? | 产品规格说明 |
| QualityAssurance | string? | 质量保证 |
| QuoteRemark | string? | 报价备注 |
| ContactName | string | 联系人姓名 |
| ContactPhone | string | 联系电话 |
| ContactEmail | string? | 联系邮箱 |
| Status | string | 状态 |
| AcceptedTime | DateTime? | 接受时间 |
| RejectedTime | DateTime? | 拒绝时间 |

## 🔧 服务接口设计

### IInquiryService (询价服务接口)
- `GetAsync(long id)` - 获取询价详情
- `GetPageAsync(PageInput<InquiryGetPageInput> input)` - 分页查询询价
- `AddAsync(InquiryAddInput input)` - 新增询价
- `UpdateAsync(InquiryUpdateInput input)` - 更新询价
- `DeleteAsync(long id)` - 删除询价
- `PublishAsync(long id)` - 发布询价
- `CloseAsync(long id)` - 关闭询价
- `GetStatsAsync()` - 获取询价统计

### IQuoteService (报价服务接口)
- `GetAsync(long id)` - 获取报价详情
- `GetPageAsync(PageInput<QuoteGetPageInput> input)` - 分页查询报价
- `AddAsync(QuoteAddInput input)` - 新增报价
- `UpdateAsync(QuoteUpdateInput input)` - 更新报价
- `DeleteAsync(long id)` - 删除报价
- `AcceptAsync(long id)` - 接受报价
- `RejectAsync(long id)` - 拒绝报价
- `GetInquiryQuotesAsync(long inquiryId, QuoteGetPageInput input)` - 获取询价的报价列表
- `GetStatsAsync()` - 获取报价统计

## 🔐 权限控制

### 询价权限
- 会员只能查看、修改、删除自己的询价
- 只有草稿状态的询价才能修改和删除
- 只有草稿状态的询价才能发布
- 只有已发布状态的询价才能关闭

### 报价权限
- 供应商只能查看、修改、删除自己提交的报价
- 会员只能查看自己询价的报价
- 只有待审核状态的报价才能修改和删除
- 只有已审核通过的报价才能被接受或拒绝

## 🔄 状态流转

### 询价状态
- `Draft` → `Published` (发布)
- `Published` → `Closed` (关闭)
- `Published` → `Completed` (完成，接受报价后)
- `Published` → `Cancelled` (取消)

### 报价状态
- `Pending` → `Approved` (审核通过)
- `Approved` → `Accepted` (接受)
- `Approved` → `Rejected` (拒绝)
- `Pending` → `Cancelled` (取消)

## 📈 业务逻辑

### 询价业务
1. 会员创建询价（草稿状态）
2. 会员发布询价（已发布状态）
3. 供应商查看已发布的询价
4. 供应商对询价进行报价
5. 会员查看报价并选择接受或拒绝
6. 会员关闭询价

### 报价业务
1. 供应商查看已发布的询价
2. 供应商提交报价（待审核状态）
3. 系统自动审核通过（已通过状态）
4. 会员查看报价
5. 会员接受或拒绝报价
6. 接受报价后询价自动关闭

## ⚠️ 注意事项

1. **数据一致性**：报价数量通过触发器自动维护
2. **权限控制**：严格按用户身份控制操作权限
3. **状态管理**：只有特定状态才能进行特定操作
4. **级联操作**：删除询价时自动删除相关报价
5. **时间管理**：报价有效期自动计算

## 🔧 后续工作

1. 更新数据库表结构（执行SQL脚本）
2. 更新前端API调用
3. 测试完整业务流程
4. 更新系统文档
5. 性能优化和监控
