<template>
  <div class="el-menu-horizontal-warp">
    <el-menu router :default-active="state.defaultActive" background-color="transparent" mode="horizontal">
      <template v-for="val in menuLists">
        <el-sub-menu :index="val.path" v-if="val.children && val.children.length > 0" :key="val.path">
          <template #title>
            <SvgIcon :name="val.meta.icon" />
            <span>{{ $t(val.meta.title) }}</span>
          </template>
          <SubItem :chil="val.children" />
        </el-sub-menu>
        <template v-else>
          <el-menu-item :index="val.path" :key="val.path">
            <template #title v-if="!val.meta.isLink || (val.meta.isLink && val.meta.isIframe)">
              <SvgIcon :name="val.meta.icon" />
              {{ $t(val.meta.title) }}
            </template>
            <template #title v-else>
              <a class="w100" @click.prevent="onALinkClick(val)">
                <SvgIcon :name="val.meta.icon" />
                {{ $t(val.meta.title) }}
              </a>
            </template>
          </el-menu-item>
        </template>
      </template>
    </el-menu>
  </div>
</template>

<script setup lang="ts" name="navMenuHorizontal">
import { defineAsyncComponent, reactive, computed, onBeforeMount } from 'vue'
import { useRoute, onBeforeRouteUpdate, RouteRecordRaw, useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useRoutesList } from '/@/stores/routesList'
import { useThemeConfig } from '/@/stores/themeConfig'
import other from '/@/utils/other'
import mittBus from '/@/utils/mitt'
import { treeToList, listToTree, filterList } from '/@/utils/tree'
import { cloneDeep } from 'lodash-es'

const router = useRouter()

// 引入组件
const SubItem = defineAsyncComponent(() => import('/@/layout/navMenu/subItem.vue'))

// 定义父组件传过来的�?
const props = defineProps({
  // 菜单列表
  menuList: {
    type: Array<RouteRecordRaw>,
    default: () => [],
  },
})

// 定义变量内容
const stores = useRoutesList()
const storesThemeConfig = useThemeConfig()
const { routesList } = storeToRefs(stores)
const { themeConfig } = storeToRefs(storesThemeConfig)
const route = useRoute()
const state = reactive({
  defaultActive: '' as string | undefined,
})

// 获取父级菜单数据
const menuLists = computed(() => {
  return <RouteItems>props.menuList
})
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

// 获得根菜单路�?
const getRootPath = (path: string) => {
  let rootPath = ''
  let routeTree = listToTree(
    filterList(treeToList(cloneDeep(routesList.value)), path, {
      filterWhere: (item: any, filterword: string) => {
        return item.path?.toLocaleLowerCase() === filterword
      },
    })
  )
  if (routeTree.length > 0 && routeTree[0]?.path) {
    rootPath = routeTree[0].path
  }
  return rootPath
}

// 传送当前子级数据到菜单�?
const setSendClassicChildren = (path: string) => {
  let rootPath = getRootPath(path)
  rootPath = rootPath || path
  const currentPathSplit = rootPath.split('/')
  let currentData: MittMenu = { children: [] }
  filterRoutesFun(routesList.value).map((v, k) => {
    if (v.path === `/${currentPathSplit[1]}`) {
      v['k'] = k
      currentData['item'] = { ...v }
      currentData['children'] = [{ ...v }]
      if (v.children) currentData['children'] = v.children
    }
  })

  return currentData
}
// 设置页面当前路由高亮
const setCurrentRouterHighlight = (currentRoute: RouteToFrom) => {
  const { path, meta } = currentRoute
  if (themeConfig.value.layout === 'classic') {
    let rootPath = getRootPath(path || '')
    rootPath = rootPath || path || ''
    state.defaultActive = `/${rootPath?.split('/')[1]}`
    if (!state.defaultActive || state.defaultActive === '/') router.push(routesList.value[0].path)
  } else {
    const pathSplit = meta?.isDynamic ? meta.isDynamicPath!.split('/') : path!.split('/')
    if (pathSplit.length >= 4 && meta?.isHide) state.defaultActive = pathSplit.splice(0, 3).join('/')
    else state.defaultActive = path
  }
}
// 打开外部链接
const onALinkClick = (val: RouteItem) => {
  other.handleOpenLink(val)
}
// 页面加载�?
onBeforeMount(() => {
  setCurrentRouterHighlight(route)
})
// 路由更新�?
onBeforeRouteUpdate((to) => {
  // 修复：https://gitee.com/lyt-top/vue-next-admin/issues/I3YX6G
  setCurrentRouterHighlight(to)
  // 修复经典布局开启切割菜单时，点击tagsView后左侧导航菜单数据不变的问题
  let { layout, isClassicSplitMenu } = themeConfig.value
  if (layout === 'classic' && isClassicSplitMenu) {
    mittBus.emit('setSendClassicChildren', setSendClassicChildren(to.path))
  }
})
</script>

<style scoped lang="scss">
.el-menu-horizontal-warp {
  flex: 1;
  overflow: hidden;
  margin-right: 30px;
  :deep(.el-scrollbar__bar.is-vertical) {
    display: none;
  }
  :deep(a) {
    width: 100%;
  }
  .el-menu.el-menu--horizontal {
    display: flex;
    height: 100%;
    width: 100%;
    box-sizing: border-box;
  }
}
</style>
