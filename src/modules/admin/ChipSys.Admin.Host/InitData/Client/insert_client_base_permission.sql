-- Client模块权限配置（新ID范围）
-- 基础权限配置，用于client模块的权限管理
-- 使用1100000000000001-1100000000000718的ID范围，避免与Admin模块冲突

-- 1. Client模块权限组
INSERT INTO base_permission (Id, Platform, ParentId, Label, Code, Type, ViewId, Name, Path, Redirect, Icon, Hidden, Opened, NewWindow, External, IsKeepAlive, IsAffix, Link, IsIframe, IsSystem, Sort, Description, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1100000000000001, NULL, 0, 'Client模块', NULL, 1, NULL, NULL, '/client', '', 'ele-Client', 0, 1, 0, 0, 1, 0, NULL, 0, 0, 1, 'Client模块权限组', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 2. 会员收藏权限组
INSERT INTO base_permission (Id, Platform, ParentId, Label, Code, Type, ViewId, Name, Path, Redirect, Icon, Hidden, Opened, NewWindow, External, IsKeepAlive, IsAffix, Link, IsIframe, IsSystem, Sort, Description, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1100000000000101, NULL, 1100000000000001, '会员收藏', NULL, 2, 1100000000000101, 'client/member-favorite', '/client/member-favorite', '', 'ele-Collection', 0, 1, 0, 0, 1, 0, NULL, 0, 0, 1, '会员收藏权限组', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 3. 会员收藏具体权限
INSERT INTO base_permission (Id, Platform, ParentId, Label, Code, Type, ViewId, Name, Path, Redirect, Icon, Hidden, Opened, NewWindow, External, IsKeepAlive, IsAffix, Link, IsIframe, IsSystem, Sort, Description, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) VALUES
(1100000000000111, NULL, 1100000000000101, '查询收藏', 'client.member-favorite.get', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 1, '查询收藏权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000112, NULL, 1100000000000101, '查询收藏分页', 'client.member-favorite.get-page', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 2, '查询收藏分页权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000113, NULL, 1100000000000101, '添加收藏', 'client.member-favorite.add', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 3, '添加收藏权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000114, NULL, 1100000000000101, '更新收藏', 'client.member-favorite.update', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 4, '更新收藏权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000115, NULL, 1100000000000101, '删除收藏', 'client.member-favorite.delete', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 5, '删除收藏权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000116, NULL, 1100000000000101, '批量删除收藏', 'client.member-favorite.batch-delete', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 6, '批量删除收藏权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000117, NULL, 1100000000000101, '检查收藏状态', 'client.member-favorite.is-favorited', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 7, '检查收藏状态权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000118, NULL, 1100000000000101, '切换收藏状态', 'client.member-favorite.toggle-favorite', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 8, '切换收藏状态权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 4. 供求信息权限组
INSERT INTO base_permission (Id, Platform, ParentId, Label, Code, Type, ViewId, Name, Path, Redirect, Icon, Hidden, Opened, NewWindow, External, IsKeepAlive, IsAffix, Link, IsIframe, IsSystem, Sort, Description, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1100000000000201, NULL, 1100000000000001, '供求信息', NULL, 2, 1100000000000201, 'client/supply-demand', '/client/supply-demand', '', 'ele-Info', 0, 1, 0, 0, 1, 0, NULL, 0, 0, 2, '供求信息权限组', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 5. 供求信息具体权限
INSERT INTO base_permission (Id, Platform, ParentId, Label, Code, Type, ViewId, Name, Path, Redirect, Icon, Hidden, Opened, NewWindow, External, IsKeepAlive, IsAffix, Link, IsIframe, IsSystem, Sort, Description, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) VALUES
(1100000000000211, NULL, 1100000000000201, '查询供求信息', 'client.supply-demand.get', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 1, '查询供求信息权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000212, NULL, 1100000000000201, '查询供求信息分页', 'client.supply-demand.get-page', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 2, '查询供求信息分页权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000213, NULL, 1100000000000201, '添加供求信息', 'client.supply-demand.add', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 3, '添加供求信息权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000214, NULL, 1100000000000201, '更新供求信息', 'client.supply-demand.update', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 4, '更新供求信息权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000215, NULL, 1100000000000201, '删除供求信息', 'client.supply-demand.delete', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 5, '删除供求信息权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000216, NULL, 1100000000000201, '批量删除供求信息', 'client.supply-demand.batch-delete', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 6, '批量删除供求信息权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000217, NULL, 1100000000000201, '发布供求信息', 'client.supply-demand.publish', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 7, '发布供求信息权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000218, NULL, 1100000000000201, '关闭供求信息', 'client.supply-demand.close', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 8, '关闭供求信息权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000219, NULL, 1100000000000201, '查询公开供求信息', 'client.supply-demand.get-public-page', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 9, '查询公开供求信息权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 6. 供应商申请权限组
INSERT INTO base_permission (Id, Platform, ParentId, Label, Code, Type, ViewId, Name, Path, Redirect, Icon, Hidden, Opened, NewWindow, External, IsKeepAlive, IsAffix, Link, IsIframe, IsSystem, Sort, Description, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1100000000000301, NULL, 1100000000000001, '供应商申请', NULL, 2, 1100000000000301, 'client/supplier-application', '/client/supplier-application', '', 'ele-Application', 0, 1, 0, 0, 1, 0, NULL, 0, 0, 3, '供应商申请权限组', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 7. 供应商申请具体权限
INSERT INTO base_permission (Id, Platform, ParentId, Label, Code, Type, ViewId, Name, Path, Redirect, Icon, Hidden, Opened, NewWindow, External, IsKeepAlive, IsAffix, Link, IsIframe, IsSystem, Sort, Description, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) VALUES
(1100000000000311, NULL, 1100000000000301, '查询供应商申请', 'client.supplier-application.get', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 1, '查询供应商申请权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000312, NULL, 1100000000000301, '查询供应商申请分页', 'client.supplier-application.get-page', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 2, '查询供应商申请分页权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000313, NULL, 1100000000000301, '添加供应商申请', 'client.supplier-application.add', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 3, '添加供应商申请权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000314, NULL, 1100000000000301, '更新供应商申请', 'client.supplier-application.update', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 4, '更新供应商申请权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000315, NULL, 1100000000000301, '删除供应商申请', 'client.supplier-application.delete', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 5, '删除供应商申请权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000316, NULL, 1100000000000301, '提交供应商申请', 'client.supplier-application.submit', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 6, '提交供应商申请权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000317, NULL, 1100000000000301, '撤销供应商申请', 'client.supplier-application.cancel', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 7, '撤销供应商申请权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000318, NULL, 1100000000000301, '检查申请状态', 'client.supplier-application.has-pending-application', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 8, '检查申请状态权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 8. 公开查询权限组
INSERT INTO base_permission (Id, Platform, ParentId, Label, Code, Type, ViewId, Name, Path, Redirect, Icon, Hidden, Opened, NewWindow, External, IsKeepAlive, IsAffix, Link, IsIframe, IsSystem, Sort, Description, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1100000000000401, NULL, 1100000000000001, '公开查询', NULL, 2, 1100000000000401, 'client/public-query', '/client/public-query', '', 'ele-Search', 0, 1, 0, 0, 1, 0, NULL, 0, 0, 4, '公开查询权限组', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 9. 公开查询具体权限
INSERT INTO base_permission (Id, Platform, ParentId, Label, Code, Type, ViewId, Name, Path, Redirect, Icon, Hidden, Opened, NewWindow, External, IsKeepAlive, IsAffix, Link, IsIframe, IsSystem, Sort, Description, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) VALUES
(1100000000000411, NULL, 1100000000000401, '查询公开供应商', 'client.public-query.get-public-suppliers', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 1, '查询公开供应商权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000412, NULL, 1100000000000401, '查询供应商详情', 'client.public-query.get-public-supplier', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 2, '查询供应商详情权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000413, NULL, 1100000000000401, '查询公开产品', 'client.public-query.get-public-products', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 3, '查询公开产品权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000414, NULL, 1100000000000401, '查询产品详情', 'client.public-query.get-public-product', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 4, '查询产品详情权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000415, NULL, 1100000000000401, '查询供应商产品', 'client.public-query.get-supplier-products', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 5, '查询供应商产品权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000416, NULL, 1100000000000401, '搜索功能', 'client.public-query.search', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 6, '搜索功能权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 10. 会员仪表板权限组
INSERT INTO base_permission (Id, Platform, ParentId, Label, Code, Type, ViewId, Name, Path, Redirect, Icon, Hidden, Opened, NewWindow, External, IsKeepAlive, IsAffix, Link, IsIframe, IsSystem, Sort, Description, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1100000000000501, NULL, 1100000000000001, '会员仪表板', NULL, 2, 1100000000000501, 'client/member-dashboard', '/client/member-dashboard', '', 'ele-Dashboard', 0, 1, 0, 0, 1, 0, NULL, 0, 0, 5, '会员仪表板权限组', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 11. 会员仪表板具体权限
INSERT INTO base_permission (Id, Platform, ParentId, Label, Code, Type, ViewId, Name, Path, Redirect, Icon, Hidden, Opened, NewWindow, External, IsKeepAlive, IsAffix, Link, IsIframe, IsSystem, Sort, Description, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) VALUES
(1100000000000511, NULL, 1100000000000501, '获取仪表板统计', 'client.member-dashboard.get-dashboard-stats', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 1, '获取仪表板统计权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000512, NULL, 1100000000000501, '获取收藏统计', 'client.member-dashboard.get-favorite-stats', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 2, '获取收藏统计权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000513, NULL, 1100000000000501, '获取供求统计', 'client.member-dashboard.get-supply-demand-stats', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 3, '获取供求统计权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 12. 会员等级权限组
INSERT INTO base_permission (Id, Platform, ParentId, Label, Code, Type, ViewId, Name, Path, Redirect, Icon, Hidden, Opened, NewWindow, External, IsKeepAlive, IsAffix, Link, IsIframe, IsSystem, Sort, Description, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1100000000000601, NULL, 1100000000000001, '会员等级', NULL, 2, 1100000000000601, 'client/member-level', '/client/member-level', '', 'ele-Level', 0, 1, 0, 0, 1, 0, NULL, 0, 0, 6, '会员等级权限组', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 13. 会员等级具体权限
INSERT INTO base_permission (Id, Platform, ParentId, Label, Code, Type, ViewId, Name, Path, Redirect, Icon, Hidden, Opened, NewWindow, External, IsKeepAlive, IsAffix, Link, IsIframe, IsSystem, Sort, Description, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) VALUES
(1100000000000611, NULL, 1100000000000601, '查询会员等级', 'client.member-level.get', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 1, '查询会员等级权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000612, NULL, 1100000000000601, '查询会员等级分页', 'client.member-level.get-page', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 2, '查询会员等级分页权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000613, NULL, 1100000000000601, '获取当前等级', 'client.member-level.get-current-level', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 3, '获取当前等级权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000614, NULL, 1100000000000601, '升级会员等级', 'client.member-level.upgrade-level', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 4, '升级会员等级权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000615, NULL, 1100000000000601, '更新会员等级', 'client.member-level.update-level', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 5, '更新会员等级权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000616, NULL, 1100000000000601, '检查会员权限', 'client.member-level.check-permission', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 6, '检查会员权限权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 14. 询价报价权限组
INSERT INTO base_permission (Id, Platform, ParentId, Label, Code, Type, ViewId, Name, Path, Redirect, Icon, Hidden, Opened, NewWindow, External, IsKeepAlive, IsAffix, Link, IsIframe, IsSystem, Sort, Description, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1100000000000701, NULL, 1100000000000001, '询价报价', NULL, 2, 1100000000000701, 'client/inquiry-quote', '/client/inquiry-quote', '', 'ele-Quote', 0, 1, 0, 0, 1, 0, NULL, 0, 0, 7, '询价报价权限组', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 15. 询价报价具体权限
INSERT INTO base_permission (Id, Platform, ParentId, Label, Code, Type, ViewId, Name, Path, Redirect, Icon, Hidden, Opened, NewWindow, External, IsKeepAlive, IsAffix, Link, IsIframe, IsSystem, Sort, Description, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) VALUES
(1100000000000711, NULL, 1100000000000701, '查询询价报价', 'client.inquiry-quote.get', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 1, '查询询价报价权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000712, NULL, 1100000000000701, '查询询价报价分页', 'client.inquiry-quote.get-page', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 2, '查询询价报价分页权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000713, NULL, 1100000000000701, '查询询价报价列表', 'client.inquiry-quote.get-inquiry-quotes', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 3, '查询询价报价列表权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000714, NULL, 1100000000000701, '提交报价', 'client.inquiry-quote.submit-quote', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 4, '提交报价权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000715, NULL, 1100000000000701, '更新报价', 'client.inquiry-quote.update-quote', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 5, '更新报价权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000716, NULL, 1100000000000701, '接受报价', 'client.inquiry-quote.accept-quote', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 6, '接受报价权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000717, NULL, 1100000000000701, '拒绝报价', 'client.inquiry-quote.reject-quote', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 7, '拒绝报价权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0),
(1100000000000718, NULL, 1100000000000701, '删除报价', 'client.inquiry-quote.delete', 3, NULL, NULL, '', '', '', 0, 0, 0, 0, 1, 0, NULL, 0, 0, 8, '删除报价权限', 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);
