-- 新增报价模块配置
-- 执行时间：2024-12-01 00:00:11

-- 1. 报价服务API
INSERT INTO base_api (Id, ParentId, Name, Label, Path, HttpMethods, EnabledLog, EnabledParams, EnabledResult, Description, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1100000000000801, 1100000000000001, NULL, '报价服务', 'quote', NULL, 1, 0, 0, '报价相关API', 8, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 2. 报价具体API
INSERT INTO base_api (Id, ParentId, Name, Label, Path, HttpMethods, EnabledLog, EnabledParams, EnabledResult, Description, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) VALUES
(1100000000000811, 1100000000000801, NULL, '查询报价', '/api/client/quote/get', 'get', 1, 0, 0, '查询单个报价', 1, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000812, 1100000000000801, NULL, '查询报价分页', '/api/client/quote/get-page', 'post', 1, 0, 0, '查询报价分页列表', 2, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000813, 1100000000000801, NULL, '新增报价', '/api/client/quote/add', 'post', 1, 0, 0, '新增报价', 3, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000814, 1100000000000801, NULL, '更新报价', '/api/client/quote/update', 'put', 1, 0, 0, '更新报价', 4, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000815, 1100000000000801, NULL, '删除报价', '/api/client/quote/delete', 'delete', 1, 0, 0, '删除报价', 5, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000816, 1100000000000801, NULL, '接受报价', '/api/client/quote/accept', 'post', 1, 0, 0, '接受报价', 6, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000817, 1100000000000801, NULL, '拒绝报价', '/api/client/quote/reject', 'post', 1, 0, 0, '拒绝报价', 7, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000818, 1100000000000801, NULL, '获取询价报价列表', '/api/client/quote/get-inquiry-quotes', 'post', 1, 0, 0, '获取询价的报价列表', 8, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000819, 1100000000000801, NULL, '获取报价统计', '/api/client/quote/get-stats', 'get', 1, 0, 0, '获取报价统计信息', 9, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 3. 报价权限组
INSERT INTO base_permission (Id, Platform, ParentId, Label, Code, Type, ViewId, Name, Path, Redirect, Icon, Hidden, Opened, NewWindow, External, IsKeepAlive, IsAffix, Link, IsIframe, IsSystem, Sort, Description, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1100000000000801, NULL, 1100000000000001, '报价管理', NULL, 2, 1100000000000801, 'client/quote', '/client/quote', '', 'ele-Money', 0, 1, 0, 0, 1, 0, NULL, 0, 0, 8, '报价管理权限组', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 4. 报价具体权限
INSERT INTO base_permission (Id, Platform, ParentId, Label, Code, Type, ViewId, Name, Path, Redirect, Icon, Hidden, Opened, NewWindow, External, IsKeepAlive, IsAffix, Link, IsIframe, IsSystem, Sort, Description, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) VALUES
(1100000000000811, NULL, 1100000000000801, '查询报价', 'client.quote.get', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 1, '查询报价权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000812, NULL, 1100000000000801, '查询报价分页', 'client.quote.get-page', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 2, '查询报价分页权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000813, NULL, 1100000000000801, '新增报价', 'client.quote.add', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 3, '新增报价权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000814, NULL, 1100000000000801, '更新报价', 'client.quote.update', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 4, '更新报价权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000815, NULL, 1100000000000801, '删除报价', 'client.quote.delete', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 5, '删除报价权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000816, NULL, 1100000000000801, '接受报价', 'client.quote.accept', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 6, '接受报价权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000817, NULL, 1100000000000801, '拒绝报价', 'client.quote.reject', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 7, '拒绝报价权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000818, NULL, 1100000000000801, '获取询价报价列表', 'client.quote.get-inquiry-quotes', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 8, '获取询价报价列表权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000819, NULL, 1100000000000801, '获取报价统计', 'client.quote.get-stats', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 9, '获取报价统计权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 5. 报价权限与API关联
INSERT INTO base_permission_api (Id, PermissionId, ApiId, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) VALUES
(1100000000000801, 1100000000000811, 1100000000000811, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000802, 1100000000000812, 1100000000000812, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000803, 1100000000000813, 1100000000000813, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000804, 1100000000000814, 1100000000000814, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000805, 1100000000000815, 1100000000000815, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000806, 1100000000000816, 1100000000000816, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000807, 1100000000000817, 1100000000000817, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000808, 1100000000000818, 1100000000000818, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000809, 1100000000000819, 1100000000000819, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 6. 报价视图
INSERT INTO base_view (Id, Platform, ParentId, Name, Label, Path, Description, Cache, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1100000000000801, NULL, 1100000000000001, 'client/quote', '报价管理', 'client/quote/index', '报价管理页面', 1, 8, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);
