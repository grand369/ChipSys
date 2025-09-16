-- 新增询价菜单配置
-- 执行时间：2024-12-01 00:00:02

-- 1. 新增询价主菜单
INSERT INTO sys_menu (
    Id, ParentId, Name, Code, Type, Route, Component, Permission, Application, 
    OpenType, Visible, Icon, Sort, Remark, CreatedUserId, CreatedUserName, 
    CreatedTime, ModifiedUserId, ModifiedUserName, ModifiedTime, IsDeleted
) VALUES (
    NEWID(), 
    (SELECT Id FROM sys_menu WHERE Code = 'client' AND IsDeleted = 0), 
    '询价管理', 
    'client_inquiry', 
    1, 
    '/client/inquiry', 
    'client/inquiry/index', 
    'client:inquiry:view', 
    'client', 
    1, 
    1, 
    'el-icon-document', 
    40, 
    '会员询价需求管理', 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    0
);

-- 2. 新增询价查看权限
INSERT INTO sys_menu (
    Id, ParentId, Name, Code, Type, Route, Component, Permission, Application, 
    OpenType, Visible, Icon, Sort, Remark, CreatedUserId, CreatedUserName, 
    CreatedTime, ModifiedUserId, ModifiedUserName, ModifiedTime, IsDeleted
) VALUES (
    NEWID(), 
    (SELECT Id FROM sys_menu WHERE Code = 'client_inquiry' AND IsDeleted = 0), 
    '查看', 
    'client_inquiry_view', 
    2, 
    '', 
    '', 
    'client:inquiry:view', 
    'client', 
    1, 
    1, 
    '', 
    1, 
    '查看询价信息', 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    0
);

-- 3. 新增询价新增权限
INSERT INTO sys_menu (
    Id, ParentId, Name, Code, Type, Route, Component, Permission, Application, 
    OpenType, Visible, Icon, Sort, Remark, CreatedUserId, CreatedUserName, 
    CreatedTime, ModifiedUserId, ModifiedUserName, ModifiedTime, IsDeleted
) VALUES (
    NEWID(), 
    (SELECT Id FROM sys_menu WHERE Code = 'client_inquiry' AND IsDeleted = 0), 
    '新增', 
    'client_inquiry_add', 
    2, 
    '', 
    '', 
    'client:inquiry:add', 
    'client', 
    1, 
    1, 
    '', 
    2, 
    '新增询价', 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    0
);

-- 4. 新增询价编辑权限
INSERT INTO sys_menu (
    Id, ParentId, Name, Code, Type, Route, Component, Permission, Application, 
    OpenType, Visible, Icon, Sort, Remark, CreatedUserId, CreatedUserName, 
    CreatedTime, ModifiedUserId, ModifiedUserName, ModifiedTime, IsDeleted
) VALUES (
    NEWID(), 
    (SELECT Id FROM sys_menu WHERE Code = 'client_inquiry' AND IsDeleted = 0), 
    '编辑', 
    'client_inquiry_edit', 
    2, 
    '', 
    '', 
    'client:inquiry:edit', 
    'client', 
    1, 
    1, 
    '', 
    3, 
    '编辑询价', 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    0
);

-- 5. 新增询价删除权限
INSERT INTO sys_menu (
    Id, ParentId, Name, Code, Type, Route, Component, Permission, Application, 
    OpenType, Visible, Icon, Sort, Remark, CreatedUserId, CreatedUserName, 
    CreatedTime, ModifiedUserId, ModifiedUserName, ModifiedTime, IsDeleted
) VALUES (
    NEWID(), 
    (SELECT Id FROM sys_menu WHERE Code = 'client_inquiry' AND IsDeleted = 0), 
    '删除', 
    'client_inquiry_delete', 
    2, 
    '', 
    '', 
    'client:inquiry:delete', 
    'client', 
    1, 
    1, 
    '', 
    4, 
    '删除询价', 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    0
);

-- 6. 新增询价发布权限
INSERT INTO sys_menu (
    Id, ParentId, Name, Code, Type, Route, Component, Permission, Application, 
    OpenType, Visible, Icon, Sort, Remark, CreatedUserId, CreatedUserName, 
    CreatedTime, ModifiedUserId, ModifiedUserName, ModifiedTime, IsDeleted
) VALUES (
    NEWID(), 
    (SELECT Id FROM sys_menu WHERE Code = 'client_inquiry' AND IsDeleted = 0), 
    '发布', 
    'client_inquiry_publish', 
    2, 
    '', 
    '', 
    'client:inquiry:publish', 
    'client', 
    1, 
    1, 
    '', 
    5, 
    '发布询价', 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    0
);

-- 7. 新增询价关闭权限
INSERT INTO sys_menu (
    Id, ParentId, Name, Code, Type, Route, Component, Permission, Application, 
    OpenType, Visible, Icon, Sort, Remark, CreatedUserId, CreatedUserName, 
    CreatedTime, ModifiedUserId, ModifiedUserName, ModifiedTime, IsDeleted
) VALUES (
    NEWID(), 
    (SELECT Id FROM sys_menu WHERE Code = 'client_inquiry' AND IsDeleted = 0), 
    '关闭', 
    'client_inquiry_close', 
    2, 
    '', 
    '', 
    'client:inquiry:close', 
    'client', 
    1, 
    1, 
    '', 
    6, 
    '关闭询价', 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    0
);
