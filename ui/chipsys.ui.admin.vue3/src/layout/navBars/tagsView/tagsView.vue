<template>
  <div class="layout-navbars-tagsview" :class="{ 'layout-navbars-tagsview-shadow': getThemeConfig.layout === 'classic' }">
    <el-scrollbar ref="scrollbarRef" @wheel.prevent="onHandleScroll">
      <ul class="layout-navbars-tagsview-ul" :class="setTagsStyle" ref="tagsUlRef">
        <li
          v-for="(v, k) in state.tagsViewList"
          :key="k"
          class="layout-navbars-tagsview-ul-li"
          :data-url="v.url"
          :class="{ 'is-active': isActive(v) }"
          @contextmenu.prevent="onContextmenu(v, $event)"
          @mousedown="onMousedownMenu(v, $event)"
          @click="onTagsClick(v, k)"
          :ref="
            (el) => {
              if (el) tagsRefs[k] = el
            }
          "
        >
          <i class="iconfont icon-webicon318 layout-navbars-tagsview-ul-li-iconfont" v-if="isActive(v)"></i>
          <SvgIcon :name="v.meta.icon" v-if="!isActive(v) && getThemeConfig.isTagsviewIcon" class="pr5" />
          <span>{{ setTagsViewNameI18n(v) }}</span>
          <template v-if="isActive(v)">
            <SvgIcon
              name="ele-RefreshRight"
              class="ml5 layout-navbars-tagsview-ul-li-refresh"
              @click.stop="refreshCurrentTagsView($route.fullPath)"
            />
            <SvgIcon
              name="ele-Close"
              class="layout-navbars-tagsview-ul-li-icon layout-icon-active"
              v-if="!v.meta.isAffix"
              @click.stop="closeCurrentTagsView(getThemeConfig.isShareTagsView ? v.path : v.url)"
            />
          </template>
          <SvgIcon
            name="ele-Close"
            class="layout-navbars-tagsview-ul-li-icon layout-icon-three"
            v-if="!v.meta.isAffix"
            @click.stop="closeCurrentTagsView(getThemeConfig.isShareTagsView ? v.path : v.url)"
          />
        </li>
      </ul>
    </el-scrollbar>
    <Contextmenu :dropdown="state.dropdown" ref="contextmenuRef" @currentContextmenuClick="onCurrentContextmenuClick" />
  </div>
</template>

<script setup lang="ts" name="layoutTagsView">
import { defineAsyncComponent, reactive, onMounted, computed, ref, nextTick, onBeforeUpdate, onBeforeMount, onUnmounted, watch } from 'vue'
import { useRoute, useRouter, onBeforeRouteUpdate } from 'vue-router'
import Sortable from 'sortablejs'
import { ElMessage } from 'element-plus'
import { storeToRefs } from 'pinia'
import { useTagsViewRoutes } from '/@/stores/tagsViewRoutes'
import { useThemeConfig } from '/@/stores/themeConfig'
import { useKeepALiveNames } from '/@/stores/keepAliveNames'
import { useRoutesList } from '/@/stores/routesList'
import { Session } from '/@/utils/storage'
import { isObjectValueEqual } from '/@/utils/arrayOperation'
import other from '/@/utils/other'
import mittBus from '/@/utils/mitt'

// 引入组件
const Contextmenu = defineAsyncComponent(() => import('/@/layout/navBars/tagsView/contextmenu.vue'))

// 定义变量内容
const tagsRefs = ref<RefType>([])
const scrollbarRef = ref()
const contextmenuRef = ref()
const tagsUlRef = ref()
const stores = useTagsViewRoutes()
const storesThemeConfig = useThemeConfig()
const storesTagsViewRoutes = useTagsViewRoutes()
const storesRoutesList = useRoutesList()
const { themeConfig } = storeToRefs(storesThemeConfig)
const { tagsViewRoutes } = storeToRefs(storesTagsViewRoutes)
const { routesList } = storeToRefs(storesRoutesList)
const storesKeepALiveNames = useKeepALiveNames()
const route = useRoute()
const router = useRouter()
const state = reactive<TagsViewState>({
  routeActive: '',
  routePath: route.path,
  dropdown: { x: '', y: '' },
  sortable: '',
  tagsRefsIndex: 0,
  tagsViewList: [],
  tagsViewRoutesList: [],
})

// 动态设�?tagsView 风格样式
const setTagsStyle = computed(() => {
  return themeConfig.value.tagsStyle
})
// 获取布局配置信息
const getThemeConfig = computed(() => {
  return themeConfig.value
})
// 设置 自定�?tagsView 名称�?自定�?tagsView 名称国际�?
const setTagsViewNameI18n = computed(() => {
  return (v: RouteItem) => {
    return other.setTagsViewNameI18n(v)
  }
})
// 设置 tagsView 高亮
const isActive = (v: RouteItem) => {
  if (getThemeConfig.value.isShareTagsView) {
    return v.path === state.routePath
  } else {
    if ((v.query && Object.keys(v.query).length) || (v.params && Object.keys(v.params).length)) {
      // 普通传�?
      return v.url ? v.url === state.routeActive : v.path === state.routeActive
    } else {
      // 通过 name 传参，params 取值，刷新页面参数消失
      return v.path === state.routePath
    }
  }
}
// 存储 tagsViewList 到浏览器临时缓存中，页面刷新时，保留记录
const addBrowserSetSession = (tagsViewList: Array<object>) => {
  Session.set('tagsViewList', tagsViewList)
}
// 获取 pinia 中的 tagsViewRoutes 列表
const getTagsViewRoutes = async () => {
  state.routeActive = await setTagsViewHighlight(route)
  state.routePath = (await route.meta.isDynamic) ? route.meta.isDynamicPath : route.path
  state.tagsViewList = []
  state.tagsViewRoutesList = tagsViewRoutes.value
  initTagsView()
}
// pinia 中获取路由信息：如果是设置了固定的（isAffix），进行初始化显�?
const initTagsView = async () => {
  if (Session.get('tagsViewList') && getThemeConfig.value.isCacheTagsView) {
    state.tagsViewList = await Session.get('tagsViewList')
  } else {
    await state.tagsViewRoutesList.map((v: RouteItem) => {
      if (v.meta?.isAffix && !v.meta.isHide) {
        v.url = setTagsViewHighlight(v)
        state.tagsViewList.push({ ...v })
        storesKeepALiveNames.addCachedView(v)
      }
    })
    await addTagsView(route.path, <RouteToFrom>route)
  }
  // 初始化当前元�?li)的下�?
  getTagsRefsIndex(getThemeConfig.value.isShareTagsView ? state.routePath : state.routeActive)
}
// 处理可开启多标签详情，单标签详情（动态路由（xxx/:id/:name"），普通路由处理）
const solveAddTagsView = async (path: string, to?: RouteToFrom) => {
  let isDynamicPath = to?.meta?.isDynamic ? to.meta.isDynamicPath : path
  let current = state.tagsViewList.filter(
    (v: RouteItem) =>
      v.path === isDynamicPath &&
      isObjectValueEqual(
        to?.meta?.isDynamic ? (v.params ? v.params : null) : v.query ? v.query : null,
        to?.meta?.isDynamic ? (to?.params ? to?.params : null) : to?.query ? to?.query : null
      )
  )
  if (current.length <= 0) {
    // 防止：Avoid app logic that relies on enumerating keys on a component instance. The keys will be empty in production mode to avoid performance overhead.
    let findItem = state.tagsViewRoutesList.find((v: RouteItem) => v.path === isDynamicPath)
    if (!findItem) return false
    if (findItem.meta.isAffix) return false
    if (findItem.meta.isLink && !findItem.meta.isIframe) return false
    to?.meta?.isDynamic ? (findItem.params = to.params) : (findItem.query = to?.query)
    findItem.url = setTagsViewHighlight(findItem)
    state.tagsViewList.push({ ...findItem })
    await storesKeepALiveNames.addCachedView(findItem)
    addBrowserSetSession(state.tagsViewList)
  }
}
// 处理单标签时，第二次的值未覆盖第一次的 tagsViewList 值（Session Storage�?
const singleAddTagsView = (path: string, to?: RouteToFrom) => {
  let isDynamicPath = to?.meta?.isDynamic ? to.meta.isDynamicPath : path
  state.tagsViewList.forEach((v) => {
    if (
      v.path === isDynamicPath &&
      !isObjectValueEqual(
        to?.meta?.isDynamic ? (v.params ? v.params : null) : v.query ? v.query : null,
        to?.meta?.isDynamic ? (to?.params ? to?.params : null) : to?.query ? to?.query : null
      )
    ) {
      to?.meta?.isDynamic ? (v.params = to.params) : (v.query = to?.query)
      v.url = setTagsViewHighlight(v)
      addBrowserSetSession(state.tagsViewList)
    }
  })
}
// 1、添�?tagsView：未设置隐藏（isHide）也添加到在 tagsView 中（可开启多标签详情，单标签详情�?
const addTagsView = (path: string, to?: RouteToFrom) => {
  if (to?.meta?.isDir) {
    return
  }
  // 防止拿取不到路由信息
  nextTick(async () => {
    let item: RouteItem
    if (to?.meta?.isDynamic) {
      // 动态路由（xxx/:id/:name"）：参数不同，开启多�?tagsview
      if (!getThemeConfig.value.isShareTagsView) await solveAddTagsView(path, to)
      else await singleAddTagsView(path, to)
      if (state.tagsViewList.some((v: RouteItem) => v.path === to?.meta?.isDynamicPath)) {
        // 防止首次进入界面�?登录进入) tagsViewList 不存浏览器中
        addBrowserSetSession(state.tagsViewList)
        return false
      }
      item = state.tagsViewRoutesList.find((v: RouteItem) => v.path === to?.meta?.isDynamicPath)
    } else {
      // 普通路由：参数不同，开启多�?tagsview
      if (!getThemeConfig.value.isShareTagsView) await solveAddTagsView(path, to)
      else await singleAddTagsView(path, to)
      if (state.tagsViewList.some((v: RouteItem) => v.path === path)) {
        // 防止首次进入界面�?登录进入) tagsViewList 不存浏览器中
        addBrowserSetSession(state.tagsViewList)
        return false
      }
      item = state.tagsViewRoutesList.find((v: RouteItem) => v.path === path)
    }
    if (!item) return false
    if (item?.meta?.isLink && !item.meta.isIframe) return false
    if (to?.meta?.isDynamic) item.params = to?.params ? to?.params : route.params
    else item.query = to?.query ? to?.query : route.query
    item.url = setTagsViewHighlight(item)
    await storesKeepALiveNames.addCachedView(item)
    await state.tagsViewList.push({ ...item })
    await addBrowserSetSession(state.tagsViewList)
  })
}
// 2、刷新当�?tagsView�?
const refreshCurrentTagsView = async (fullPath: string) => {
  const decodeURIPath = decodeURI(fullPath)
  let item: RouteToFrom = {}
  state.tagsViewList.forEach((v: RouteItem) => {
    v.transUrl = transUrlParams(v)
    if (v.transUrl) {
      if (v.transUrl === transUrlParams(v)) item = v
    } else {
      if (v.path === decodeURIPath) item = v
    }
  })
  if (!item) return false
  await storesKeepALiveNames.delCachedView(item)
  mittBus.emit('onTagsViewRefreshRouterView', fullPath)
  if (item.meta?.isKeepAlive) storesKeepALiveNames.addCachedView(item)
}
// 3、关闭当�?tagsView：如果是设置了固定的（isAffix），不可以关�?
const closeCurrentTagsView = (path: string) => {
  state.tagsViewList.map((v: RouteItem, k: number, arr: RouteItems) => {
    if (!v.meta?.isAffix) {
      if (getThemeConfig.value.isShareTagsView ? v.path === path : v.url === path) {
        storesKeepALiveNames.delCachedView(v)
        state.tagsViewList.splice(k, 1)
        setTimeout(() => {
          if (state.tagsViewList.length === k && getThemeConfig.value.isShareTagsView ? state.routePath === path : state.routeActive === path) {
            // 最后一个且高亮�?
            if (arr[arr.length - 1].meta.isDynamic) {
              // 动态路由（xxx/:id/:name"�?
              if (k !== arr.length) router.push({ name: arr[k].name, params: arr[k].params })
              else router.push({ name: arr[arr.length - 1].name, params: arr[arr.length - 1].params })
            } else {
              // 普通路�?
              if (k !== arr.length) router.push({ path: arr[k].path, query: arr[k].query })
              else router.push({ path: arr[arr.length - 1].path, query: arr[arr.length - 1].query })
            }
          } else {
            // 非最后一个且高亮时，跳转到下一�?
            if (state.tagsViewList.length !== k && getThemeConfig.value.isShareTagsView ? state.routePath === path : state.routeActive === path) {
              if (arr[k].meta.isDynamic) {
                // 动态路由（xxx/:id/:name"�?
                router.push({ name: arr[k].name, params: arr[k].params })
              } else {
                // 普通路�?
                router.push({ path: arr[k].path, query: arr[k].query })
              }
            }
          }
        }, 0)
      }
    }
  })

  addBrowserSetSession(state.tagsViewList)

  if (state.tagsViewList.length === 0) router.push({ path: '/' })
}
// 4、关闭其�?tagsView：如果是设置了固定的（isAffix），不进行关�?
const closeOtherTagsView = (path: string) => {
  if (Session.get('tagsViewList')) {
    state.tagsViewList = []
    Session.get('tagsViewList').map((v: RouteItem) => {
      if (v.meta?.isAffix && !v.meta.isHide) {
        v.url = setTagsViewHighlight(v)
        storesKeepALiveNames.delOthersCachedViews(v)
        state.tagsViewList.push({ ...v })
      }
    })
    addTagsView(path, <RouteToFrom>route)
    addBrowserSetSession(state.tagsViewList)
  }
}
// 5、关闭全�?tagsView：如果是设置了固定的（isAffix），不进行关�?
const closeAllTagsView = () => {
  if (Session.get('tagsViewList')) {
    storesKeepALiveNames.delAllCachedViews()
    state.tagsViewList = []
    Session.get('tagsViewList').map((v: RouteItem) => {
      if (v.meta?.isAffix && !v.meta.isHide) {
        v.url = setTagsViewHighlight(v)
        state.tagsViewList.push({ ...v })
      }
    })
    addBrowserSetSession(state.tagsViewList)

    if (state.tagsViewList.length > 0) {
      var tagsView = state.tagsViewList[state.tagsViewList.length - 1]
      if (tagsView.meta.isDynamic) {
        // 动态路由（xxx/:id/:name"�?
        router.push({ name: tagsView.name, params: tagsView.params })
      } else {
        // 普通路�?
        router.push({ path: tagsView.path, query: tagsView.query })
      }
    } else {
      router.push({ path: '/' })
    }
  }
}
// 6、开启当前页面全�?
const openCurrenFullscreen = async (path: string) => {
  const item = state.tagsViewList.find((v: RouteItem) => (getThemeConfig.value.isShareTagsView ? v.path === path : v.url === path))
  if (item.meta.isDynamic) await router.push({ name: item.name, params: item.params })
  else await router.push({ name: item.name, query: item.query })
  stores.setCurrenFullscreen(true)
}
// 当前项右键菜单点击，�?`当前点击的路由路径` 对比 `tagsView 路由数组`，取当前点击项的详细路由信息
// 防止 tagsView 非当前页演示时，操作异常
const getCurrentRouteItem = (item: RouteItem): any => {
  let resItem: RouteToFrom = {}
  state.tagsViewList.forEach((v: RouteItem) => {
    v.transUrl = transUrlParams(v)
    if (v.transUrl) {
      // 动态路由、普通路由带参数
      if (v.transUrl === transUrlParams(v) && v.transUrl === item.commonUrl) resItem = v
    } else {
      // 路由不带参数
      if (v.path === decodeURI(item.path)) resItem = v
    }
  })
  if (!resItem) return null
  else return resItem
}
// 当前项右键菜单点�?
const onCurrentContextmenuClick = async (item: RouteItem) => {
  item.commonUrl = transUrlParams(item)
  if (!getCurrentRouteItem(item)) return ElMessage({ type: 'warning', message: '请正确输入路径及完整参数（query、params�? })
  const { path, name, params, query, meta, url } = getCurrentRouteItem(item)
  switch (item.contextMenuClickId) {
    case 0:
      // 刷新当前
      if (meta.isDynamic) await router.push({ name, params })
      else await router.push({ path, query })
      refreshCurrentTagsView(route.fullPath)
      break
    case 1:
      // 关闭当前
      closeCurrentTagsView(getThemeConfig.value.isShareTagsView ? path : url)
      break
    case 2:
      // 关闭其它
      if (meta.isDynamic) await router.push({ name, params })
      else await router.push({ path, query })
      closeOtherTagsView(path)
      break
    case 3:
      // 关闭全部
      closeAllTagsView()
      break
    case 4:
      // 开启当前页面全�?
      openCurrenFullscreen(getThemeConfig.value.isShareTagsView ? path : url)
      break
  }
}
// 右键点击时：�?x,y 坐标值到子组件中（props�?
const onContextmenu = (v: RouteItem, e: MouseEvent) => {
  const { clientX, clientY } = e
  state.dropdown.x = clientX
  state.dropdown.y = clientY
  contextmenuRef.value.openContextmenu(v)
}
// 鼠标按下时，判断是鼠标中键就关闭当前 tasgview
const onMousedownMenu = (v: RouteItem, e: MouseEvent) => {
  if (!v.meta?.isAffix && e.button === 1) {
    const item = Object.assign({}, { contextMenuClickId: 1, ...v })
    onCurrentContextmenuClick(item)
  }
}
// 当前�?tagsView 项点击时
const onTagsClick = (v: RouteItem, k: number) => {
  state.tagsRefsIndex = k
  router.push(v)
}
// 处理 url，地址栏链接有参数时，tagsview 右键菜单刷新功能失效问题，感�?@ZzZz-RIPPER、@dejavuuuuu
const transUrlParams = (v: RouteItem) => {
  let params = v.query && Object.keys(v.query).length > 0 ? v.query : v.params
  if (!params) return ''
  let path = ''
  for (let [key, value] of Object.entries(params)) {
    if (v.meta?.isDynamic) path += `/${value}`
    else path += `&${key}=${value}`
  }
  // 判断是否是动态路由（xxx/:id/:name"）isDynamic
  if (v.meta?.isDynamic) {
    /**
     *
     * isFnClick 用于判断是通过方法调用，还是直接右键菜单点击（此处只针对动态路由）
     * 原因�?
     * 1、右键菜单点击时，路由的 path 还是原始定义的路由格式，如：/params/dynamic/details/:t/:id/:tagsViewName
     * 2、通过事件调用时，路由�?path 不是原始定义的路由格式，如：/params/dynamic/details/vue-next-admin/111/我是动态路由测试tagsViewName(非国际化)
     *
     * 所以右侧菜单点击时，需要处理路径拼�?v.path.split(':')[0]，得到路�?+ 参数的完整路�?
     */
    return v.isFnClick ? decodeURI(v.path) : `${v.path.split(':')[0]}${path.replace(/^\//, '')}`
  } else {
    return `${v.path}${path.replace(/^&/, '?')}`
  }
}
// 处理 tagsView 高亮（多标签详情时使用，单标签详情未使用�?
const setTagsViewHighlight = (v: RouteToFrom) => {
  let params = v.query && Object.keys(v.query).length > 0 ? v.query : v.params
  if (!params || Object.keys(params).length <= 0) return v.path
  let path = ''
  for (let i in params) {
    path += params[i]
  }
  // 判断是否是动态路由（xxx/:id/:name"�?
  return `${v.meta?.isDynamic ? v.meta.isDynamicPath : v.path}-${path}`
}
// 鼠标滚轮滚动
const onHandleScroll = (e: WheelEventType) => {
  scrollbarRef.value.$refs.wrapRef.scrollLeft += e.wheelDelta / 4
}
// tagsView 横向滚动
const tagsViewmoveToCurrentTag = () => {
  nextTick(() => {
    if (tagsRefs.value.length <= 0) return false
    // 当前 li 元素
    let liDom = tagsRefs.value[state.tagsRefsIndex]
    // 当前 li 元素下标
    let liIndex = state.tagsRefsIndex
    // 当前 ul �?li 元素总长�?
    let liLength = tagsRefs.value.length
    // 最�?li
    let liFirst = tagsRefs.value[0]
    // 最�?li
    let liLast = tagsRefs.value[tagsRefs.value.length - 1]
    // 当前滚动条的�?
    let scrollRefs = scrollbarRef.value.$refs.wrapRef
    // 当前滚动条滚动宽�?
    let scrollS = scrollRefs.scrollWidth
    // 当前滚动条偏移宽�?
    let offsetW = scrollRefs.offsetWidth
    // 当前滚动条偏移距�?
    let scrollL = scrollRefs.scrollLeft
    // 上一�?tags li dom
    let liPrevTag = tagsRefs.value[state.tagsRefsIndex - 1]
    // 下一�?tags li dom
    let liNextTag = tagsRefs.value[state.tagsRefsIndex + 1]
    // 上一�?tags li dom 的偏移距�?
    let beforePrevL = 0
    // 下一�?tags li dom 的偏移距�?
    let afterNextL = 0
    if (liDom === liFirst) {
      // 头部
      scrollRefs.scrollLeft = 0
    } else if (liDom === liLast) {
      // 尾部
      scrollRefs.scrollLeft = scrollS - offsetW
    } else {
      // 非头/尾部
      if (liIndex === 0) beforePrevL = liFirst.offsetLeft - 5
      else beforePrevL = liPrevTag?.offsetLeft - 5
      if (liIndex === liLength) afterNextL = liLast.offsetLeft + liLast.offsetWidth + 5
      else afterNextL = liNextTag.offsetLeft + liNextTag.offsetWidth + 5
      if (afterNextL > scrollL + offsetW) {
        scrollRefs.scrollLeft = afterNextL - offsetW
      } else if (beforePrevL < scrollL) {
        scrollRefs.scrollLeft = beforePrevL
      }
    }
    // 更新滚动条，防止不出�?
    scrollbarRef.value.update()
  })
}
// 获取 tagsView 的下标：用于处理 tagsView 点击时的横向滚动
const getTagsRefsIndex = (path: string | unknown) => {
  nextTick(async () => {
    // await 使用该写法，防止拿取不到 tagsViewList 列表数据不完�?
    let tagsViewList = await state.tagsViewList
    state.tagsRefsIndex = tagsViewList.findIndex((v: RouteItem) => {
      if (getThemeConfig.value.isShareTagsView) {
        return v.path === path
      } else {
        return v.url === path
      }
    })
    // 添加初始化横向滚动条移动到对应位�?
    tagsViewmoveToCurrentTag()
  })
}
// 设置 tagsView 可以进行拖拽
const initSortable = async () => {
  const el = <HTMLElement>document.querySelector('.layout-navbars-tagsview-ul')
  if (!el) return false
  state.sortable.el && state.sortable.destroy()
  state.sortable = Sortable.create(el, {
    animation: 300,
    dataIdAttr: 'data-url',
    disabled: getThemeConfig.value.isSortableTagsView ? false : true,
    onEnd: () => {
      const sortEndList: RouteItem[] = []
      state.sortable.toArray().map((val: string) => {
        state.tagsViewList.map((v: RouteItem) => {
          if (v.url === val) sortEndList.push({ ...v })
        })
      })
      addBrowserSetSession(sortEndList)
    },
  })
}
// 拖动问题
const onSortableResize = async () => {
  await initSortable()
  if (other.isMobile()) state.sortable.el && state.sortable.destroy()
}
// 页面加载�?
onBeforeMount(() => {
  // 初始化，防止手机端直接访问时还可以拖�?
  onSortableResize()
  // 拖动问题
  window.addEventListener('resize', onSortableResize)
  // 监听非本页面调用 0 刷新当前�? 关闭当前�? 关闭其它�? 关闭全部 4 当前页全�?
  mittBus.on('onCurrentContextmenuClick', (data: RouteItem) => {
    // 通过方法点击关闭 tagsView
    data.isFnClick = true
    onCurrentContextmenuClick(data)
  })
  // 监听布局配置界面开�?关闭拖拽
  mittBus.on('openOrCloseSortable', () => {
    initSortable()
  })
  // 监听布局配置开�?TagsView 共用，为了演示还原默认�?
  mittBus.on('openShareTagsView', () => {
    if (getThemeConfig.value.isShareTagsView) {
      router.push('/')
      state.tagsViewList = []
      state.tagsViewRoutesList.map((v: RouteItem) => {
        if (v.meta?.isAffix && !v.meta.isHide) {
          v.url = setTagsViewHighlight(v)
          state.tagsViewList.push({ ...v })
        }
      })
    }
  })
})
// 页面卸载�?
onUnmounted(() => {
  // 取消非本页面调用监听
  mittBus.off('onCurrentContextmenuClick', () => {})
  // 取消监听布局配置界面开�?关闭拖拽
  mittBus.off('openOrCloseSortable', () => {})
  // 取消监听布局配置开�?TagsView 共用
  mittBus.off('openShareTagsView', () => {})
  // 取消窗口 resize 监听
  window.removeEventListener('resize', onSortableResize)
})
// 页面更新�?
onBeforeUpdate(() => {
  tagsRefs.value = []
})
// 页面加载�?
onMounted(() => {
  // 初始�?pinia 中的 tagsViewRoutes 列表
  getTagsViewRoutes()
  initSortable()
})
// 路由更新时（组件内生命钩子）
onBeforeRouteUpdate(async (to) => {
  state.routeActive = setTagsViewHighlight(to)
  state.routePath = to.meta.isDynamic ? to.meta.isDynamicPath : to.path
  await addTagsView(to.path, <RouteToFrom>to)
  getTagsRefsIndex(getThemeConfig.value.isShareTagsView ? state.routePath : state.routeActive)
})
// 监听路由的变化，动态赋值给 tagsView
watch(
  () => tagsViewRoutes.value,
  (val) => {
    if (val.length === state.tagsViewRoutesList.length) {
      tagsViewmoveToCurrentTag()
      return false
    }
    getTagsViewRoutes()
  },
  {
    deep: true,
  }
)
</script>

<style scoped lang="scss">
.layout-navbars-tagsview {
  background-color: var(--el-bg-color-overlay);
  border-bottom: 1px solid var(--next-border-color-light);
  position: relative;
  z-index: 4;
  :deep(.el-scrollbar__wrap) {
    overflow-x: auto !important;
  }
  &-ul {
    list-style: none;
    margin: 0;
    padding: 0;
    height: 34px;
    display: flex;
    align-items: center;
    color: var(--el-text-color-regular);
    font-size: 12px;
    white-space: nowrap;
    padding: 0 15px;
    &-li {
      height: 26px;
      line-height: 26px;
      display: flex;
      align-items: center;
      border: 1px solid var(--el-border-color-lighter);
      padding: 0 15px;
      margin-right: 5px;
      border-radius: 2px;
      position: relative;
      z-index: 0;
      cursor: pointer;
      justify-content: space-between;
      &:hover {
        background-color: var(--el-color-primary-light-9);
        color: var(--el-color-primary);
        border-color: var(--el-color-primary-light-5);
      }
      &-iconfont {
        position: relative;
        left: -5px;
        font-size: 12px;
      }
      &-icon {
        border-radius: 100%;
        position: relative;
        height: 14px;
        width: 14px;
        text-align: center;
        line-height: 14px;
        right: -5px;
        &:hover {
          color: var(--el-color-white);
          background-color: var(--el-color-primary-light-3);
        }
      }
      .layout-icon-active {
        display: block;
      }
      .layout-icon-three {
        display: none;
      }
    }
    .is-active {
      color: var(--el-color-white);
      background: var(--el-color-primary);
      border-color: var(--el-color-primary);
      transition: border-color 3s ease;
    }
  }
  // 风格4
  .tags-style-four {
    .layout-navbars-tagsview-ul-li {
      margin-right: 0 !important;
      border: none !important;
      position: relative;
      border-radius: 3px !important;
      .layout-icon-active {
        display: none;
      }
      .layout-icon-three {
        display: block;
      }
      &:hover {
        background: none !important;
      }
    }
    .is-active {
      background: none !important;
      color: var(--el-color-primary) !important;
    }
  }
  // 风格5
  .tags-style-five {
    align-items: flex-end;
    .tags-style-five-svg {
      -webkit-mask-image:
        url('data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNzAiIGhlaWdodD0iNzAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgZmlsbD0ibm9uZSI+CgogPGc+CiAgPHRpdGxlPkxheWVyIDE8L3RpdGxlPgogIDxwYXRoIHRyYW5zZm9ybT0icm90YXRlKC0wLjEzMzUwNiA1MC4xMTkyIDUwKSIgaWQ9InN2Z18xIiBkPSJtMTAwLjExOTE5LDEwMGMtNTUuMjI4LDAgLTEwMCwtNDQuNzcyIC0xMDAsLTEwMGwwLDEwMGwxMDAsMHoiIG9wYWNpdHk9InVuZGVmaW5lZCIgc3Ryb2tlPSJudWxsIiBmaWxsPSIjRjhFQUU3Ii8+CiAgPHBhdGggZD0ibS0wLjYzNzY2LDcuMzEyMjhjMC4xMTkxOSwwIDAuMjE3MzcsMC4wNTc5NiAwLjQ3Njc2LDAuMTE5MTljMC4yMzIsMC4wNTQ3NyAwLjI3MzI5LDAuMDM0OTEgMC4zNTc1NywwLjExOTE5YzAuMDg0MjgsMC4wODQyOCAwLjM1NzU3LDAgMC40NzY3NiwwbDAuMTE5MTksMGwwLjIzODM4LDAiIGlkPSJzdmdfMiIgc3Ryb2tlPSJudWxsIiBmaWxsPSJub25lIi8+CiAgPHBhdGggZD0ibTI4LjkyMTM0LDY5LjA1MjQ0YzAsMC4xMTkxOSAwLDAuMjM4MzggMCwwLjM1NzU3bDAsMC4xMTkxOWwwLDAuMTE5MTkiIGlkPSJzdmdfMyIgc3Ryb2tlPSJudWxsIiBmaWxsPSJub25lIi8+CiAgPHJlY3QgaWQ9InN2Z180IiBoZWlnaHQ9IjAiIHdpZHRoPSIxLjMxMTA4IiB5PSI2LjgzNTUyIiB4PSItMC4wNDE3MSIgc3Ryb2tlPSJudWxsIiBmaWxsPSJub25lIi8+CiAgPHJlY3QgaWQ9InN2Z181IiBoZWlnaHQ9IjEuNzg3ODQiIHdpZHRoPSIwLjExOTE5IiB5PSI2OC40NTY1IiB4PSIyOC45MjEzNCIgc3Ryb2tlPSJudWxsIiBmaWxsPSJub25lIi8+CiAgPHJlY3QgaWQ9InN2Z182IiBoZWlnaHQ9IjQuODg2NzciIHdpZHRoPSIxOS4wNzAzMiIgeT0iNTEuMjkzMjEiIHg9IjM2LjY2ODY2IiBzdHJva2U9Im51bGwiIGZpbGw9Im5vbmUiLz4KIDwvZz4KPC9zdmc+'),
        url('data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNzAiIGhlaWdodD0iNzAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgZmlsbD0ibm9uZSI+CiA8Zz4KICA8dGl0bGU+TGF5ZXIgMTwvdGl0bGU+CiAgPHBhdGggdHJhbnNmb3JtPSJyb3RhdGUoLTg5Ljc2MjQgNy4zMzAxNCA1NS4xMjUyKSIgc3Ryb2tlPSJudWxsIiBpZD0ic3ZnXzEiIGZpbGw9IiNGOEVBRTciIGQ9Im02Mi41NzQ0OSwxMTcuNTIwODZjLTU1LjIyOCwwIC0xMDAsLTQ0Ljc3MiAtMTAwLC0xMDBsMCwxMDBsMTAwLDB6IiBjbGlwLXJ1bGU9ImV2ZW5vZGQiIGZpbGwtcnVsZT0iZXZlbm9kZCIvPgogIDxwYXRoIGQ9Im0tMC42Mzc2Niw3LjMxMjI4YzAuMTE5MTksMCAwLjIxNzM3LDAuMDU3OTYgMC40NzY3NiwwLjExOTE5YzAuMjMyLDAuMDU0NzcgMC4yNzMyOSwwLjAzNDkxIDAuMzU3NTcsMC4xMTkxOWMwLjA4NDI4LDAuMDg0MjggMC4zNTc1NywwIDAuNDc2NzYsMGwwLjExOTE5LDBsMC4yMzgzOCwwIiBpZD0ic3ZnXzIiIHN0cm9rZT0ibnVsbCIgZmlsbD0ibm9uZSIvPgogIDxwYXRoIGQ9Im0yOC45MjEzNCw2OS4wNTI0NGMwLDAuMTE5MTkgMCwwLjIzODM4IDAsMC4zNTc1N2wwLDAuMTE5MTlsMCwwLjExOTE5IiBpZD0ic3ZnXzMiIHN0cm9rZT0ibnVsbCIgZmlsbD0ibm9uZSIvPgogIDxyZWN0IGlkPSJzdmdfNCIgaGVpZ2h0PSIwIiB3aWR0aD0iMS4zMTEwOCIgeT0iNi44MzU1MiIgeD0iLTAuMDQxNzEiIHN0cm9rZT0ibnVsbCIgZmlsbD0ibm9uZSIvPgogIDxyZWN0IGlkPSJzdmdfNSIgaGVpZ2h0PSIxLjc4Nzg0IiB3aWR0aD0iMC4xMTkxOSIgeT0iNjguNDU2NSIgeD0iMjguOTIxMzQiIHN0cm9rZT0ibnVsbCIgZmlsbD0ibm9uZSIvPgogIDxyZWN0IGlkPSJzdmdfNiIgaGVpZ2h0PSI0Ljg4Njc3IiB3aWR0aD0iMTkuMDcwMzIiIHk9IjUxLjI5MzIxIiB4PSIzNi42Njg2NiIgc3Ryb2tlPSJudWxsIiBmaWxsPSJub25lIi8+CiA8L2c+Cjwvc3ZnPg=='),
        url("data:image/svg+xml,<svg xmlns='http://www.w3.org/2000/svg'><rect rx='8' width='100%' height='100%' fill='%23F8EAE7'/></svg>");
      -webkit-mask-size:
        18px 30px,
        20px 30px,
        calc(100% - 30px) calc(100% + 17px);
      -webkit-mask-position:
        right bottom,
        left bottom,
        center top;
      -webkit-mask-repeat: no-repeat;
    }
    .layout-navbars-tagsview-ul-li {
      padding: 0 5px;
      border-width: 15px 27px 15px;
      border-style: solid;
      border-color: transparent;
      margin: 0 -15px;
      .layout-icon-active,
      .layout-navbars-tagsview-ul-li-iconfont,
      .layout-navbars-tagsview-ul-li-refresh {
        display: none;
      }
      .layout-icon-three {
        display: block;
      }
      &:hover {
        @extend .tags-style-five-svg;
        background: var(--el-color-primary-light-9);
        color: unset;
      }
    }
    .is-active {
      @extend .tags-style-five-svg;
      background: var(--el-color-primary-light-9) !important;
      color: var(--el-color-primary) !important;
      z-index: 1;
    }
  }
}
.layout-navbars-tagsview-shadow {
  box-shadow: rgb(0 21 41 / 4%) 0px 1px 4px;
}
</style>
