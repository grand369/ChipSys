<template>
  <div v-show="!isTagsViewCurrenFull" class="layout-columns-aside my-flex-column">
    <div v-if="setShowLogo" class="layout-logo">
      <img :src="logoMini" class="layout-logo-medium-img" />
    </div>
    <el-scrollbar class="my-flex-fill">
      <ul @mouseleave="onColumnsAsideMenuMouseleave()">
        <li
          v-for="(v, k) in state.columnsAsideList"
          :key="k"
          @click="onColumnsAsideMenuClick(v)"
          @mouseenter="onColumnsAsideMenuMouseenter(v, k)"
          :ref="
            (el) => {
              if (el) columnsAsideOffsetTopRefs[k] = el
            }
          "
          :class="{ 'layout-columns-active': state.liIndex === k, 'layout-columns-hover': state.liHoverIndex === k }"
          :title="$t(v.meta.title)"
        >
          <div :class="themeConfig.columnsAsideLayout" v-if="!v.meta.isLink || (v.meta.isLink && v.meta.isIframe)">
            <SvgIcon :name="v.meta.icon" />
            <div class="columns-vertical-title font12">
              {{
                $t(v.meta.title) && $t(v.meta.title).length >= 4
                  ? $t(v.meta.title).substr(0, themeConfig.columnsAsideLayout === 'columns-vertical' ? 4 : 3)
                  : $t(v.meta.title)
              }}
            </div>
          </div>
          <div :class="themeConfig.columnsAsideLayout" v-else>
            <a :href="v.meta.isLink" target="_blank">
              <SvgIcon :name="v.meta.icon" />
              <div class="columns-vertical-title font12">
                {{
                  $t(v.meta.title) && $t(v.meta.title).length >= 4
                    ? $t(v.meta.title).substr(0, themeConfig.columnsAsideLayout === 'columns-vertical' ? 4 : 3)
                    : $t(v.meta.title)
                }}
              </div>
            </a>
          </div>
        </li>
        <div ref="columnsAsideActiveRef" :class="themeConfig.columnsAsideStyle"></div>
      </ul>
    </el-scrollbar>
  </div>
</template>

<script setup lang="ts" name="layoutColumnsAside">
import { reactive, ref, onMounted, nextTick, watch, onUnmounted, computed } from 'vue'
import { useRoute, useRouter, onBeforeRouteUpdate, RouteRecordRaw } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useRoutesList } from '/@/stores/routesList'
import { useThemeConfig } from '/@/stores/themeConfig'
import { useTagsViewRoutes } from '/@/stores/tagsViewRoutes'
import mittBus from '/@/utils/mitt'
import logoMini from '/@/assets/logo-mini.svg'
import { treeToList, listToTree, filterList } from '/@/utils/tree'
import { cloneDeep } from 'lodash-es'

// 定义变量内容
const columnsAsideOffsetTopRefs = ref<RefType>([])
const columnsAsideActiveRef = ref()
const stores = useRoutesList()
const storesThemeConfig = useThemeConfig()
const storesTagsViewRoutes = useTagsViewRoutes()
const { routesList, isColumnsMenuHover, isColumnsNavHover } = storeToRefs(stores)
const { themeConfig } = storeToRefs(storesThemeConfig)
const { isTagsViewCurrenFull } = storeToRefs(storesTagsViewRoutes)
const route = useRoute()
const router = useRouter()
const state = reactive<ColumnsAsideState>({
  columnsAsideList: [],
  liIndex: 0,
  liOldIndex: null,
  liHoverIndex: null,
  liOldPath: null,
  difference: 0,
  routeSplit: [],
})

// 设置显示/隐藏 logo
const setShowLogo = computed(() => {
  let { layout, isShowLogo } = themeConfig.value
  return (isShowLogo && layout === 'defaults') || (isShowLogo && layout === 'columns')
})

// 设置菜单高亮位置移动
const setColumnsAsideMove = (k: number) => {
  if (k === undefined) return false
  state.liIndex = k
  columnsAsideActiveRef.value.style.top = `${columnsAsideOffsetTopRefs.value[k].offsetTop + state.difference}px`
}
// 菜单高亮点击事件
const onColumnsAsideMenuClick = async (v: RouteItem) => {
  let { path, redirect } = v
  if (redirect) {
    onColumnsAsideDown(v.k)
    if (route.path.startsWith(redirect)) mittBus.emit('setSendColumnsChildren', setSendChildren(redirect))
    else router.push(redirect)
  } else {
    if (v.children && v.children.length > 0) {
      const resData: MittMenu = setSendChildren(path)
      if (Object.keys(resData).length <= 0) return false
      onColumnsAsideDown(resData.item?.k)
      mittBus.emit('setSendColumnsChildren', resData)
    } else {
      onColumnsAsideDown(v.k)
      router.push(path)
    }
  }

  // 一个路由设置自动收起菜�?
  !v.children || v.children.length < 1 ? (themeConfig.value.isCollapse = true) : (themeConfig.value.isCollapse = false)
}
// 鼠标移入时，显示当前的子级菜�?
const onColumnsAsideMenuMouseenter = (v: RouteRecordRaw, k: number) => {
  if (!themeConfig.value.isColumnsMenuHoverPreload) return false
  let { path } = v
  state.liOldPath = path
  state.liOldIndex = k
  state.liHoverIndex = k
  mittBus.emit('setSendColumnsChildren', setSendChildren(path))
  stores.setColumnsMenuHover(false)
  stores.setColumnsNavHover(true)
}
// 鼠标移走时，显示原来的子级菜�?
const onColumnsAsideMenuMouseleave = async () => {
  if (!themeConfig.value.isColumnsMenuHoverPreload) return false
  await stores.setColumnsNavHover(false)
  // 添加延时器，防止拿到�?store.state.routesList 值不是最新的
  setTimeout(() => {
    if (!isColumnsMenuHover && !isColumnsNavHover) mittBus.emit('restoreDefault')
  }, 100)
}
// 设置高亮动态位�?
const onColumnsAsideDown = (k: number) => {
  nextTick(() => {
    setColumnsAsideMove(k)
  })
}
// 设置/过滤路由（非静态路�?是否显示在菜单中�?
const setFilterRoutes = () => {
  state.columnsAsideList = filterRoutesFun(routesList.value)
  const resData: MittMenu = setSendChildren(route.path)
  if (Object.keys(resData).length <= 0) return false
  onColumnsAsideDown(resData.item?.k)
  if (!resData.children || resData.children.length < 1) {
    themeConfig.value.isCollapse = true
    setTimeout(() => {
      if (state.columnsAsideList?.length > 0) onColumnsAsideMenuClick(state.columnsAsideList[0])
    }, 300)
  } else {
    themeConfig.value.isCollapse = false
    setTimeout(() => {
      mittBus.emit('setSendColumnsChildren', resData)
    }, 300)
  }
}
// 传送当前子级数据到菜单�?
const setSendChildren = (path: string) => {
  const currentPathSplit = path.split('/')
  let rootPath = `/${currentPathSplit[1]}`
  //判断是否能够找到根节�?
  if (!state.columnsAsideList.find((v) => v.path === rootPath)) {
    //不存在则使用顶级的分�?
    let routeTree = listToTree(
      filterList(treeToList(cloneDeep(state.columnsAsideList)), path, {
        filterWhere: (item: any, filterword: string) => {
          return item.path?.toLocaleLowerCase() === filterword
        },
      })
    )

    //找到根节点则使用根节�?
    if (routeTree.length > 0 && routeTree[0]?.path) {
      rootPath = routeTree[0].path
    }
  }
  let currentData: MittMenu = { children: [] }
  state.columnsAsideList.map((v: RouteItem, k: number) => {
    if (v.path === rootPath) {
      v['k'] = k
      currentData['item'] = { ...v }
      currentData['children'] = [{ ...v }]
      if (v.children) currentData['children'] = v.children
    }
  })
  return currentData
}
// 路由过滤递归函数
const filterRoutesFun = <T extends RouteItem>(arr: T[]): T[] => {
  return arr
    .filter((item: T) => !item.meta?.isHide)
    .map((item: T) => {
      item = Object.assign({}, item)
      if (item.children) item.children = filterRoutesFun(item.children)
      return item
    })
}
// tagsView 点击时，根据路由查找下标 columnsAsideList，实现左侧菜单高�?
const setColumnsMenuHighlight = (path: string) => {
  state.routeSplit = path.split('/')
  state.routeSplit.shift()
  const routeFirst = `/${state.routeSplit[0]}`
  let currentSplitRoute = state.columnsAsideList.find((v: RouteItem) => v.path === routeFirst)
  if (!currentSplitRoute) {
    let routeTree = listToTree(
      filterList(treeToList(cloneDeep(state.columnsAsideList)), path, {
        filterWhere: (item: any, filterword: string) => {
          return item.path?.toLocaleLowerCase() === filterword
        },
      })
    )

    if (routeTree.length > 0 && routeTree[0]?.path) {
      const rootPath = routeTree[0].path
      currentSplitRoute = state.columnsAsideList.find((v: RouteItem) => v.path === rootPath)
    }
  }
  if (!currentSplitRoute) return false
  // 延迟拿值，防止取不�?
  setTimeout(() => {
    onColumnsAsideDown(currentSplitRoute.k)
  }, 0)
}
// 页面加载�?
onMounted(() => {
  setFilterRoutes()
  // 销毁变量，防止鼠标再次移入时，保留了上次的记录
  mittBus.on('restoreDefault', () => {
    state.liOldIndex = null
    state.liOldPath = null
  })
})
// 页面卸载�?
onUnmounted(() => {
  mittBus.off('restoreDefault', () => {})
})
// 路由更新�?
onBeforeRouteUpdate((to) => {
  setColumnsMenuHighlight(to.path)
  mittBus.emit('setSendColumnsChildren', setSendChildren(to.path))
})
// 监听布局配置信息的变化，动态增加菜单高亮位置移动像�?
watch(
  [() => themeConfig.value.columnsAsideStyle, isColumnsMenuHover, isColumnsNavHover],
  () => {
    themeConfig.value.columnsAsideStyle === 'columnsRound' ? (state.difference = 3) : (state.difference = 0)
    if (!isColumnsMenuHover.value && !isColumnsNavHover.value) {
      state.liHoverIndex = null
      mittBus.emit('setSendColumnsChildren', setSendChildren(route.path))
    } else {
      state.liHoverIndex = state.liOldIndex
      if (!state.liOldPath) return false
      mittBus.emit('setSendColumnsChildren', setSendChildren(state.liOldPath))
    }
  },
  {
    deep: true,
  }
)
</script>

<style scoped lang="scss">
.layout-columns-aside {
  width: 70px;
  height: 100%;
  background: var(--next-bg-columnsMenuBar);
  ul {
    position: relative;
    .layout-columns-active,
    .layout-columns-active a {
      color: var(--next-color-columnsMenuBarActiveColor) !important;
      transition: 0.3s ease-in-out;
    }
    .layout-columns-hover {
      color: var(--el-color-primary);
      a {
        color: var(--el-color-primary);
      }
    }
    li {
      color: var(--next-bg-columnsMenuBarColor);
      width: 100%;
      height: 50px;
      text-align: center;
      display: flex;
      cursor: pointer;
      position: relative;
      z-index: 1;
      &:hover {
        @extend .layout-columns-hover;
      }
      .columns-vertical {
        margin: auto;
        .columns-vertical-title {
          padding-top: 1px;
        }
      }
      .columns-horizontal {
        display: flex;
        height: 50px;
        width: 100%;
        align-items: center;
        padding: 0 5px;
        i {
          margin-right: 3px;
        }
        a {
          display: flex;
          .columns-horizontal-title {
            padding-top: 1px;
          }
        }
      }
      a {
        text-decoration: none;
        color: var(--next-bg-columnsMenuBarColor);
      }
    }
    .columns-round {
      background: var(--el-color-primary);
      color: var(--el-color-white);
      position: absolute;
      left: 50%;
      top: 2px;
      height: 44px;
      width: 65px;
      transform: translateX(-50%);
      z-index: 0;
      transition: 0.3s ease-in-out;
      border-radius: 5px;
    }
    .columns-card {
      @extend .columns-round;
      top: 0;
      height: 50px;
      width: 100%;
      border-radius: 0;
    }
  }
}

.layout-logo {
  height: 50px;
  display: flex;
  align-items: center;
  justify-content: center;
  box-shadow: rgb(0 21 41 / 2%) 0px 1px 4px;
  &-medium-img {
    width: 30px;
  }
}
</style>
