# 滴答滴系统需求文档（最终版 PRD）

## 版本信息
- **文档版本**：V2.0  
- **编写日期**：2025-09-08  
- **编写人**：产品团队  

---

## 1. 项目背景
滴答滴系统是一个面向供应链与采购业务的综合平台，提供从零件搜索、询价广播，到供应商管理、库存管理，再到报表统计的全流程服务。  
本项目特别支持：  
- **产品分类管理**，便于多级分类检索  
- **多供应商价格**，包括成色、历史价格和最新价格  
- **上传清单与命中清单**，实现快速比对和数据闭环  

---

## 2. 系统目标
1. 提供高效的 **搜索与清单比对**，提升采购效率。  
2. 支持 **多供应商报价、库存、成色** 管理，适配二手市场需求。  
3. 实现 **询价单广播、供应商管理**，加强采购方与供应商的协同。  
4. 提供 **报表统计和趋势分析**，辅助企业决策。  

---

## 3. 用户角色
- **普通用户**：执行搜索、查看结果、上传清单。  
- **采购人员**：管理命中清单、发送询价、维护供应商关系。  
- **管理员**：系统配置、权限管理、产品与分类维护。  
- **管理层**：查看报表，进行采购决策与供应商评价。  

---

## 4. 系统功能结构

### 4.1 搜索与命中清单
- **搜索功能**：支持按产品编码、品牌、分类关键字查询。  
- **上传清单**：支持 Excel/CSV 文件，系统自动比对并生成命中清单。  
- **命中清单**：展示产品-供应商匹配结果，包括：
  - 产品编码、品牌、描述  
  - 供应商信息（联系人、联系方式）  
  - 报价（之前价格、最新价格、币种）  
  - 成色（全新、95新、翻新等）  
  - 库存数量、交期  

### 4.2 广播与询价
- **广播功能**：  
  - 批量推送询价需求给指定供应商或分组  
  - 支持附件上传、草稿保存、预览  
- **询价单**：  
  - 从命中清单快速生成  
  - 支持转发、归档、回复  

### 4.3 管理模块
- **公司与供应商管理**  
  - 公司信息、供应商基本信息维护  
  - 支持导入、编辑、导出  
- **联系人管理**  
  - 多联系人维护  
  - 供应商联系人信息统一管理  
- **库存管理**  
  - 批量导入库存数据  
  - 锁定/解锁库存  

### 4.4 报告模块
- **统计维度**：  
  - 分类、产品、供应商、联系人  
- **报表输出**：  
  - 图表与数据表  
  - 支持导出 Excel / PDF  
- **数据应用**：  
  - 价格趋势分析  
  - 供应商响应率分析  

---

## 5. 数据库表结构（核心）

### 5.1 产品分类表 `Categories`
- 支持多级分类，形成树状结构。  
- 字段：`CategoryId`、`ParentId`、`Name`、`Code`、`Description`、`Enabled`、`Sort`  

### 5.2 产品表 `Products`
- 产品基础信息。  
- 字段：`ProductId`、`CategoryId`、`Code`、`Brand`、`Description`、`Manufacturer`  

### 5.3 供应商表 `Suppliers`
- 字段：`SupplierId`、`Name`、`ContactPerson`、`Phone`、`Email`、`Address`、`Rating`  

### 5.4 联系人表 `Contacts`
- 字段：`ContactId`、`SupplierId`、`Name`、`Phone`、`Email`、`Position`  

### 5.5 产品-供应商关系表 `ProductSuppliers`
- 体现产品的不同供应商报价和二手信息。  
- 字段：  
  - `ProductSupplierId`、`ProductId`、`SupplierId`  
  - `PreviousPrice`、`CurrentPrice`、`Currency`  
  - `Condition`、`UsageDescription`  
  - `MOQ`、`LeadTimeDays`、`StockQty`  
  - `ValidFrom`、`ValidTo`  

### 5.6 上传清单表 `UploadLists`
- 用户上传文件信息。  
- 字段：`ListId`、`UserId`、`FileName`、`Status`  

### 5.7 清单条目表 `UploadListItems`
- 上传清单的具体条目。  
- 字段：`ItemId`、`ListId`、`RawCode`、`RawBrand`、`RawDescription`、`MatchStatus`、`MatchedProductId`  

### 5.8 命中清单表 `HitLists`
- 比对后生成的命中结果。  
- 字段：`HitId`、`UserId`、`ItemId`、`ProductSupplierId`、`Confidence`  

### 5.9 用户操作清单表 `UserLists`
- 用户的二次操作清单（关注、询价、采购）。  
- 字段：`UserListId`、`UserId`、`ProductSupplierId`、`ListType`、`Status`  

---

## 6. 页面流程

```mermaid
flowchart TD
    U[用户上传清单] --> UL[UploadLists]
    UL --> ULI[UploadListItems]
    ULI -->|比对| P[Products]
    P --> PS[ProductSuppliers]
    PS --> HL[HitLists]
    HL -->|转化| UL2[UserLists]
    UL2 -->|生成| IQ[询价单]
