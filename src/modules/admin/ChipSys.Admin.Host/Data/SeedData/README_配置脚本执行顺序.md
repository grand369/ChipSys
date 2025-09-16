# 询价报价模块配置脚本执行顺序

## 📋 执行概述

由于将询价和报价分开了，需要删除旧的询价报价配置，并新增独立的询价和报价配置。

## 🗂️ 执行脚本列表

按以下顺序执行SQL脚本：

### 1. 删除旧配置
```sql
-- 删除旧的询价报价配置
20241201000009_DeleteOldInquiryQuoteConfig.sql
```

### 2. 新增询价配置
```sql
-- 新增询价模块配置
20241201000010_AddInquiryConfig.sql
```

### 3. 新增报价配置
```sql
-- 新增报价模块配置
20241201000011_AddQuoteConfig.sql
```

## 📊 配置详情

### 询价模块配置 (ID范围: 1100000000000701-1100000000000718)

#### base_api 表
| ID | 名称 | 路径 | 方法 | 说明 |
|----|------|------|------|------|
| 1100000000000701 | 询价服务 | inquiry | - | 询价服务根节点 |
| 1100000000000711 | 查询询价 | /api/client/inquiry/get | GET | 查询单个询价 |
| 1100000000000712 | 查询询价分页 | /api/client/inquiry/get-page | POST | 查询询价分页列表 |
| 1100000000000713 | 新增询价 | /api/client/inquiry/add | POST | 新增询价 |
| 1100000000000714 | 更新询价 | /api/client/inquiry/update | PUT | 更新询价 |
| 1100000000000715 | 删除询价 | /api/client/inquiry/delete | DELETE | 删除询价 |
| 1100000000000716 | 发布询价 | /api/client/inquiry/publish | POST | 发布询价 |
| 1100000000000717 | 关闭询价 | /api/client/inquiry/close | POST | 关闭询价 |
| 1100000000000718 | 获取询价统计 | /api/client/inquiry/get-stats | GET | 获取询价统计信息 |

#### base_permission 表
| ID | 名称 | 代码 | 类型 | 说明 |
|----|------|------|------|------|
| 1100000000000701 | 询价管理 | - | 2 | 询价管理权限组 |
| 1100000000000711 | 查询询价 | client.inquiry.get | 3 | 查询询价权限 |
| 1100000000000712 | 查询询价分页 | client.inquiry.get-page | 3 | 查询询价分页权限 |
| 1100000000000713 | 新增询价 | client.inquiry.add | 3 | 新增询价权限 |
| 1100000000000714 | 更新询价 | client.inquiry.update | 3 | 更新询价权限 |
| 1100000000000715 | 删除询价 | client.inquiry.delete | 3 | 删除询价权限 |
| 1100000000000716 | 发布询价 | client.inquiry.publish | 3 | 发布询价权限 |
| 1100000000000717 | 关闭询价 | client.inquiry.close | 3 | 关闭询价权限 |
| 1100000000000718 | 获取询价统计 | client.inquiry.get-stats | 3 | 获取询价统计权限 |

#### base_view 表
| ID | 名称 | 路径 | 说明 |
|----|------|------|------|
| 1100000000000701 | 询价管理 | client/inquiry/index | 询价管理页面 |

### 报价模块配置 (ID范围: 1100000000000801-1100000000000819)

#### base_api 表
| ID | 名称 | 路径 | 方法 | 说明 |
|----|------|------|------|------|
| 1100000000000801 | 报价服务 | quote | - | 报价服务根节点 |
| 1100000000000811 | 查询报价 | /api/client/quote/get | GET | 查询单个报价 |
| 1100000000000812 | 查询报价分页 | /api/client/quote/get-page | POST | 查询报价分页列表 |
| 1100000000000813 | 新增报价 | /api/client/quote/add | POST | 新增报价 |
| 1100000000000814 | 更新报价 | /api/client/quote/update | PUT | 更新报价 |
| 1100000000000815 | 删除报价 | /api/client/quote/delete | DELETE | 删除报价 |
| 1100000000000816 | 接受报价 | /api/client/quote/accept | POST | 接受报价 |
| 1100000000000817 | 拒绝报价 | /api/client/quote/reject | POST | 拒绝报价 |
| 1100000000000818 | 获取询价报价列表 | /api/client/quote/get-inquiry-quotes | POST | 获取询价的报价列表 |
| 1100000000000819 | 获取报价统计 | /api/client/quote/get-stats | GET | 获取报价统计信息 |

#### base_permission 表
| ID | 名称 | 代码 | 类型 | 说明 |
|----|------|------|------|------|
| 1100000000000801 | 报价管理 | - | 2 | 报价管理权限组 |
| 1100000000000811 | 查询报价 | client.quote.get | 3 | 查询报价权限 |
| 1100000000000812 | 查询报价分页 | client.quote.get-page | 3 | 查询报价分页权限 |
| 1100000000000813 | 新增报价 | client.quote.add | 3 | 新增报价权限 |
| 1100000000000814 | 更新报价 | client.quote.update | 3 | 更新报价权限 |
| 1100000000000815 | 删除报价 | client.quote.delete | 3 | 删除报价权限 |
| 1100000000000816 | 接受报价 | client.quote.accept | 3 | 接受报价权限 |
| 1100000000000817 | 拒绝报价 | client.quote.reject | 3 | 拒绝报价权限 |
| 1100000000000818 | 获取询价报价列表 | client.quote.get-inquiry-quotes | 3 | 获取询价报价列表权限 |
| 1100000000000819 | 获取报价统计 | client.quote.get-stats | 3 | 获取报价统计权限 |

#### base_view 表
| ID | 名称 | 路径 | 说明 |
|----|------|------|------|
| 1100000000000801 | 报价管理 | client/quote/index | 报价管理页面 |

## 🔗 权限API关联

### 询价权限API关联 (ID范围: 1100000000000701-1100000000000708)
- 1100000000000701: 查询询价权限 ↔ 查询询价API
- 1100000000000702: 查询询价分页权限 ↔ 查询询价分页API
- 1100000000000703: 新增询价权限 ↔ 新增询价API
- 1100000000000704: 更新询价权限 ↔ 更新询价API
- 1100000000000705: 删除询价权限 ↔ 删除询价API
- 1100000000000706: 发布询价权限 ↔ 发布询价API
- 1100000000000707: 关闭询价权限 ↔ 关闭询价API
- 1100000000000708: 获取询价统计权限 ↔ 获取询价统计API

### 报价权限API关联 (ID范围: 1100000000000801-1100000000000809)
- 1100000000000801: 查询报价权限 ↔ 查询报价API
- 1100000000000802: 查询报价分页权限 ↔ 查询报价分页API
- 1100000000000803: 新增报价权限 ↔ 新增报价API
- 1100000000000804: 更新报价权限 ↔ 更新报价API
- 1100000000000805: 删除报价权限 ↔ 删除报价API
- 1100000000000806: 接受报价权限 ↔ 接受报价API
- 1100000000000807: 拒绝报价权限 ↔ 拒绝报价API
- 1100000000000808: 获取询价报价列表权限 ↔ 获取询价报价列表API
- 1100000000000809: 获取报价统计权限 ↔ 获取报价统计API

## ⚠️ 注意事项

1. **执行顺序**: 必须按照指定顺序执行，先删除旧配置，再新增新配置
2. **ID范围**: 使用1100000000000701-1100000000000819的ID范围，避免与现有配置冲突
3. **外键约束**: 删除时先删除base_permission_api，再删除base_permission和base_api
4. **权限分配**: 执行完成后需要为相应角色分配新的权限
5. **前端路由**: 确保前端路由配置与新的视图路径一致

## 🔧 验证方法

执行完成后，可以通过以下SQL验证配置是否正确：

```sql
-- 验证询价配置
SELECT COUNT(*) as InquiryApiCount FROM base_api WHERE Id >= 1100000000000701 AND Id <= 1100000000000718;
SELECT COUNT(*) as InquiryPermissionCount FROM base_permission WHERE Id >= 1100000000000701 AND Id <= 1100000000000718;
SELECT COUNT(*) as InquiryViewCount FROM base_view WHERE Id = 1100000000000701;

-- 验证报价配置
SELECT COUNT(*) as QuoteApiCount FROM base_api WHERE Id >= 1100000000000801 AND Id <= 1100000000000819;
SELECT COUNT(*) as QuotePermissionCount FROM base_permission WHERE Id >= 1100000000000801 AND Id <= 1100000000000819;
SELECT COUNT(*) as QuoteViewCount FROM base_view WHERE Id = 1100000000000801;
```

预期结果：
- InquiryApiCount: 9
- InquiryPermissionCount: 9
- InquiryViewCount: 1
- QuoteApiCount: 10
- QuotePermissionCount: 10
- QuoteViewCount: 1
