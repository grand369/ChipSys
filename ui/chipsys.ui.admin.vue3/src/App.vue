<template>
  <el-config-provider :size="getGlobalComponentSize" :locale="getGlobalI18n">
    <router-view v-show="getLockScreen" />
    <LockScreen v-if="themeConfig.isLockScreen" />
    <Setings ref="setingsRef" v-show="getLockScreen" />
    <CloseFull v-if="!themeConfig.isLockScreen" />
    <Upgrade v-if="getVersion" />
  </el-config-provider>
</template>

<script setup lang="ts" name="app">
import { useI18n } from 'vue-i18n'
import { useTagsViewRoutes } from '/@/stores/tagsViewRoutes'
import { useThemeConfig } from '/@/stores/themeConfig'
import other from '/@/utils/other'
import { Local, Session } from '/@/utils/storage'
import mittBus from '/@/utils/mitt'
import setIntroduction from '/@/utils/setIconfont'

// 引入组件
const LockScreen = defineAsyncComponent(() => import('/@/layout/lockScreen/index.vue'))
const Setings = defineAsyncComponent(() => import('/@/layout/navBars/topBar/setings.vue'))
const CloseFull = defineAsyncComponent(() => import('/@/layout/navBars/topBar/closeFull.vue'))
const Upgrade = defineAsyncComponent(() => import('/@/layout/upgrade/index.vue'))

// 定义变量内容
const setingsRef = useTemplateRef('setingsRef')
const { messages, locale } = useI18n()
const route = useRoute()
const stores = useTagsViewRoutes()
const storesThemeConfig = useThemeConfig()
const { themeConfig } = storeToRefs(storesThemeConfig)

// 设置锁屏时组件显示隐�?
const getLockScreen = computed(() => {
  // 防止锁屏后，刷新出现不相关界�?
  return themeConfig.value.isLockScreen ? themeConfig.value.lockScreenTime > 1 : themeConfig.value.lockScreenTime >= 0
})

// 获取版本�?
const getVersion = computed(() => {
  let isVersion = false
  if (route.path !== '/login') {
    // @ts-ignore
    const currentVersion = __NEXT_VERSION__
    const lastVersion = Local.get('version')
    if (!lastVersion) {
      Local.set('version', currentVersion)
    } else if (lastVersion !== currentVersion) {
      isVersion = true
    }
  }
  return isVersion
})
// 获取全局组件大小
const getGlobalComponentSize = computed(() => {
  return other.globalComponentSize()
})
// 获取全局 i18n
const getGlobalI18n = computed(() => {
  return messages.value[locale.value]
})
// 设置初始化，防止刷新时恢复默�?
onBeforeMount(() => {
  // 设置批量第三�?icon 图标
  setIntroduction.cssCdn()
  // 设置批量第三�?js
  setIntroduction.jsCdn()
})
// 页面加载�?
onMounted(() => {
  nextTick(() => {
    // 监听布局�?置弹窗点击打开
    mittBus.on('openSetingsDrawer', () => {
      setingsRef.value?.openDrawer()
    })
    // 获取缓存中的布局配置
    if (Local.get('themeConfig')) {
      storesThemeConfig.setThemeConfig({ themeConfig: Local.get('themeConfig') })
      document.documentElement.style.cssText = Local.get('themeConfigStyle')
    }
    // 获取缓存中的全屏配置
    if (Session.get('isTagsViewCurrenFull')) {
      stores.setCurrenFullscreen(Session.get('isTagsViewCurrenFull'))
    }
  })
})
// 页面销毁时，关闭监听布局配置/i18n监听
onUnmounted(() => {
  mittBus.off('openSetingsDrawer', () => {})
})
// 监听路由的变化，设置网站标题
watch(
  () => route.path,
  () => {
    other.useTitle()
  },
  {
    deep: true,
  }
)
</script>
