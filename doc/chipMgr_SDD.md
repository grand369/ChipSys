# 滴答滴系统详细设计文档（SDD V2.0）

## 版本信息
- **文档版本**：V2.0  
- **编写日期**：2025-09-08  
- **编写人**：技术团队  

---

## 1. 文档目的
本设计文档用于指导滴答滴系统的研发，确保开发人员对功能、数据结构、接口、流程有一致理解。文档与 PRD V2.0 配套，描述系统的技术实现和设计细节。  

---

## 2. 系统架构

### 2.1 架构风格
- **前端**：基于 Vue/React，组件化 UI  
- **后端**：.NET 8 + WebAPI  
- **数据库**：MySQL 8.x  
- **ORM**：FreeSql  
- **消息队列**（可选）：RabbitMQ，用于异步处理（如批量导入、通知广播）  
- **缓存**：Redis，用于加速查询和缓存命中清单  

### 2.2 架构图
```mermaid
flowchart TD
    FE[前端 Web/移动端] --> API[.NET API 网关]
    API --> SVC[应用服务层]
    SVC --> DB[(MySQL)]
    SVC --> REDIS[(Redis)]
    SVC --> MQ[(消息队列)]

## 3. 模块设计

### 3.1 搜索与命中清单模块
- **输入**：用户输入产品编码/上传文件  
- **处理**：
  1. 标准化输入（去除空格、大小写、别名匹配）  
  2. 在 `Products` 表中匹配  
  3. 关联 `ProductSuppliers` 获取报价、成色、库存  
  4. 结果写入 `HitLists`  
- **输出**：命中清单数据，返回前端展示  

### 3.2 广播与询价模块
- **广播**：从 `HitLists` 或 `UserLists` 生成询价单，发送给多家供应商  
- **询价单**：存储在 `UserLists`，支持状态变更（待回复、已回复、归档）  
- **通知机制**：消息队列推送，或邮件/短信 API  

### 3.3 管理模块
- **供应商管理**：CRUD 操作 `Suppliers`、`Contacts`  
- **库存管理**：`ProductSuppliers.StockQty` 字段维护，支持批量导入  
- **分类管理**：基于树状结构 `Categories`，支持无限级  

### 3.4 报告模块
- **统计来源**：`Products`、`ProductSuppliers`、`HitLists`、`UserLists`  
- **展示**：折线图、柱状图、数据表格  
- **导出**：Excel、PDF  

---

## 4. 数据库设计

### 4.1 实体关系图
```mermaid
erDiagram
    Categories ||--o{ Products : 包含
    Products ||--o{ ProductSuppliers : 供应
    Suppliers ||--o{ ProductSuppliers : 提供
    Suppliers ||--o{ Contacts : 拥有
    UploadLists ||--o{ UploadListItems : 包含
    UploadListItems ||--o{ HitLists : 命中
    HitLists ||--o{ UserLists : 转化

### 4.2 表结构说明
- **Categories**：产品分类，自关联  
- **Products**：产品信息  
- **Suppliers**：供应商  
- **Contacts**：联系人  
- **ProductSuppliers**：产品-供应商报价、成色、库存  
- **UploadLists**：用户上传的清单  
- **UploadListItems**：清单条目  
- **HitLists**：命中清单  
- **UserLists**：用户操作清单  

---

## 5. 接口设计（API 示例）

### 5.1 产品分类 API
- `GET /api/categories` → 获取所有分类树  
- `POST /api/categories` → 新增分类  
- `PUT /api/categories/{id}` → 修改分类  
- `DELETE /api/categories/{id}` → 删除分类  

### 5.2 产品 API
- `GET /api/products?code=xxx&brand=yyy` → 搜索产品  
- `POST /api/products` → 新增产品  
- `PUT /api/products/{id}` → 修改产品  

### 5.3 上传清单 API
- `POST /api/upload-lists` → 上传清单文件  
- `GET /api/upload-lists/{id}/items` → 获取清单条目  
- `POST /api/upload-lists/{id}/match` → 执行匹配生成命中清单  

### 5.4 命中清单 API
- `GET /api/hit-lists/{id}` → 获取命中清单详情  
- `POST /api/hit-lists/{id}/to-userlist` → 转为用户清单  

---

## 6. 流程设计

### 6.1 清单处理流程
```mermaid
sequenceDiagram
    participant U as 用户
    participant API as 系统API
    participant DB as 数据库

    U->>API: 上传清单文件
    API->>DB: 写入 UploadLists & UploadListItems
    API->>DB: 产品匹配（Products + ProductSuppliers）
    DB-->>API: 返回匹配结果
    API->>DB: 写入 HitLists
    API-->>U: 返回命中清单

### 6.2 询价广播流程
```mermaid
sequenceDiagram
    participant U as 采购人员
    participant API as 系统API
    participant MQ as 消息队列
    participant S as 供应商

    U->>API: 从命中清单生成询价单
    API->>DB: 保存 UserLists
    API->>MQ: 推送询价消息
    MQ->>S: 通知供应商
    S->>API: 提交报价
    API->>DB: 更新报价状态
