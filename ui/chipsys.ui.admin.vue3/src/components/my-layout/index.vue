<template>
  <div class="my-layout h100" :style="`position: ${state.isMobile ? 'relative' : 'absolute'}`">
    <slot></slot>
  </div>
</template>

<script lang="ts" setup name="my-layout">
import { reactive, onBeforeMount } from 'vue'
import mittBus from '/@/utils/mitt'

const state = reactive({
  isMobile: document.body.clientWidth < 1000,
})

// 页面加载�?
onBeforeMount(() => {
  // 监听窗口大小改变�?适配移动�?
  mittBus.on('layoutMobileResize', (res: LayoutMobileResize) => {
    // 判断是否是手机端
    state.isMobile = res.clientWidth < 1000
  })
})
</script>
