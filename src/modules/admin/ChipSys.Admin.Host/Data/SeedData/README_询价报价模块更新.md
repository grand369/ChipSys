# 询价报价模块更新说明

## 📋 更新概述

将原来的询价报价合并模块拆分为两个独立的模块：
- **询价模块**：会员发布询价需求
- **报价模块**：供应商对询价进行报价

## 🗂️ 执行脚本列表

按顺序执行以下SQL脚本：

### 1. 菜单配置更新
```sql
-- 删除旧的询价报价菜单
20241201000001_UpdateMenuConfig.sql

-- 新增询价菜单配置
20241201000002_AddInquiryMenu.sql

-- 新增报价菜单配置
20241201000003_AddQuoteMenu.sql

-- 角色权限分配
20241201000004_AssignRolePermissions.sql
```

### 2. 数据库表结构
```sql
-- 创建询价表
20241201000005_CreateInquiryTable.sql

-- 创建报价表
20241201000006_CreateQuoteTable.sql

-- 创建触发器
20241201000007_CreateTriggers.sql
```

### 3. 初始化数据
```sql
-- 插入示例数据
20241201000008_InitData.sql
```

## 📊 数据库表结构

### 询价表 (client_inquiry)
| 字段名 | 类型 | 说明 |
|--------|------|------|
| Id | bigint | 主键ID |
| Title | nvarchar(100) | 询价标题 |
| ProductName | nvarchar(100) | 产品名称 |
| ProductModel | nvarchar(50) | 产品型号 |
| ProductBrand | nvarchar(50) | 产品品牌 |
| ProductManufacturer | nvarchar(100) | 产品制造商 |
| Quantity | int | 需求数量 |
| BudgetRange | nvarchar(100) | 预算范围 |
| ExpectedDeliveryDays | int | 期望交期天数 |
| ValidDays | int | 询价有效期天数 |
| TechnicalRequirements | nvarchar(1000) | 技术参数要求 |
| QualityRequirements | nvarchar(500) | 质量要求 |
| OtherRequirements | nvarchar(500) | 其他要求 |
| ContactInfo | nvarchar(500) | 联系信息 |
| QuoteCount | int | 报价数量 |
| Status | nvarchar(20) | 状态 |
| MemberId | bigint | 会员ID |

### 报价表 (client_quote)
| 字段名 | 类型 | 说明 |
|--------|------|------|
| Id | bigint | 主键ID |
| InquiryId | bigint | 询价ID（外键） |
| SupplierId | bigint | 供应商ID |
| SupplierName | nvarchar(100) | 供应商名称 |
| QuotePrice | decimal(18,2) | 报价金额 |
| Currency | nvarchar(10) | 货币类型 |
| DeliveryDays | int | 交期天数 |
| MinOrderQuantity | int | 最小起订量 |
| ValidDays | int | 有效期天数 |
| ValidUntil | datetime2(7) | 有效期至 |
| ProductSpecification | nvarchar(1000) | 产品规格说明 |
| QualityAssurance | nvarchar(500) | 质量保证 |
| QuoteRemark | nvarchar(1000) | 报价备注 |
| ContactName | nvarchar(50) | 联系人姓名 |
| ContactPhone | nvarchar(20) | 联系电话 |
| ContactEmail | nvarchar(100) | 联系邮箱 |
| Status | nvarchar(20) | 状态 |
| AcceptedTime | datetime2(7) | 接受时间 |
| RejectedTime | datetime2(7) | 拒绝时间 |

## 🔄 状态说明

### 询价状态
- `Draft`: 草稿
- `Published`: 已发布
- `Closed`: 已关闭
- `Completed`: 已完成
- `Cancelled`: 已取消

### 报价状态
- `Pending`: 待审核
- `Approved`: 已通过
- `Accepted`: 已接受
- `Rejected`: 已拒绝
- `Cancelled`: 已取消

## 🔐 权限配置

### 询价权限
- `client:inquiry:view`: 查看询价
- `client:inquiry:add`: 新增询价
- `client:inquiry:edit`: 编辑询价
- `client:inquiry:delete`: 删除询价
- `client:inquiry:publish`: 发布询价
- `client:inquiry:close`: 关闭询价

### 报价权限
- `client:quote:view`: 查看报价
- `client:quote:add`: 新增报价
- `client:quote:edit`: 编辑报价
- `client:quote:delete`: 删除报价
- `client:quote:accept`: 接受报价
- `client:quote:reject`: 拒绝报价

## 🎯 角色权限分配

- **管理员**: 拥有所有权限
- **会员**: 拥有询价的所有权限（除删除外）
- **供应商**: 拥有报价的查看、新增、编辑权限

## ⚠️ 注意事项

1. 执行脚本前请备份数据库
2. 按顺序执行脚本，不要跳过任何步骤
3. 执行完成后检查菜单配置是否正确
4. 测试各个角色的权限是否正常
5. 验证触发器是否正常工作

## 🔧 后续工作

1. 更新前端路由配置
2. 更新API接口实现
3. 测试完整业务流程
4. 更新系统文档
