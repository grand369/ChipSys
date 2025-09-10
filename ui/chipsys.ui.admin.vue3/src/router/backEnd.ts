import { RouteRecordRaw } from 'vue-router'
import pinia from '/@/stores/index'
import { useUserInfo } from '/@/stores/userInfo'
import { useRequestOldRoutes } from '/@/stores/requestOldRoutes'
import { NextLoading } from '/@/utils/loading'
import { dynamicRoutes, notFoundAndNoPower, commonRoutes } from '/@/router/route'
import { formatTwoStageRoutes, formatFlatteningRoutes, router } from '/@/router/index'
import { useRoutesList } from '/@/stores/routesList'
import { useTagsViewRoutes } from '/@/stores/tagsViewRoutes'
// import { useMenuApi } from '/@/api/menu/index'
import { AuthApi } from '/@/api/admin/Auth'
import { listToTree } from '/@/utils/tree'

// 后端控制路由

// 引入 api 请求接口
// const menuApi = useMenuApi()

/**
 * 获取目录下的 .vue�?tsx 全部文件
 * @method import.meta.glob
 * @link 参考：https://cn.vitejs.dev/guide/features.html#json
 */
const layouModules: any = import.meta.glob('../layout/routerView/*.{vue,tsx}')
const viewsModules: any = import.meta.glob('../views/**/*.{vue,tsx}')
const dynamicViewsModules: Record<string, Function> = Object.assign({}, { ...layouModules }, { ...viewsModules })

/**
 * 后端控制路由：初始化方法，防止刷新时路由丢失
 * @method NextLoading 界面 loading 动画开始执�?
 * @method useUserInfo().setUserInfos() 触发初始化用户信�?pinia
 * @method useRequestOldRoutes().setRequestOldRoutes() 存储接口原始路由（未处理component），根据需求选择使用
 * @method setAddRoute 添加动态路�?
 * @method setFilterMenuAndCacheTagsViewRoutes 设置路由�?pinia routesList 中（已处理成多级嵌套路由）及缓存多级嵌套数组处理后的一维数�?
 */
export async function initBackEndControlRoutes() {
  // 界面 loading 动画开始执�?
  if (window.nextLoading === undefined) NextLoading.start()
  // �?token 停止执行下一�?
  if (!useUserInfo().getToken()) return false
  // 触发初始化用户信�?pinia
  await useUserInfo().setUserInfos()
  // 获取路由菜单数据
  const menus = await getBackEndControlRoutes()
  if (!(menus?.length > 0)) return Promise.resolve(true)
  // 存储接口原始路由（未处理component），根据需求选择使用
  useRequestOldRoutes().setRequestOldRoutes(JSON.parse(JSON.stringify(menus)))
  // 处理路由（component），替换 dynamicRoutes�?@/router/route）第一个顶�?children 的路�?
  const routes = await backEndComponent(menus)
  dynamicRoutes[0].children?.unshift(...routes, ...commonRoutes)

  // 添加动态路�?
  await setAddRoute()
  // 设置路由�?pinia routesList 中（已处理成多级嵌套路由）及缓存多级嵌套数组处理后的一维数�?
  setFilterMenuAndCacheTagsViewRoutes()
}

/**
 * 设置路由�?pinia routesList 中（已处理成多级嵌套路由）及缓存多级嵌套数组处理后的一维数�?
 * @description 用于左侧菜单、横向菜单的显示
 * @description 用于 tagsView、菜单搜索中：未过滤隐藏�?isHide)
 */
export function setFilterMenuAndCacheTagsViewRoutes() {
  const storesRoutesList = useRoutesList(pinia)
  storesRoutesList.setRoutesList(dynamicRoutes[0].children as any)
  setCacheTagsViewRoutes()
}

/**
 * 缓存多级嵌套数组处理后的一维数�?
 * @description 用于 tagsView、菜单搜索中：未过滤隐藏�?isHide)
 */
export function setCacheTagsViewRoutes() {
  const storesTagsView = useTagsViewRoutes(pinia)
  storesTagsView.setTagsViewRoutes(formatTwoStageRoutes(formatFlatteningRoutes(dynamicRoutes))[0].children)
}

/**
 * 处理路由格式及添加捕获所有路由或 404 Not found 路由
 * @description 替换 dynamicRoutes�?@/router/route）第一个顶�?children 的路�?
 * @returns 返回替换后的路由数组
 */
export function setFilterRouteEnd() {
  let filterRouteEnd: any = formatTwoStageRoutes(formatFlatteningRoutes(dynamicRoutes))
  // notFoundAndNoPower 防止 404�?01 不在 layout 布局中，不设置的话，404�?01 界面将全屏显�?
  // 关联问题 No match found for location with path 'xxx'
  filterRouteEnd[0].children = [...filterRouteEnd[0].children, ...notFoundAndNoPower]
  return filterRouteEnd
}

/**
 * 添加动态路�?
 * @method router.addRoute
 * @description 此处循环�?dynamicRoutes�?@/router/route）第一个顶�?children 的路由一维数组，非多级嵌�?
 * @link 参考：https://next.router.vuejs.org/zh/api/#addroute
 */
export async function setAddRoute() {
  await setFilterRouteEnd().forEach((route: RouteRecordRaw) => {
    router.addRoute(route)
  })
}

/**
 * 请求后端路由菜单接口
 * @description isRequestRoutes �?true，则开启后端控制路�?
 * @returns 返回后端路由菜单数据
 */
export async function getBackEndControlRoutes() {
  const res = await new AuthApi().getUserMenus().catch(() => {})
  if (res?.success && (res?.data?.length as number) > 0) {
    const menus = [] as any
    res.data?.forEach((menu) => {
      menus.push({
        id: menu.id,
        parentId: menu.parentId,
        name: menu.name ? menu.name : menu.id + '',
        path: menu.path ? menu.path : menu.id + '',
        redirect: menu.redirect,
        component: menu.viewPath ? menu.viewPath : 'Layout',
        meta: {
          id: menu.id,
          title: menu.label as string,
          icon: menu.icon,
          isAffix: menu.isAffix,
          isHide: menu.hidden,
          isIframe: menu.isIframe,
          isKeepAlive: menu.isKeepAlive,
          isLink: menu.link,
          status: 1,
          remark: null,
          order: menu.sort,
          isDir: !menu.viewPath,
        },
      })
    })
    const menuTree = listToTree(menus)
    return menuTree
  } else {
    return []
  }
}

/**
 * 重新请求后端路由菜单接口
 * @description 用于菜单管理界面刷新菜单（未进行测试�?
 * @description 路径�?src/views/system/menu/component/addMenu.vue
 */
export function setBackEndControlRefreshRoutes() {
  getBackEndControlRoutes()
}

/**
 * 后端路由 component 转换
 * @param routes 后端返回的路由表数组
 * @returns 返回处理成函数后�?component
 */
export function backEndComponent(routes: any) {
  if (!routes) return
  return routes.map((item: any) => {
    if (item.component) item.component = dynamicImport(dynamicViewsModules, item.component as string)
    item.children && backEndComponent(item.children)
    return item
  })
}

/**
 * 后端路由 component 转换函数
 * @param dynamicViewsModules 获取目录下的 .vue�?tsx 全部文件
 * @param component 当前要处理项 component
 * @returns 返回处理成函数后�?component
 */
export function dynamicImport(dynamicViewsModules: Record<string, Function>, component: string) {
  const keys = Object.keys(dynamicViewsModules)
  const matchKeys = keys.filter((key) => {
    const k = key.replace(/..\/views|../, '')
    return k.startsWith(`${component}`) || k.startsWith(`/${component}`)
  })
  if (matchKeys?.length === 1) {
    const matchKey = matchKeys[0]
    return dynamicViewsModules[matchKey]
  }
  if (matchKeys?.length > 1) {
    return false
  }
}
