-- 新增询价模块配置
-- 执行时间：2024-12-01 00:00:10

-- 1. 询价服务API
INSERT INTO base_api (Id, ParentId, Name, Label, Path, HttpMethods, EnabledLog, EnabledParams, EnabledResult, Description, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1100000000000701, 1100000000000001, NULL, '询价服务', 'inquiry', NULL, 1, 0, 0, '询价相关API', 7, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 2. 询价具体API
INSERT INTO base_api (Id, ParentId, Name, Label, Path, HttpMethods, EnabledLog, EnabledParams, EnabledResult, Description, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) VALUES
(1100000000000711, 1100000000000701, NULL, '查询询价', '/api/client/inquiry/get', 'get', 1, 0, 0, '查询单个询价', 1, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000712, 1100000000000701, NULL, '查询询价分页', '/api/client/inquiry/get-page', 'post', 1, 0, 0, '查询询价分页列表', 2, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000713, 1100000000000701, NULL, '新增询价', '/api/client/inquiry/add', 'post', 1, 0, 0, '新增询价', 3, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000714, 1100000000000701, NULL, '更新询价', '/api/client/inquiry/update', 'put', 1, 0, 0, '更新询价', 4, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000715, 1100000000000701, NULL, '删除询价', '/api/client/inquiry/delete', 'delete', 1, 0, 0, '删除询价', 5, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000716, 1100000000000701, NULL, '发布询价', '/api/client/inquiry/publish', 'post', 1, 0, 0, '发布询价', 6, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000717, 1100000000000701, NULL, '关闭询价', '/api/client/inquiry/close', 'post', 1, 0, 0, '关闭询价', 7, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000718, 1100000000000701, NULL, '获取询价统计', '/api/client/inquiry/get-stats', 'get', 1, 0, 0, '获取询价统计信息', 8, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 3. 询价权限组
INSERT INTO base_permission (Id, Platform, ParentId, Label, Code, Type, ViewId, Name, Path, Redirect, Icon, Hidden, Opened, NewWindow, External, IsKeepAlive, IsAffix, Link, IsIframe, IsSystem, Sort, Description, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1100000000000701, NULL, 1100000000000001, '询价管理', NULL, 2, 1100000000000701, 'client/inquiry', '/client/inquiry', '', 'ele-Document', 0, 1, 0, 0, 1, 0, NULL, 0, 0, 7, '询价管理权限组', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 4. 询价具体权限
INSERT INTO base_permission (Id, Platform, ParentId, Label, Code, Type, ViewId, Name, Path, Redirect, Icon, Hidden, Opened, NewWindow, External, IsKeepAlive, IsAffix, Link, IsIframe, IsSystem, Sort, Description, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) VALUES
(1100000000000711, NULL, 1100000000000701, '查询询价', 'client.inquiry.get', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 1, '查询询价权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000712, NULL, 1100000000000701, '查询询价分页', 'client.inquiry.get-page', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 2, '查询询价分页权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000713, NULL, 1100000000000701, '新增询价', 'client.inquiry.add', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 3, '新增询价权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000714, NULL, 1100000000000701, '更新询价', 'client.inquiry.update', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 4, '更新询价权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000715, NULL, 1100000000000701, '删除询价', 'client.inquiry.delete', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 5, '删除询价权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000716, NULL, 1100000000000701, '发布询价', 'client.inquiry.publish', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 6, '发布询价权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000717, NULL, 1100000000000701, '关闭询价', 'client.inquiry.close', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 7, '关闭询价权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000718, NULL, 1100000000000701, '获取询价统计', 'client.inquiry.get-stats', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 8, '获取询价统计权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 5. 询价权限与API关联
INSERT INTO base_permission_api (Id, PermissionId, ApiId, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) VALUES
(1100000000000701, 1100000000000711, 1100000000000711, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000702, 1100000000000712, 1100000000000712, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000703, 1100000000000713, 1100000000000713, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000704, 1100000000000714, 1100000000000714, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000705, 1100000000000715, 1100000000000715, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000706, 1100000000000716, 1100000000000716, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000707, 1100000000000717, 1100000000000717, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000708, 1100000000000718, 1100000000000718, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 6. 询价视图
INSERT INTO base_view (Id, Platform, ParentId, Name, Label, Path, Description, Cache, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1100000000000701, NULL, 1100000000000001, 'client/inquiry', '询价管理', 'client/inquiry/index', '询价管理页面', 1, 7, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);
