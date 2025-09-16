# Client模块前端页面说明

## 概述

本目录包含了Client模块的前端页面，用于会员端的功能展示和操作。

## 目录结构

```
client/
├── member-favorite/           # 会员收藏模块
│   ├── components/
│   │   └── member-favorite-form.vue  # 收藏表单组件
│   └── index.vue             # 收藏列表页面
├── supply-demand/            # 供求信息模块
│   ├── components/
│   │   └── supply-demand-form.vue    # 供求信息表单组件
│   └── index.vue             # 供求信息列表页面
├── supplier-application/     # 供应商申请模块
│   ├── components/
│   │   └── supplier-application-form.vue  # 申请表单组件
│   └── index.vue             # 申请列表页面
├── public-query/             # 公开查询模块
│   ├── components/
│   └── index.vue             # 查询页面
├── member-dashboard/         # 会员仪表板模块
│   ├── components/
│   └── index.vue             # 仪表板页面
├── member-level/             # 会员等级模块
│   ├── components/
│   │   └── member-level-form.vue     # 等级表单组件
│   └── index.vue             # 等级列表页面
├── inquiry-quote/            # 询价报价模块
│   ├── components/
│   │   └── inquiry-quote-form.vue    # 报价表单组件
│   └── index.vue             # 报价列表页面
└── README.md                 # 本说明文件
```

## 功能模块

### 1. 会员收藏 (member-favorite)
- **功能**: 管理会员的收藏记录，包括供应商、产品、联系人
- **页面**: 列表展示、新增、编辑、删除收藏
- **权限**: 需要相应的收藏权限

### 2. 供求信息 (supply-demand)
- **功能**: 发布和管理供求信息
- **页面**: 列表展示、发布、编辑、删除供求信息
- **权限**: 需要相应的供求信息权限

### 3. 供应商申请 (supplier-application)
- **功能**: 申请成为供应商
- **页面**: 申请表单、申请状态查看
- **权限**: 需要相应的申请权限

### 4. 公开查询 (public-query)
- **功能**: 查询公开的供应商和产品信息
- **页面**: 搜索、浏览供应商和产品
- **权限**: 公开访问，但受会员等级限制

### 5. 会员仪表板 (member-dashboard)
- **功能**: 展示会员的统计信息和最近活动
- **页面**: 数据统计、最近收藏、最近供求信息
- **权限**: 需要相应的仪表板权限

### 6. 会员等级 (member-level)
- **功能**: 管理会员等级和权限
- **页面**: 等级查看、升级申请
- **权限**: 需要相应的等级权限

### 7. 询价报价 (inquiry-quote)
- **功能**: 处理询价和报价
- **页面**: 报价列表、提交报价、接受/拒绝报价
- **权限**: 需要相应的报价权限

## 技术特点

### 1. 组件化设计
- 每个功能模块都有独立的页面和组件
- 表单组件可复用，便于维护

### 2. 权限控制
- 使用 `v-auth` 指令控制按钮和功能的显示
- 权限码与后端API权限保持一致

### 3. 响应式布局
- 使用 Element Plus 的栅格系统
- 适配不同屏幕尺寸

### 4. 数据管理
- 使用 Vue 3 的 Composition API
- 响应式数据管理
- 统一的API调用方式

## 使用说明

### 1. 路由配置
需要在路由配置中添加client模块的路由：

```typescript
// router/index.ts
{
  path: '/client',
  name: 'client',
  component: () => import('/@/layout/index.vue'),
  redirect: '/client/dashboard',
  children: [
    {
      path: '/client/dashboard',
      name: 'client/dashboard',
      component: () => import('/@/views/client/member-dashboard/index.vue'),
      meta: { title: '会员仪表板', icon: 'ele-Dashboard' }
    },
    {
      path: '/client/member-favorite',
      name: 'client/member-favorite',
      component: () => import('/@/views/client/member-favorite/index.vue'),
      meta: { title: '我的收藏', icon: 'ele-Collection' }
    },
    // ... 其他路由
  ]
}
```

### 2. 菜单配置
需要在菜单配置中添加client模块的菜单项，菜单权限码与后端权限保持一致。

### 3. API配置
确保API文件中的接口地址与后端API保持一致。

## 开发规范

### 1. 命名规范
- 文件名使用kebab-case
- 组件名使用PascalCase
- 变量名使用camelCase

### 2. 代码规范
- 使用TypeScript
- 遵循Vue 3 Composition API规范
- 使用Element Plus组件库

### 3. 权限规范
- 所有需要权限控制的功能都要添加`v-auth`指令
- 权限码格式：`client.模块名.操作名`

## 扩展说明

如果需要添加新的功能模块：

1. 在`client`目录下创建新的模块目录
2. 创建对应的页面和组件文件
3. 在`/api/client/`目录下创建对应的API文件
4. 在路由配置中添加新的路由
5. 在菜单配置中添加新的菜单项
6. 更新本文档的说明

## 注意事项

1. **权限控制**: 所有页面都要考虑权限控制，确保用户只能访问有权限的功能
2. **数据验证**: 表单提交前要进行数据验证
3. **错误处理**: API调用要有适当的错误处理
4. **用户体验**: 提供加载状态、成功提示、错误提示等用户反馈
5. **响应式设计**: 确保在不同设备上都有良好的显示效果
