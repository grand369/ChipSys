/**
 * mitt 事件类型定义
 *
 * @method openSetingsDrawer 打开布局设置弹窗
 * @method restoreDefault 分栏布局，鼠标移入、移出数据显�?
 * @method setSendColumnsChildren 分栏布局，鼠标移入、移出菜单数据传入到 navMenu 下的菜单�?
 * @method setSendClassicChildren 经典布局，开启切割菜单时，菜单数据传入到 navMenu 下的菜单�?
 * @method getBreadcrumbIndexSetFilterRoutes 布局设置弹窗，开启切割菜单时，菜单数据传入到 navMenu 下的菜单�?
 * @method layoutMobileResize 浏览器窗口改变时，用于适配移动端界面显�?
 * @method openOrCloseSortable 布局设置弹窗，开�?TagsView 拖拽
 * @method openShareTagsView 布局设置弹窗，开�?TagsView 共用
 * @method onTagsViewRefreshRouterView tagsview 刷新界面
 * @method onCurrentContextmenuClick tagsview 右键菜单每项点击�?

 * @method refreshDictType 刷新字典类型
 * @method refreshDict 刷新字典
 * @method refreshOrg 刷新部门
 * @method refreshOrgImg 刷新部门图形
 * @method refreshApi 刷新接口
 * @method refreshPermission 刷新权限
 * @method refreshRole 刷新角色
 * @method refreshTenant 刷新租户
 * @method refreshUser 刷新用户
 * @method refreshView 刷新视图
 * @method refreshTask 刷新任务
 * @method refreshRegion 刷新地区
 * @method refreshMsg 刷新消息
 * @method refreshMsgType 刷新消息分类
 * @method refreshSiteMsg 刷新站内�?
 * @method checkUnreadMsg 检查未读消�?
 * @method forceOffline 强制下线
 * @method refreshPrintTemplate 刷新打印模板
 */
declare type MittType<T = any> = {
  openSetingsDrawer?: string
  restoreDefault?: string
  setSendColumnsChildren: T
  setSendClassicChildren: T
  getBreadcrumbIndexSetFilterRoutes?: string
  layoutMobileResize: T
  openOrCloseSortable?: string
  openShareTagsView?: string
  onTagsViewRefreshRouterView?: T
  onCurrentContextmenuClick?: T

  refreshDictType?: T
  refreshDict?: T
  refreshOrg?: T
  refreshOrgImg?: T
  refreshApi?: T
  refreshPermission?: T
  refreshRole?: T
  refreshTenant?: T
  refreshPkg?: T
  refreshUser?: T
  refreshView?: T
  refreshFile?: T
  refreshTask?: T
  refreshRegion?: T
  refreshMsg?: T
  refreshMsgType?: T
  refreshSiteMsg?: T
  checkUnreadMsg?: T
  forceOffline?: T
  refreshPrintTemplate?: T
}

// mitt 参数类型定义
declare type LayoutMobileResize = {
  layout: string
  clientWidth: number
}

// mitt 参数菜单类型
declare type MittMenu = {
  children: RouteRecordRaw[]
  item?: RouteItem
}
