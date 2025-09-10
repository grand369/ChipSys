<template>
  <el-container class="layout-container">
    <ColumnsAside />
    <el-container class="layout-columns-warp layout-container-view h100">
      <LayoutAside />
      <el-scrollbar ref="layoutScrollbarRef" class="layout-backtop">
        <LayoutHeader />
        <LayoutMain ref="layoutMainRef" />
      </el-scrollbar>
    </el-container>
  </el-container>
</template>

<script setup lang="ts" name="layoutColumns">
import { useRoute } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useThemeConfig } from '/@/stores/themeConfig'

// 引入组件
const LayoutAside = defineAsyncComponent(() => import('/@/layout/component/aside.vue'))
const LayoutHeader = defineAsyncComponent(() => import('/@/layout/component/header.vue'))
const LayoutMain = defineAsyncComponent(() => import('/@/layout/component/main.vue'))
const ColumnsAside = defineAsyncComponent(() => import('/@/layout/component/columnsAside.vue'))

// 定义变量内容
const layoutScrollbarRef = ref<RefType>('')
const layoutMainRef = useTemplateRef('layoutMainRef')
const route = useRoute()
const storesThemeConfig = useThemeConfig()
const { themeConfig } = storeToRefs(storesThemeConfig)

// 重置滚动条高�?
const updateScrollbar = () => {
  // 更新父级 scrollbar
  layoutScrollbarRef.value?.update()
  // 更新子级 scrollbar
  layoutMainRef.value?.layoutMainScrollbarRef.update()
}
// 重置滚动条高度，由于组件是异步引入的
const initScrollBarHeight = () => {
  nextTick(() => {
    setTimeout(() => {
      updateScrollbar()
      if (layoutScrollbarRef.value) layoutScrollbarRef.value.wrapRef.scrollTop = 0
      if (layoutMainRef.value) layoutMainRef.value!.layoutMainScrollbarRef.wrapRef.scrollTop = 0
    }, 500)
  })
}
// 页面加载�?
onMounted(() => {
  initScrollBarHeight()
})
// 监听路由的变化，切换界面时，滚动条置�?
watch(
  () => route.path,
  () => {
    initScrollBarHeight()
  }
)
// 监听 themeConfig 配置文件的变化，更新菜单 el-scrollbar 的高�?
watch(
  () => [themeConfig.value.isTagsview, themeConfig.value.isFixedHeader],
  () => {
    nextTick(() => {
      updateScrollbar()
    })
  },
  {
    deep: true,
  }
)
</script>
