<template>
  <div class="layout-parent">
    <router-view v-slot="{ Component }">
      <transition :name="setTransitionName" mode="out-in">
        <keep-alive :include="getKeepAliveNames">
          <component :is="Component" :key="state.refreshRouterViewKey" class="w100" v-show="!isIframePage" />
        </keep-alive>
      </transition>
    </router-view>
    <transition :name="setTransitionName" mode="out-in">
      <Iframes class="w100" v-show="isIframePage" :refreshKey="state.iframeRefreshKey" :name="setTransitionName" :list="state.iframeList" />
    </transition>
  </div>
</template>

<script setup lang="ts" name="layoutParentView">
import { defineAsyncComponent, computed, reactive, onBeforeMount, onUnmounted, nextTick, watch, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useKeepALiveNames } from '/@/stores/keepAliveNames'
import { useThemeConfig } from '/@/stores/themeConfig'
import { Session } from '/@/utils/storage'
import mittBus from '/@/utils/mitt'

// 引入组件
const Iframes = defineAsyncComponent(() => import('/@/layout/routerView/iframes.vue'))

// 定义变量内容
const route = useRoute()
const router = useRouter()
const storesKeepAliveNames = useKeepALiveNames()
const storesThemeConfig = useThemeConfig()
const { keepAliveNames, cachedViews } = storeToRefs(storesKeepAliveNames)
const { themeConfig } = storeToRefs(storesThemeConfig)
const state = reactive<ParentViewState>({
  refreshRouterViewKey: '', // �?iframe tagsview 右键菜单刷新�?
  iframeRefreshKey: '', // iframe tagsview 右键菜单刷新�?
  keepAliveNameList: [],
  iframeList: [],
})

// 设置主界面切换动�?
const setTransitionName = computed(() => {
  return themeConfig.value.animation
})
// 获取组件缓存列表(name�?
const getKeepAliveNames = computed(() => {
  return themeConfig.value.isTagsview ? cachedViews.value : state.keepAliveNameList
})
// 设置 iframe 显示/隐藏
const isIframePage = computed(() => {
  return route.meta.isIframe
})
// 获取 iframe 组件列表(未进行渲�?
const getIframeListRoutes = async () => {
  router.getRoutes().forEach((v) => {
    if (v.meta.isIframe) {
      v.meta.isIframeOpen = false
      v.meta.loading = true
      state.iframeList.push({ ...v })
    }
  })
}
// 页面加载前，处理缓存，页面刷新时路由缓存处理
onBeforeMount(() => {
  state.keepAliveNameList = keepAliveNames.value
  mittBus.on('onTagsViewRefreshRouterView', (fullPath: string) => {
    state.keepAliveNameList = keepAliveNames.value.filter((name: string) => route.name !== name)
    state.refreshRouterViewKey = ''
    state.iframeRefreshKey = ''
    nextTick(() => {
      state.refreshRouterViewKey = fullPath
      state.iframeRefreshKey = fullPath
      state.keepAliveNameList = keepAliveNames.value
    })
  })
})
// 页面加载�?
onMounted(() => {
  getIframeListRoutes()
  nextTick(() => {
    setTimeout(() => {
      if (themeConfig.value.isCacheTagsView) {
        let tagsViewArr: RouteItem[] = Session.get('tagsViewList') || []
        cachedViews.value = tagsViewArr.filter((item) => item.meta?.isKeepAlive).map((item) => item.name as string)
      }
    }, 0)
  })
})
// 页面卸载�?
onUnmounted(() => {
  mittBus.off('onTagsViewRefreshRouterView', () => {})
})
// 监听路由变化，防�?tagsView 多标签时，切换动画消�?
watch(
  () => route.fullPath,
  () => {
    state.refreshRouterViewKey = decodeURI(route.fullPath)
  },
  {
    immediate: true,
  }
)
</script>
