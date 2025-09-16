-- Client模块视图配置
-- 基础视图配置，用于client模块的前端组件路径映射

-- 1. Client模块根视图
INSERT INTO base_view (Id, Platform, ParentId, Name, Label, Path, Description, Cache, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1000000000000001, NULL, 0, NULL, 'Client模块', NULL, 'Client模块根视图', 1, 1, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 2. 会员收藏视图
INSERT INTO base_view (Id, Platform, ParentId, Name, Label, Path, Description, Cache, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1000000000000101, NULL, 1000000000000001, 'client/member-favorite', '会员收藏', 'client/member-favorite/index', '会员收藏管理页面', 1, 1, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 3. 供求信息视图
INSERT INTO base_view (Id, Platform, ParentId, Name, Label, Path, Description, Cache, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1000000000000201, NULL, 1000000000000001, 'client/supply-demand', '供求信息', 'client/supply-demand/index', '供求信息管理页面', 1, 2, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 4. 供应商申请视图
INSERT INTO base_view (Id, Platform, ParentId, Name, Label, Path, Description, Cache, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1000000000000301, NULL, 1000000000000001, 'client/supplier-application', '供应商申请', 'client/supplier-application/index', '供应商申请管理页面', 1, 3, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 5. 公开查询视图
INSERT INTO base_view (Id, Platform, ParentId, Name, Label, Path, Description, Cache, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1000000000000401, NULL, 1000000000000001, 'client/public-query', '公开查询', 'client/public-query/index', '公开查询页面', 1, 4, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 6. 会员仪表板视图
INSERT INTO base_view (Id, Platform, ParentId, Name, Label, Path, Description, Cache, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1000000000000501, NULL, 1000000000000001, 'client/member-dashboard', '会员仪表板', 'client/member-dashboard/index', '会员仪表板页面', 1, 5, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 7. 会员等级视图
INSERT INTO base_view (Id, Platform, ParentId, Name, Label, Path, Description, Cache, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1000000000000601, NULL, 1000000000000001, 'client/member-level', '会员等级', 'client/member-level/index', '会员等级管理页面', 1, 6, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);

-- 8. 询价报价视图
INSERT INTO base_view (Id, Platform, ParentId, Name, Label, Path, Description, Cache, Sort, Enabled, CreatedUserId, CreatedUserName, CreatedUserRealName, CreatedTime, IsDeleted) 
VALUES (1000000000000701, NULL, 1000000000000001, 'client/inquiry-quote', '询价报价', 'client/inquiry-quote/index', '询价报价管理页面', 1, 7, 1, 161223411986501, 'admin', '管理员', '2025-01-10T10:00:00', 0);
