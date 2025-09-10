/** 账号类型 */
export const AccountType = {
  UserName: { name: 'UserName', value: 1, desc: '账号' },
  Mobile: { name: 'Mobile', value: 2, desc: '手机' },
  Email: { name: 'Email', value: 3, desc: '邮箱' },
}

/** 密码加密类型 */
export const PasswordEncryptType = {
  MD5Encrypt32: { name: 'MD5Encrypt32', value: 0, desc: '32位MD5加密' },
  PasswordHasher: { name: 'PasswordHasher', value: 1, desc: '标准标识密码哈希' },
}

/** 用户状�?*/
export const UserStatus = {
  WaitChangePasssword: { name: 'WaitChangePasssword', value: 2, desc: '待修改密�? },
  WaitActive: { name: 'WaitActive', value: 3, desc: '待激�? },
}

/** 性别 */
export const Sex = {
  Unknown: { name: 'Unknown', value: 0, desc: '未知' },
  Male: { name: 'Male', value: 1, desc: '�? },
  Female: { name: 'Female', value: 2, desc: '�? },
}

/** 角色类型 */
export const RoleType = {
  Group: { name: 'Group', value: 1, desc: '分组' },
  Role: { name: 'Role', value: 2, desc: '角色' },
}

/** 地区级别 */
export const RegionLevel = {
  Province: { name: 'Province', value: 1, desc: '省份' },
  City: { name: 'City', value: 2, desc: '城市' },
  County: { name: 'County', value: 3, desc: '�?�? },
  Town: { name: 'Town', value: 4, desc: '�?�? },
  Vilage: { name: 'Vilage', value: 5, desc: '�?社区' },
}

/** 权限类型 */
export const PermissionType = {
  Group: { name: 'Group', value: 1, desc: '分组' },
  Menu: { name: 'Menu', value: 2, desc: '菜单' },
  Dot: { name: 'Dot', value: 3, desc: '权限�? },
}

/** 消息状�?*/
export const MsgStatusEnum = {
  Draft: { name: 'Draft', value: 1, desc: '草稿' },
  Published: { name: 'Published', value: 2, desc: '已发�? },
  Scheduled: { name: 'Scheduled', value: 3, desc: '定时发布' },
  Revoked: { name: 'Revoked', value: 4, desc: '已撤销' },
  Archived: { name: 'Archived', value: 5, desc: '已归�? },
}

/** 文档类型 */
export const DocType = {
  Group: { name: 'Group', value: 1, desc: '分组' },
  Markdown: { name: 'Markdown', value: 2, desc: 'Markdown文档' },
}

/** 缓存类型 */
export const CacheType = {
  Memory: { name: 'Memory', value: 0, desc: '内存缓存' },
  Redis: { name: 'Redis', value: 1, desc: 'Redis缓存' },
}

/** 租户类型 */
export const TenantType = {
  Platform: { name: 'Platform', value: 1, desc: '平台' },
  Tenant: { name: 'Tenant', value: 2, desc: '租户' },
}

/** 数据范围 */
export const DataScope = {
  All: { name: 'All', value: 1, desc: '全部' },
  DeptWithChild: { name: 'DeptWithChild', value: 2, desc: '本部门和下级部门' },
  Dept: { name: 'Dept', value: 3, desc: '本部�? },
  Self: { name: 'Self', value: 4, desc: '本人数据' },
  Custom: { name: 'Custom', value: 5, desc: '指定部门' },
}

/** 用户类型 */
export const UserType = {
  Member: { name: 'Member', value: 0, desc: '会员' },
  DefaultUser: { name: 'DefaultUser', value: 1, desc: '普通用�? },
  TenantAdmin: { name: 'TenantAdmin', value: 10, desc: '租户管理�? },
  PlatformAdmin: { name: 'PlatformAdmin', value: 100, desc: '平台管理�? },
}

/** 接口版本 */
export const ApiVersion = {
  V1: { name: 'V1', value: 1, desc: 'V1 版本' },
  V2: { name: 'V2', value: 2, desc: 'V2 版本' },
}

/** 内容类型 */
export const ContentTypeEnum = {
  FormData: { name: 'FormData', value: 0, desc: '表单数据' },
  Json: { name: 'Json', value: 1, desc: 'Json格式' },
}

/** 状态码枚举 */
export const StatusCodes = {
  Status0NotOk: { name: 'Status0NotOk', value: 0, desc: '操作失败' },
  Status1Ok: { name: 'Status1Ok', value: 1, desc: '操作成功' },
  Status401Unauthorized: { name: 'Status401Unauthorized', value: 401, desc: '未登�? },
  Status403Forbidden: { name: 'Status403Forbidden', value: 403, desc: '权限不足' },
  Status404NotFound: { name: 'Status404NotFound', value: 404, desc: '资源不存�? },
  Status500InternalServerError: { name: 'Status500InternalServerError', value: 500, desc: '系统内部错误' },
}

/** 排序方式 */
export const SortOrder = {
  Asc: { name: 'Asc', value: 0, desc: '' },
  Desc: { name: 'Desc', value: 1, desc: '' },
}

/** 应用程序类型 */
export const AppType = {
  Controllers: { name: 'Controllers', value: 0, desc: '' },
  ControllersWithViews: { name: 'ControllersWithViews', value: 1, desc: '' },
  MVC: { name: 'MVC', value: 2, desc: '' },
}

/** 记录日志方式 */
export const LogMethod = {
  Grpc: { name: 'Grpc', value: 0, desc: '' },
  Cap: { name: 'Cap', value: 1, desc: '' },
}

