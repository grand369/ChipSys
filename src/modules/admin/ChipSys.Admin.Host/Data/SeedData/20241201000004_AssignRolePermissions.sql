-- 为角色分配询价和报价菜单权限
-- 执行时间：2024-12-01 00:00:04

-- 1. 为管理员角色分配询价菜单权限
INSERT INTO sys_role_menu (Id, RoleId, MenuId, CreatedUserId, CreatedUserName, CreatedTime, IsDeleted)
SELECT 
    NEWID(),
    r.Id,
    m.Id,
    '00000000-0000-0000-0000-000000000001',
    'System',
    GETDATE(),
    0
FROM sys_role r
CROSS JOIN sys_menu m
WHERE r.Code = 'admin' 
  AND r.IsDeleted = 0
  AND m.Code IN (
    'client_inquiry',
    'client_inquiry_view',
    'client_inquiry_add',
    'client_inquiry_edit',
    'client_inquiry_delete',
    'client_inquiry_publish',
    'client_inquiry_close'
  )
  AND m.IsDeleted = 0;

-- 2. 为管理员角色分配报价菜单权限
INSERT INTO sys_role_menu (Id, RoleId, MenuId, CreatedUserId, CreatedUserName, CreatedTime, IsDeleted)
SELECT 
    NEWID(),
    r.Id,
    m.Id,
    '00000000-0000-0000-0000-000000000001',
    'System',
    GETDATE(),
    0
FROM sys_role r
CROSS JOIN sys_menu m
WHERE r.Code = 'admin' 
  AND r.IsDeleted = 0
  AND m.Code IN (
    'client_quote',
    'client_quote_view',
    'client_quote_add',
    'client_quote_edit',
    'client_quote_delete',
    'client_quote_accept',
    'client_quote_reject'
  )
  AND m.IsDeleted = 0;

-- 3. 为会员角色分配询价菜单权限（只读权限）
INSERT INTO sys_role_menu (Id, RoleId, MenuId, CreatedUserId, CreatedUserName, CreatedTime, IsDeleted)
SELECT 
    NEWID(),
    r.Id,
    m.Id,
    '00000000-0000-0000-0000-000000000001',
    'System',
    GETDATE(),
    0
FROM sys_role r
CROSS JOIN sys_menu m
WHERE r.Code = 'member' 
  AND r.IsDeleted = 0
  AND m.Code IN (
    'client_inquiry',
    'client_inquiry_view',
    'client_inquiry_add',
    'client_inquiry_edit',
    'client_inquiry_publish',
    'client_inquiry_close'
  )
  AND m.IsDeleted = 0;

-- 4. 为供应商角色分配报价菜单权限
INSERT INTO sys_role_menu (Id, RoleId, MenuId, CreatedUserId, CreatedUserName, CreatedTime, IsDeleted)
SELECT 
    NEWID(),
    r.Id,
    m.Id,
    '00000000-0000-0000-0000-000000000001',
    'System',
    GETDATE(),
    0
FROM sys_role r
CROSS JOIN sys_menu m
WHERE r.Code = 'supplier' 
  AND r.IsDeleted = 0
  AND m.Code IN (
    'client_quote',
    'client_quote_view',
    'client_quote_add',
    'client_quote_edit'
  )
  AND m.IsDeleted = 0;
