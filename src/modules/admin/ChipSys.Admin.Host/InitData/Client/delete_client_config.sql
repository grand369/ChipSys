-- 删除Client模块配置脚本
-- 删除所有Client模块相关的配置数据，避免重复插入

-- 注意：删除顺序很重要，需要先删除有外键引用的表，再删除被引用的表

-- 1. 删除base_permission_api表中的Client模块权限API关联
DELETE FROM base_permission_api 
WHERE PermissionId IN (
    SELECT Id FROM base_permission 
    WHERE Id >= 1000000000000001 AND Id <= 1000000000000718
)
OR ApiId IN (
    SELECT Id FROM base_api 
    WHERE Id >= 1000000000000001 AND Id <= 1000000000000718
);

-- 2. 删除base_permission表中的Client模块权限
DELETE FROM base_permission 
WHERE Id >= 1000000000000001 AND Id <= 1000000000000718;

-- 3. 删除base_api表中的Client模块API
DELETE FROM base_api 
WHERE Id >= 1000000000000001 AND Id <= 1000000000000718;

-- 4. 删除base_view表中的Client模块视图
DELETE FROM base_view 
WHERE Id >= 1000000000000001 AND Id <= 1000000000000701;

-- 验证删除结果
SELECT 'base_permission_api' as TableName, COUNT(*) as RemainingCount
FROM base_permission_api 
WHERE PermissionId >= 1000000000000001 AND PermissionId <= 1000000000000718
UNION ALL
SELECT 'base_permission' as TableName, COUNT(*) as RemainingCount
FROM base_permission 
WHERE Id >= 1000000000000001 AND Id <= 1000000000000718
UNION ALL
SELECT 'base_api' as TableName, COUNT(*) as RemainingCount
FROM base_api 
WHERE Id >= 1000000000000001 AND Id <= 1000000000000718
UNION ALL
SELECT 'base_view' as TableName, COUNT(*) as RemainingCount
FROM base_view 
WHERE Id >= 1000000000000001 AND Id <= 1000000000000701;
