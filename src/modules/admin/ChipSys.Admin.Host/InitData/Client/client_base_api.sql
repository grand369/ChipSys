-- Client模块API配置
-- 基础API配置，用于client模块的权限管理

-- 1. Client模块根节点
INSERT INTO base_api (Id, ParentId, Name, Label, Path, HttpMethods, EnabledLog, EnabledParams, EnabledResult, Description, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1000000000000001, 0, NULL, 'Client模块', 'client', NULL, 1, 0, 0, 'Client模块API根节点', 1, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 2. 会员收藏服务API
INSERT INTO base_api (Id, ParentId, Name, Label, Path, HttpMethods, EnabledLog, EnabledParams, EnabledResult, Description, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1000000000000101, 1000000000000001, NULL, '会员收藏服务', 'member-favorite', NULL, 1, 0, 0, '会员收藏相关API', 1, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 3. 会员收藏具体API
INSERT INTO base_api (Id, ParentId, Name, Label, Path, HttpMethods, EnabledLog, EnabledParams, EnabledResult, Description, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) VALUES
(1000000000000111, 1000000000000101, NULL, '查询收藏', '/api/client/member-favorite/get', 'get', 1, 0, 0, '查询单个收藏记录', 1, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000112, 1000000000000101, NULL, '查询收藏分页', '/api/client/member-favorite/get-page', 'post', 1, 0, 0, '查询收藏分页列表', 2, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000113, 1000000000000101, NULL, '添加收藏', '/api/client/member-favorite/add', 'post', 1, 0, 0, '添加收藏记录', 3, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000114, 1000000000000101, NULL, '更新收藏', '/api/client/member-favorite/update', 'put', 1, 0, 0, '更新收藏记录', 4, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000115, 1000000000000101, NULL, '删除收藏', '/api/client/member-favorite/delete', 'delete', 1, 0, 0, '删除收藏记录', 5, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000116, 1000000000000101, NULL, '批量删除收藏', '/api/client/member-favorite/batch-delete', 'post', 1, 0, 0, '批量删除收藏记录', 6, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000117, 1000000000000101, NULL, '检查收藏状态', '/api/client/member-favorite/is-favorited', 'get', 1, 0, 0, '检查是否已收藏', 7, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000118, 1000000000000101, NULL, '切换收藏状态', '/api/client/member-favorite/toggle-favorite', 'post', 1, 0, 0, '一键收藏/取消收藏', 8, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 4. 供求信息服务API
INSERT INTO base_api (Id, ParentId, Name, Label, Path, HttpMethods, EnabledLog, EnabledParams, EnabledResult, Description, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1000000000000201, 1000000000000001, NULL, '供求信息服务', 'supply-demand', NULL, 1, 0, 0, '供求信息相关API', 2, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 5. 供求信息具体API
INSERT INTO base_api (Id, ParentId, Name, Label, Path, HttpMethods, EnabledLog, EnabledParams, EnabledResult, Description, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) VALUES
(1000000000000211, 1000000000000201, NULL, '查询供求信息', '/api/client/supply-demand/get', 'get', 1, 0, 0, '查询单个供求信息', 1, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000212, 1000000000000201, NULL, '查询供求信息分页', '/api/client/supply-demand/get-page', 'post', 1, 0, 0, '查询供求信息分页列表', 2, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000213, 1000000000000201, NULL, '添加供求信息', '/api/client/supply-demand/add', 'post', 1, 0, 0, '添加供求信息', 3, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000214, 1000000000000201, NULL, '更新供求信息', '/api/client/supply-demand/update', 'put', 1, 0, 0, '更新供求信息', 4, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000215, 1000000000000201, NULL, '删除供求信息', '/api/client/supply-demand/delete', 'delete', 1, 0, 0, '删除供求信息', 5, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000216, 1000000000000201, NULL, '批量删除供求信息', '/api/client/supply-demand/batch-delete', 'post', 1, 0, 0, '批量删除供求信息', 6, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000217, 1000000000000201, NULL, '发布供求信息', '/api/client/supply-demand/publish', 'post', 1, 0, 0, '发布供求信息', 7, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000218, 1000000000000201, NULL, '关闭供求信息', '/api/client/supply-demand/close', 'post', 1, 0, 0, '关闭供求信息', 8, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000219, 1000000000000201, NULL, '查询公开供求信息', '/api/client/supply-demand/get-public-page', 'post', 1, 0, 0, '查询公开的供求信息', 9, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 6. 供应商申请服务API
INSERT INTO base_api (Id, ParentId, Name, Label, Path, HttpMethods, EnabledLog, EnabledParams, EnabledResult, Description, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1000000000000301, 1000000000000001, NULL, '供应商申请服务', 'supplier-application', NULL, 1, 0, 0, '供应商申请相关API', 3, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 7. 供应商申请具体API
INSERT INTO base_api (Id, ParentId, Name, Label, Path, HttpMethods, EnabledLog, EnabledParams, EnabledResult, Description, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) VALUES
(1000000000000311, 1000000000000301, NULL, '查询供应商申请', '/api/client/supplier-application/get', 'get', 1, 0, 0, '查询单个供应商申请', 1, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000312, 1000000000000301, NULL, '查询供应商申请分页', '/api/client/supplier-application/get-page', 'post', 1, 0, 0, '查询供应商申请分页列表', 2, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000313, 1000000000000301, NULL, '添加供应商申请', '/api/client/supplier-application/add', 'post', 1, 0, 0, '添加供应商申请', 3, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000314, 1000000000000301, NULL, '更新供应商申请', '/api/client/supplier-application/update', 'put', 1, 0, 0, '更新供应商申请', 4, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000315, 1000000000000301, NULL, '删除供应商申请', '/api/client/supplier-application/delete', 'delete', 1, 0, 0, '删除供应商申请', 5, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000316, 1000000000000301, NULL, '提交供应商申请', '/api/client/supplier-application/submit', 'post', 1, 0, 0, '提交供应商申请', 6, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000317, 1000000000000301, NULL, '撤销供应商申请', '/api/client/supplier-application/cancel', 'post', 1, 0, 0, '撤销供应商申请', 7, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000318, 1000000000000301, NULL, '检查申请状态', '/api/client/supplier-application/has-pending-application', 'get', 1, 0, 0, '检查是否有待处理申请', 8, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 8. 公开查询服务API
INSERT INTO base_api (Id, ParentId, Name, Label, Path, HttpMethods, EnabledLog, EnabledParams, EnabledResult, Description, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1000000000000401, 1000000000000001, NULL, '公开查询服务', 'public-query', NULL, 1, 0, 0, '公开查询相关API', 4, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 9. 公开查询具体API
INSERT INTO base_api (Id, ParentId, Name, Label, Path, HttpMethods, EnabledLog, EnabledParams, EnabledResult, Description, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) VALUES
(1000000000000411, 1000000000000401, NULL, '查询公开供应商', '/api/client/public-query/get-public-suppliers', 'post', 1, 0, 0, '查询公开的供应商列表', 1, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000412, 1000000000000401, NULL, '查询供应商详情', '/api/client/public-query/get-public-supplier', 'get', 1, 0, 0, '查询公开的供应商详情', 2, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000413, 1000000000000401, NULL, '查询公开产品', '/api/client/public-query/get-public-products', 'post', 1, 0, 0, '查询公开的产品列表', 3, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000414, 1000000000000401, NULL, '查询产品详情', '/api/client/public-query/get-public-product', 'get', 1, 0, 0, '查询公开的产品详情', 4, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000415, 1000000000000401, NULL, '查询供应商产品', '/api/client/public-query/get-supplier-products', 'post', 1, 0, 0, '查询供应商的产品列表', 5, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000416, 1000000000000401, NULL, '搜索功能', '/api/client/public-query/search', 'get', 1, 0, 0, '搜索供应商和产品', 6, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 10. 会员仪表板服务API
INSERT INTO base_api (Id, ParentId, Name, Label, Path, HttpMethods, EnabledLog, EnabledParams, EnabledResult, Description, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1000000000000501, 1000000000000001, NULL, '会员仪表板服务', 'member-dashboard', NULL, 1, 0, 0, '会员仪表板相关API', 5, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 11. 会员仪表板具体API
INSERT INTO base_api (Id, ParentId, Name, Label, Path, HttpMethods, EnabledLog, EnabledParams, EnabledResult, Description, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) VALUES
(1000000000000511, 1000000000000501, NULL, '获取仪表板统计', '/api/client/member-dashboard/get-dashboard-stats', 'get', 1, 0, 0, '获取会员仪表板统计数据', 1, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000512, 1000000000000501, NULL, '获取收藏统计', '/api/client/member-dashboard/get-favorite-stats', 'get', 1, 0, 0, '获取会员收藏统计', 2, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000513, 1000000000000501, NULL, '获取供求统计', '/api/client/member-dashboard/get-supply-demand-stats', 'get', 1, 0, 0, '获取会员供求信息统计', 3, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 12. 会员等级服务API
INSERT INTO base_api (Id, ParentId, Name, Label, Path, HttpMethods, EnabledLog, EnabledParams, EnabledResult, Description, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1000000000000601, 1000000000000001, NULL, '会员等级服务', 'member-level', NULL, 1, 0, 0, '会员等级相关API', 6, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 13. 会员等级具体API
INSERT INTO base_api (Id, ParentId, Name, Label, Path, HttpMethods, EnabledLog, EnabledParams, EnabledResult, Description, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) VALUES
(1000000000000611, 1000000000000601, NULL, '查询会员等级', '/api/client/member-level/get', 'get', 1, 0, 0, '查询单个会员等级', 1, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000612, 1000000000000601, NULL, '查询会员等级分页', '/api/client/member-level/get-page', 'post', 1, 0, 0, '查询会员等级分页列表', 2, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000613, 1000000000000601, NULL, '获取当前等级', '/api/client/member-level/get-current-level', 'get', 1, 0, 0, '获取会员当前等级', 3, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000614, 1000000000000601, NULL, '升级会员等级', '/api/client/member-level/upgrade-level', 'post', 1, 0, 0, '升级会员等级', 4, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000615, 1000000000000601, NULL, '更新会员等级', '/api/client/member-level/update-level', 'put', 1, 0, 0, '更新会员等级', 5, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000616, 1000000000000601, NULL, '检查会员权限', '/api/client/member-level/check-permission', 'get', 1, 0, 0, '检查会员权限', 6, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 14. 询价报价服务API
INSERT INTO base_api (Id, ParentId, Name, Label, Path, HttpMethods, EnabledLog, EnabledParams, EnabledResult, Description, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1000000000000701, 1000000000000001, NULL, '询价报价服务', 'inquiry-quote', NULL, 1, 0, 0, '询价报价相关API', 7, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 15. 询价报价具体API
INSERT INTO base_api (Id, ParentId, Name, Label, Path, HttpMethods, EnabledLog, EnabledParams, EnabledResult, Description, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) VALUES
(1000000000000711, 1000000000000701, NULL, '查询询价报价', '/api/client/inquiry-quote/get', 'get', 1, 0, 0, '查询单个询价报价', 1, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000712, 1000000000000701, NULL, '查询询价报价分页', '/api/client/inquiry-quote/get-page', 'post', 1, 0, 0, '查询询价报价分页列表', 2, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000713, 1000000000000701, NULL, '查询询价报价列表', '/api/client/inquiry-quote/get-inquiry-quotes', 'get', 1, 0, 0, '查询询价的报价列表', 3, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000714, 1000000000000701, NULL, '提交报价', '/api/client/inquiry-quote/submit-quote', 'post', 1, 0, 0, '提交报价', 4, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000715, 1000000000000701, NULL, '更新报价', '/api/client/inquiry-quote/update-quote', 'put', 1, 0, 0, '更新报价', 5, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000716, 1000000000000701, NULL, '接受报价', '/api/client/inquiry-quote/accept-quote', 'post', 1, 0, 0, '接受报价', 6, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000717, 1000000000000701, NULL, '拒绝报价', '/api/client/inquiry-quote/reject-quote', 'post', 1, 0, 0, '拒绝报价', 7, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1000000000000718, 1000000000000701, NULL, '删除报价', '/api/client/inquiry-quote/delete', 'delete', 1, 0, 0, '删除报价', 8, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);
