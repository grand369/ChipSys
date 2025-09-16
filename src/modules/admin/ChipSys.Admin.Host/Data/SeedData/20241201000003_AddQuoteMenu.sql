-- 新增报价菜单配置
-- 执行时间：2024-12-01 00:00:03

-- 1. 新增报价主菜单
INSERT INTO sys_menu (
    Id, ParentId, Name, Code, Type, Route, Component, Permission, Application, 
    OpenType, Visible, Icon, Sort, Remark, CreatedUserId, CreatedUserName, 
    CreatedTime, ModifiedUserId, ModifiedUserName, ModifiedTime, IsDeleted
) VALUES (
    NEWID(), 
    (SELECT Id FROM sys_menu WHERE Code = 'client' AND IsDeleted = 0), 
    '报价管理', 
    'client_quote', 
    1, 
    '/client/quote', 
    'client/quote/index', 
    'client:quote:view', 
    'client', 
    1, 
    1, 
    'el-icon-money', 
    41, 
    '供应商报价管理', 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    0
);

-- 2. 新增报价查看权限
INSERT INTO sys_menu (
    Id, ParentId, Name, Code, Type, Route, Component, Permission, Application, 
    OpenType, Visible, Icon, Sort, Remark, CreatedUserId, CreatedUserName, 
    CreatedTime, ModifiedUserId, ModifiedUserName, ModifiedTime, IsDeleted
) VALUES (
    NEWID(), 
    (SELECT Id FROM sys_menu WHERE Code = 'client_quote' AND IsDeleted = 0), 
    '查看', 
    'client_quote_view', 
    2, 
    '', 
    '', 
    'client:quote:view', 
    'client', 
    1, 
    1, 
    '', 
    1, 
    '查看报价信息', 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    0
);

-- 3. 新增报价新增权限
INSERT INTO sys_menu (
    Id, ParentId, Name, Code, Type, Route, Component, Permission, Application, 
    OpenType, Visible, Icon, Sort, Remark, CreatedUserId, CreatedUserName, 
    CreatedTime, ModifiedUserId, ModifiedUserName, ModifiedTime, IsDeleted
) VALUES (
    NEWID(), 
    (SELECT Id FROM sys_menu WHERE Code = 'client_quote' AND IsDeleted = 0), 
    '新增', 
    'client_quote_add', 
    2, 
    '', 
    '', 
    'client:quote:add', 
    'client', 
    1, 
    1, 
    '', 
    2, 
    '新增报价', 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    0
);

-- 4. 新增报价编辑权限
INSERT INTO sys_menu (
    Id, ParentId, Name, Code, Type, Route, Component, Permission, Application, 
    OpenType, Visible, Icon, Sort, Remark, CreatedUserId, CreatedUserName, 
    CreatedTime, ModifiedUserId, ModifiedUserName, ModifiedTime, IsDeleted
) VALUES (
    NEWID(), 
    (SELECT Id FROM sys_menu WHERE Code = 'client_quote' AND IsDeleted = 0), 
    '编辑', 
    'client_quote_edit', 
    2, 
    '', 
    '', 
    'client:quote:edit', 
    'client', 
    1, 
    1, 
    '', 
    3, 
    '编辑报价', 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    0
);

-- 5. 新增报价删除权限
INSERT INTO sys_menu (
    Id, ParentId, Name, Code, Type, Route, Component, Permission, Application, 
    OpenType, Visible, Icon, Sort, Remark, CreatedUserId, CreatedUserName, 
    CreatedTime, ModifiedUserId, ModifiedUserName, ModifiedTime, IsDeleted
) VALUES (
    NEWID(), 
    (SELECT Id FROM sys_menu WHERE Code = 'client_quote' AND IsDeleted = 0), 
    '删除', 
    'client_quote_delete', 
    2, 
    '', 
    '', 
    'client:quote:delete', 
    'client', 
    1, 
    1, 
    '', 
    4, 
    '删除报价', 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    0
);

-- 6. 新增报价接受权限
INSERT INTO sys_menu (
    Id, ParentId, Name, Code, Type, Route, Component, Permission, Application, 
    OpenType, Visible, Icon, Sort, Remark, CreatedUserId, CreatedUserName, 
    CreatedTime, ModifiedUserId, ModifiedUserName, ModifiedTime, IsDeleted
) VALUES (
    NEWID(), 
    (SELECT Id FROM sys_menu WHERE Code = 'client_quote' AND IsDeleted = 0), 
    '接受', 
    'client_quote_accept', 
    2, 
    '', 
    '', 
    'client:quote:accept', 
    'client', 
    1, 
    1, 
    '', 
    5, 
    '接受报价', 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    0
);

-- 7. 新增报价拒绝权限
INSERT INTO sys_menu (
    Id, ParentId, Name, Code, Type, Route, Component, Permission, Application, 
    OpenType, Visible, Icon, Sort, Remark, CreatedUserId, CreatedUserName, 
    CreatedTime, ModifiedUserId, ModifiedUserName, ModifiedTime, IsDeleted
) VALUES (
    NEWID(), 
    (SELECT Id FROM sys_menu WHERE Code = 'client_quote' AND IsDeleted = 0), 
    '拒绝', 
    'client_quote_reject', 
    2, 
    '', 
    '', 
    'client:quote:reject', 
    'client', 
    1, 
    1, 
    '', 
    6, 
    '拒绝报价', 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    '00000000-0000-0000-0000-000000000001', 
    'System', 
    GETDATE(), 
    0
);
