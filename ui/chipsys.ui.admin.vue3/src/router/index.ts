import { createRouter, createWebHistory, createWebHashHistory } from 'vue-router'
import NProgress from 'nprogress'
import 'nprogress/nprogress.css'
import pinia from '/@/stores/index'
import { storeToRefs } from 'pinia'
import { useKeepALiveNames } from '/@/stores/keepAliveNames'
import { useRoute } from '/@/stores/route'
import { useRoutesList } from '/@/stores/routesList'
import { useThemeConfig } from '/@/stores/themeConfig'
import { Session } from '/@/utils/storage'
import { staticRoutes, notFoundAndNoPower } from '/@/router/route'
import { initFrontEndControlRoutes } from '/@/router/frontEnd'
import { initBackEndControlRoutes } from '/@/router/backEnd'
import { useUserInfo } from '/@/stores/userInfo'
import { ElMessage } from 'element-plus'

/**
 * 1、前端控制路由时：isRequestRoutes �?false，需要写 roles，需要走 setFilterRoute 方法�?
 * 2、后端控制路由时：isRequestRoutes �?true，不需要写 roles，不需要走 setFilterRoute 方法），
 * 相关方法已拆解到对应�?`backEnd.ts` �?`frontEnd.ts`（他们互不影响，不需要同时改 2 个文件）�?
 * 特别说明�?
 * 1、前端控制：路由菜单由前端去写（无菜单管理界面，有角色管理界面），角色管理中�?roles 属性，需返回�?userInfo 中�?
 * 2、后端控制：路由菜单由后端返回（有菜单管理界面、有角色管理界面�?
 */

// 读取 `/src/stores/themeConfig.ts` 是否开启后端控制路由配�?
const storesThemeConfig = useThemeConfig(pinia)
const { themeConfig } = storeToRefs(storesThemeConfig)
const { isRequestRoutes, isCreateWebHistory } = themeConfig.value

/**
 * 创建一个可以被 Vue 应用程序使用的路由实�?
 * @method createRouter(options: RouterOptions): Router
 * @link 参考：https://next.router.vuejs.org/zh/api/#createrouter
 */
export const router = createRouter({
  history: isCreateWebHistory ? createWebHistory() : createWebHashHistory(),
  /**
   * 说明�?
   * 1、notFoundAndNoPower 默认添加 404�?01 界面，防止一直提�?No match found for location with path 'xxx'
   * 2、backEnd.ts(后端控制路由)、frontEnd.ts(前端控制路由) 中也需要加 notFoundAndNoPower 404�?01 界面�?
   *    防止 404�?01 不在 layout 布局中，不设置的话，404�?01 界面将全屏显�?
   */
  routes: [...notFoundAndNoPower, ...staticRoutes],
})

/**
 * 路由多级嵌套数组处理成一维数�?
 * @param arr 传入路由菜单数据数组
 * @returns 返回处理后的一维路由菜单数�?
 */
export function formatFlatteningRoutes(arr: any) {
  if (arr.length <= 0) return false
  for (let i = 0; i < arr.length; i++) {
    if (arr[i].children) {
      arr = arr.slice(0, i + 1).concat(arr[i].children, arr.slice(i + 1))
    }
  }
  return arr
}

/**
 * 一维数组处理成多级嵌套数组（只保留二级：也就是二级以上全部处理成只有二级，keep-alive 支持二级缓存�?
 * @description isKeepAlive 处理 `name` 值，进行缓存。顶级关闭，全部不缓�?
 * @link 参考：https://v3.cn.vuejs.org/api/built-in-components.html#keep-alive
 * @param arr 处理后的一维路由菜单数�?
 * @returns 返回将一维数组重新处理成 `定义动态路由（dynamicRoutes）` 的格�?
 */
export function formatTwoStageRoutes(arr: any) {
  if (arr.length <= 0) return false
  const newArr: any = []
  const cacheList: Array<string> = []
  arr.forEach((v: any) => {
    if (v.path === '/') {
      newArr.push({ component: v.component, name: v.name, path: v.path, redirect: v.redirect, meta: v.meta, children: [] })
    } else {
      // 判断是否是动态路由（xx/:id/:name），用于 tagsView 等中使用
      if (v.path?.indexOf('/:') > -1) {
        v.meta['isDynamic'] = true
        v.meta['isDynamicPath'] = v.path
      }
      newArr[0].children.push({ ...v })
      // �?name 值，keep-alive �?include 使用，实现路由的缓存
      // 路径�?@/layout/routerView/parent.vue
      if (newArr[0].meta?.isKeepAlive && v.meta?.isKeepAlive) {
        cacheList.push(v.name)
        const stores = useKeepALiveNames(pinia)
        stores.setCacheKeepAlive(cacheList)
      }
    }
  })
  return newArr
}

// 路由加载�?
router.beforeEach(async (to, from, next) => {
  NProgress.configure({ showSpinner: false })
  if (to.meta.title) NProgress.start()
  const storesUseUserInfo = useUserInfo(pinia)
  const token = storesUseUserInfo.getToken()
  if (to.meta.isPublic && !token) {
    next()
    NProgress.done()
  } else {
    if (!token) {
      next(`/login?redirect=${to.path}&params=${JSON.stringify(to.query ? to.query : to.params)}`)
      storesUseUserInfo.removeTokenInfo()
      Session.clear()
      NProgress.done()
    } else if (token && to.path === '/login') {
      next('/')
      NProgress.done()
    } else {
      const storesRoute = useRoute(pinia)
      storesRoute.setRouteTo({ to: { path: to.path, params: JSON.stringify(to.query ? to.query : to.params) } })
      const storesRoutesList = useRoutesList(pinia)
      const { routesList } = storeToRefs(storesRoutesList)
      if (routesList.value.length === 0) {
        if (isRequestRoutes) {
          // 后端控制路由：路由数据初始化，防止刷新时丢失
          const isNoPower = await initBackEndControlRoutes()
          if (isNoPower) {
            ElMessage.warning('抱歉，您没有分配权限，请联系管理�?)
            storesUseUserInfo.removeTokenInfo()
            Session.clear()
          }
          // 解决刷新时，一直跳 404 页面问题，关联问�?No match found for location with path 'xxx'
          // to.query 防止页面刷新时，普通路由带参数时，参数丢失。动态路由（xxx/:id/:name"）isDynamic 无需处理
          next({ path: to.path, query: to.query })
        } else {
          await initFrontEndControlRoutes()
          next({ path: to.path, query: to.query })
        }
      } else {
        next()
      }
    }
  }
})

// 路由加载�?
router.afterEach(() => {
  NProgress.done()
})

// 导出路由
export default router
