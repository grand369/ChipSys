<template>
  <div v-if="isShowBreadcrumb" class="layout-navbars-breadcrumb">
    <SvgIcon class="layout-navbars-breadcrumb-icon" :name="getIconName" :size="16" @click="onThemeConfigChange" />
    <el-breadcrumb class="layout-navbars-breadcrumb-hide">
      <transition-group name="breadcrumb">
        <el-breadcrumb-item v-for="(v, k) in state.breadcrumbList" :key="v.path">
          <span v-if="k === state.breadcrumbList.length - 1" class="layout-navbars-breadcrumb-span">
            <SvgIcon :name="v.meta.icon" class="layout-navbars-breadcrumb-iconfont" v-if="themeConfig.isBreadcrumbIcon" />
            <div v-if="v.meta.title">{{ $t(v.meta.title) }}</div>
            <div v-else>{{ v.meta.tagsViewName }}</div>
          </span>
          <a v-else @click.prevent="onBreadcrumbClick(v)">
            <SvgIcon :name="v.meta.icon" class="layout-navbars-breadcrumb-iconfont" v-if="themeConfig.isBreadcrumbIcon" />{{ $t(v.meta.title) }}
          </a>
        </el-breadcrumb-item>
      </transition-group>
    </el-breadcrumb>
  </div>
</template>

<script setup lang="ts" name="layoutBreadcrumb">
import { reactive, computed, onMounted, watch, ref, onBeforeMount } from 'vue'
import { onBeforeRouteUpdate, useRoute, useRouter, RouteLocationNormalized } from 'vue-router'
import { Local } from '/@/utils/storage'
import other from '/@/utils/other'
import { storeToRefs } from 'pinia'
import { useThemeConfig } from '/@/stores/themeConfig'
import { useRoutesList } from '/@/stores/routesList'
import { treeToList, listToTree, filterList } from '/@/utils/tree'
import { cloneDeep } from 'lodash-es'
import mittBus from '/@/utils/mitt'

// 定义变量内容
const stores = useRoutesList()
const storesThemeConfig = useThemeConfig()
const { themeConfig } = storeToRefs(storesThemeConfig)
const { routesList } = storeToRefs(stores)
const route = useRoute()
const router = useRouter()
const state = reactive<BreadcrumbState>({
  breadcrumbList: [],
  routeSplit: [],
  routeSplitFirst: '',
  routeSplitIndex: 1,
})
const isMobile = ref()
isMobile.value = document.body.clientWidth < 1000

const getIconName = computed(() => {
  if (isMobile.value) {
    return !themeConfig.value.isCollapse ? 'ele-Expand' : 'ele-Fold'
  } else {
    return themeConfig.value.isCollapse ? 'ele-Expand' : 'ele-Fold'
  }
})

// 动态设置经典、横向布局不显�?
const isShowBreadcrumb = computed(() => {
  const { layout, isBreadcrumb } = themeConfig.value
  if (layout === 'classic' || layout === 'transverse') return false
  else return isBreadcrumb ? true : false
})
// 面包屑点击时
const onBreadcrumbClick = (v: RouteItem) => {
  const { redirect } = v
  if (redirect) router.push(redirect)
}
// 展开/收起左侧菜单点击
const onThemeConfigChange = () => {
  themeConfig.value.isCollapse = !themeConfig.value.isCollapse
  setLocalThemeConfig()
}
// 存储布局配置
const setLocalThemeConfig = () => {
  Local.remove('themeConfig')
  Local.set('themeConfig', themeConfig.value)
}
// 处理面包屑数�?
const getBreadcrumbList = (arr: RouteItems, path: string) => {
  path = path?.toLocaleLowerCase()
  //第一次初始化时执行时,避免使用路由查找时重复执�?
  if (state.routeSplitIndex == 1) {
    //优先使用菜单判断面包屑显示，如果找不到匹配的路由菜单，则执行旧的逻辑使用地址判断
    let routeTree = listToTree(
      filterList(treeToList(cloneDeep(arr)), path, {
        filterWhere: (item: any, word: string) => {
          return item.path?.toLocaleLowerCase() === word
        },
      })
    )

    if (routeTree.length > 0) {
      //查找第一个匹配的路由，将其展开添加到面包屑�?
      const routeArr = treeToList([routeTree[0]])
      if (routeArr.length > 0) {
        routeArr.forEach((item: RouteItem, k: number) => {
          !state.breadcrumbList.find((a) => a.path === item.path) && state.breadcrumbList.push(item)
        })
        //匹配不到再使用路径去匹配
        if (state.breadcrumbList.length > 0) return
      }
    }
  }
  //不存在则使用顶级的分�?
  arr.forEach((item: RouteItem) => {
    state.routeSplit.forEach((v: string, k: number, arrs: string[]) => {
      if (state.routeSplitFirst === item.path) {
        state.routeSplitFirst += `/${arrs[state.routeSplitIndex]}`
        !state.breadcrumbList.find((a) => a.path === item.path) && state.breadcrumbList.push(item)
        state.routeSplitIndex++
        if (item.children) getBreadcrumbList(item.children, path)
      }
    })
  })
}
// 当前路由字符串切割成数组，并删除第一项空内容
const initRouteSplit = (toRoute: RouteLocationNormalized) => {
  if (!themeConfig.value.isBreadcrumb) return false
  state.breadcrumbList = []
  state.routeSplit = toRoute.path.split('/')
  state.routeSplit.shift()
  state.routeSplitFirst = `/${state.routeSplit[0]}`
  state.routeSplitIndex = 1
  getBreadcrumbList(routesList.value, toRoute.path)
  if (toRoute.name === 'home' || (toRoute.name === 'notFound' && state.breadcrumbList.length > 1)) state.breadcrumbList.shift()
  if (state.breadcrumbList.length > 0)
    state.breadcrumbList[state.breadcrumbList.length - 1].meta.tagsViewName = other.setTagsViewNameI18n(<RouteToFrom>route)
}

// 页面加载�?
onMounted(() => {
  initRouteSplit(route)
})

// 路由更新�?
onBeforeRouteUpdate((to) => {
  initRouteSplit(to)
})

// 页面加载�?
onBeforeMount(() => {
  // 监听窗口大小改变�?适配移动�?
  mittBus.on('layoutMobileResize', (res: LayoutMobileResize) => {
    // 判断是否是手机端
    isMobile.value = res.clientWidth < 1000
  })
})

watch(
  () => themeConfig.value.isBreadcrumb,
  () => {
    route && initRouteSplit(route)
  }
)
</script>

<style scoped lang="scss">
.layout-navbars-breadcrumb {
  flex: 1;
  height: inherit;
  display: flex;
  align-items: center;
  .layout-navbars-breadcrumb-icon {
    cursor: pointer;
    font-size: 18px;
    color: var(--next-bg-topBarColor);
    height: 100%;
    width: 40px;
    opacity: 0.8;
    &:hover {
      opacity: 1;
    }
  }
  .layout-navbars-breadcrumb-span {
    display: flex;
    opacity: 0.7;
    color: var(--next-bg-topBarColor);
  }
  .layout-navbars-breadcrumb-iconfont {
    font-size: 14px;
    margin-right: 5px;
  }
  :deep(.el-breadcrumb__separator) {
    opacity: 0.7;
    color: var(--next-bg-topBarColor);
  }
  :deep(.el-breadcrumb__inner a, .el-breadcrumb__inner.is-link) {
    font-weight: unset !important;
    color: var(--next-bg-topBarColor);
    &:hover {
      color: var(--el-color-primary) !important;
    }
  }
}
</style>
