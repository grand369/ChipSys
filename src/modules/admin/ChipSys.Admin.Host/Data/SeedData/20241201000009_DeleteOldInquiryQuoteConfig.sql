-- 删除旧的询价报价配置
-- 执行时间：2024-12-01 00:00:09

-- 注意：删除顺序很重要，需要先删除有外键引用的表，再删除被引用的表

-- 1. 删除base_permission_api表中的询价报价权限API关联
DELETE FROM base_permission_api 
WHERE PermissionId IN (
    SELECT Id FROM base_permission 
    WHERE Id >= 1100000000000701 AND Id <= 1100000000000718
)
OR ApiId IN (
    SELECT Id FROM base_api 
    WHERE Id >= 1100000000000701 AND Id <= 1100000000000718
);

-- 2. 删除base_permission表中的询价报价权限
DELETE FROM base_permission 
WHERE Id >= 1100000000000701 AND Id <= 1100000000000718;

-- 3. 删除base_api表中的询价报价API
DELETE FROM base_api 
WHERE Id >= 1100000000000701 AND Id <= 1100000000000718;

-- 4. 删除base_view表中的询价报价视图
DELETE FROM base_view 
WHERE Id = 1100000000000701;

-- 验证删除结果
SELECT 'base_permission_api' as TableName, COUNT(*) as RemainingCount
FROM base_permission_api 
WHERE PermissionId >= 1100000000000701 AND PermissionId <= 1100000000000718
UNION ALL
SELECT 'base_permission' as TableName, COUNT(*) as RemainingCount
FROM base_permission 
WHERE Id >= 1100000000000701 AND Id <= 1100000000000718
UNION ALL
SELECT 'base_api' as TableName, COUNT(*) as RemainingCount
FROM base_api 
WHERE Id >= 1100000000000701 AND Id <= 1100000000000718
UNION ALL
SELECT 'base_view' as TableName, COUNT(*) as RemainingCount
FROM base_view 
WHERE Id = 1100000000000701;
