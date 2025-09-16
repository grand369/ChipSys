-- 检查Client模块的视图配置
-- 用于验证base_view表中的记录是否正确

-- 1. 检查Client模块的视图记录（更新后的ID范围）
SELECT 
    Id,
    Name,
    Label,
    Path,
    Enabled
FROM base_view 
WHERE Id >= 1100000000000001 AND Id <= 1100000000000701
ORDER BY Id;

-- 2. 检查Client模块的权限记录及其关联的视图（更新后的ID范围）
SELECT 
    p.Id as PermissionId,
    p.Label as PermissionLabel,
    p.ViewId,
    v.Name as ViewName,
    v.Path as ViewPath
FROM base_permission p
LEFT JOIN base_view v ON p.ViewId = v.Id
WHERE p.Id >= 1100000000000001 AND p.Id <= 1100000000000701
AND p.Type = 2  -- 只查看菜单类型的权限
ORDER BY p.Id;

-- 3. 检查是否有旧的ID记录残留
SELECT 'base_view' as TableName, COUNT(*) as OldIdCount
FROM base_view 
WHERE Id >= 1000000000000001 AND Id <= 1000000000000701
UNION ALL
SELECT 'base_permission' as TableName, COUNT(*) as OldIdCount
FROM base_permission 
WHERE Id >= 1000000000000001 AND Id <= 1000000000000701
UNION ALL
SELECT 'base_api' as TableName, COUNT(*) as OldIdCount
FROM base_api 
WHERE Id >= 1000000000000001 AND Id <= 1000000000000701
UNION ALL
SELECT 'base_permission_api' as TableName, COUNT(*) as OldIdCount
FROM base_permission_api 
WHERE Id >= 1000000000000001 AND Id <= 1000000000000068;
