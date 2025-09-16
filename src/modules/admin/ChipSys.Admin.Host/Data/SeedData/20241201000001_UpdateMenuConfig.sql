-- 更新菜单配置：删除旧的询价报价菜单，新增询价和报价两个独立菜单
-- 执行时间：2024-12-01 00:00:01

-- 1. 删除旧的询价报价菜单
DELETE FROM sys_menu WHERE Code = 'client_inquiry_quote';

-- 2. 删除相关的菜单权限
DELETE FROM sys_role_menu WHERE MenuId IN (
    SELECT Id FROM sys_menu WHERE Code = 'client_inquiry_quote'
);

-- 3. 删除相关的菜单按钮权限
DELETE FROM sys_role_menu WHERE MenuId IN (
    SELECT Id FROM sys_menu WHERE Code LIKE 'client_inquiry_quote_%'
);
